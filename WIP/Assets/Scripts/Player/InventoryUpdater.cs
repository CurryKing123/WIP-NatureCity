using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using Unity.VisualScripting;

public class InventoryUpdater : MonoBehaviour
{
    private InventoryUI invUI;
    private ItemManagement itMan;

    public void Start()
    {
        invUI = GetComponent<InventoryUI>();
        itMan = gameObject.GetComponent<ItemManagement>();
    }

    public void CheckInv(int charId, int itemId)
    {
        StartCoroutine(CheckInvForDupe(charId, itemId));
    }
    public void AddMoreItem(int charId, int itemID, string dH)
    {
        StartCoroutine(AddMoreItemToInv(charId, itemID, dH));
    }
    public void AddNewInvItem(int charId, int itemId)
    {
        StartCoroutine(AddNewItem(charId, itemId));
    }
    public void CheckHomeInventory(int charId)
    {
        StartCoroutine(CheckHomeInv(charId));
    }
    public void InvUpdate(int charId)
    {
        StartCoroutine(GetInvUpdate(charId));
    }

    IEnumerator CheckInvForDupe(int charId, int itemId)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/inventory/get-inv-by-id2?char_id={charId}&item_id={itemId}"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) 
            {
                Debug.Log(www.error);
            }
            else
            {
                string dH = www.downloadHandler.text;
                Inventory inv = new Inventory();
                inv = JsonUtility.FromJson<Inventory>(dH);
                if (inv.data.Length == 0)
                {
                    AddNewInvItem(charId, itemId);
                    Debug.Log("Adding New Item");
                }
                else
                {
                    AddMoreItem(charId, itemId, dH);
                    Debug.Log("Adding Existing Item");
                }
            }
        }
    }

        IEnumerator AddNewItem(int charId, int itemID)
    {
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:8002/inventory/post-inv",
        "{ \"char_id\": \"" + charId + "\", \"item_id\": \"" + itemID + "\", \"item_amount\": \"" + 1 + "\" }", "application/json"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) 
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Added New Item");
                InvUpdate(charId);
            }
        }
    }

    IEnumerator AddMoreItemToInv(int charId, int itemId, string dH)
    {
        Inventory inv = new Inventory();
        inv = JsonUtility.FromJson<Inventory>(dH);
        inv.data[0].item_amount = inv.data[0].item_amount + 1;
        string jsonUse = JsonUtility.ToJson(inv.data[0], true);
        using (UnityWebRequest www = UnityWebRequest.Put($"http://localhost:8002/inventory/put-inv?char_id={charId}&item_id={itemId}", jsonUse))
        {
            www.SetRequestHeader("key", "1");
            www.SetRequestHeader("content-type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                InvUpdate(charId);
            }
        }
    }

    //Get Updated Inventory
    IEnumerator GetInvUpdate(int charId)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/inventory/get-inv-by-id?char_id={charId}"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) 
            {
                Debug.Log(www.error);
            }
            else
            {
                Inventory myInv = new Inventory();
                string dH = www.downloadHandler.text;
                invUI.InvGridUpdate(dH);
            }
        }
    }

    //Check Inventory At HomeTree

    IEnumerator CheckHomeInv(int charId)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/inventory/get-inv-by-id?char_id={charId}"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) 
            {
                Debug.Log(www.error);
            }
            else
            {
                string dH = www.downloadHandler.text;
                Inventory inv = new Inventory();
                inv = JsonUtility.FromJson<Inventory>(dH);
                for(int i = 0; i < inv.data.Length; i++)
                {
                    int itemId = inv.data[i].item_id;
                    Debug.Log($"Depositing itemId: {itemId}");
                    itMan.CallItem(itemId, charId);
                }
            }
        }
    }
}
