using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;
using System.Threading;
using System.Linq;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;

public class ItemManagement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI carryCap;
    PlayerController playCont;
    public int resAmount;
    public string itemType;
    public int resId;

    public void CallEquip(string equip1)
    {
        StartCoroutine(GetEquip(equip1));
    }
    public void CallBag(string bagName)
    {
        StartCoroutine(GetBag(bagName));
    }
    public void CallItem(int itemId)
    {
        StartCoroutine(GetItemById(itemId));
    }
    //Remove Resource To Tree
    public void RemResToTree(int itemId)
    {
        StartCoroutine(RemFromPlayerInv(itemId));
    }
    public void RemoveItemFromInv(int charId, int itemId)
    {
        StartCoroutine(RemItem(charId, itemId));
    }
    public void AddToHomeTree(string dH)
    {
        StartCoroutine(AddToHome(dH));
    }
    public void CallHomeTree(int resId)
    {
        StartCoroutine(GetHomeTree(resId));
    }

    IEnumerator GetEquip(string equip1)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/item/get-item-by-name?item_name={equip1}"))
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
                Items items = new Items();
                items = JsonUtility.FromJson<Items>(dH);
                if(items.data.Length == 0)
                {
                    StartCoroutine(wait());
                    PlayerController player = gameObject.GetComponent<PlayerController>();
                    carryCap.text = "Capacity: " + player.carryAmount;
                    Debug.Log("No Equipment Found");
                }
                else if(items.data[0].item_type == "bag")
                {
                    CallBag(items.data[0].item_name);
                }

            }
        }
    }
    //Wait for ? seconds
    IEnumerator wait()
    {
        yield return new WaitForSeconds(.2f);
    }
    IEnumerator GetBag(string bagName)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/bag/get-bag-by-name?bag_name={bagName}"))
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
                Bag bag = new Bag();
                bag = JsonUtility.FromJson<Bag>(dH);

                PlayerController player = gameObject.GetComponent<PlayerController>();
                player.carryAmount += bag.data[0].item_bag_capacity;

                carryCap.text = "Capacity: " + player.carryAmount;
            }
        }
    }

    IEnumerator GetItemById(int itemId)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/item/get-item-by-id?item_id={itemId}"))
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
                Debug.Log(dH);
                Items items = new Items();
                items = JsonUtility.FromJson<Items>(dH);
                itemType = items.data[0].item_type;
                resId = items.data[0].res_id;
                

                if (itemType == "resource")
                {
                    RemResToTree(itemId);
                }
            }
        }
    }


    IEnumerator RemFromPlayerInv(int itemId)
    {
        PlayerController player = gameObject.GetComponent<PlayerController>();
        int charId = player.charId;
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
                resAmount = inv.data[0].item_amount;
                player.playerInventory -= resAmount;
                RemoveItemFromInv(charId, itemId);
                CallHomeTree(resId);
            }
        }
    }

    IEnumerator RemItem(int charId, int itemId)
    {
        using (UnityWebRequest www = UnityWebRequest.Delete($"http://localhost:8002/inventory/delete-inv?char_id={charId}&item_id={itemId}"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Deleted item from player inventory");
            }
        }
    }

    IEnumerator GetHomeTree(int resId)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/global_inventory/get-global_inv-by-id?res_id={resId}"))
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
                AddToHomeTree(dH);
            }
        }
    }

    //Add to global inventory
    IEnumerator AddToHome(string dH)
    {
        GlobalInventory globalInv = new GlobalInventory();
        globalInv = JsonUtility.FromJson<GlobalInventory>(dH);
        globalInv.data[0].res_amount += resAmount;
        Debug.Log(globalInv.data[0].res_amount);
        string jsonUse = JsonUtility.ToJson(globalInv.data[0], true);
        using (UnityWebRequest www = UnityWebRequest.Put($"http://localhost:8002/global_inventory/put-global_inv?res_id={resId}", jsonUse))
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
                Debug.Log("Added resources successfully!");
            }
        }
    }
}
