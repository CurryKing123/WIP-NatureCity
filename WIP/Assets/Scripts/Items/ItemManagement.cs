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
    public void RemResToTree(int itemId, int resId)
    {
        StartCoroutine(RemFromPlayerInv(itemId, resId));
    }
    public void RemoveItemFromInv(int charId, int itemId)
    {
        StartCoroutine(RemItem(charId, itemId));
    }
    public void AddToHomeTree(string dH, int resId)
    {
        StartCoroutine(AddToHome(dH, resId));
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
                Items items = new Items();
                items = JsonUtility.FromJson<Items>(dH);
                itemType = items.data[0].item_type;
                

                if (itemType == "resource")
                {
                    int resId = items.data[0].res_id;
                    RemResToTree(itemId, resId);
                }
            }
        }
    }


    IEnumerator RemFromPlayerInv(int itemId, int resId)
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
                Debug.Log($"Removing from inventory: {dH}");
                Inventory inv = new Inventory();
                inv = JsonUtility.FromJson<Inventory>(dH);
                resAmount = inv.data[0].item_amount;
                player.playerInventory -= resAmount;

                Debug.Log($"ResourceId: {resId}");
                //Get Home Tree Data
                CallHomeTree(resId);
                //Remove From Player Inventory
                RemoveItemFromInv(charId, itemId);
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
                Debug.Log(dH);
                AddToHomeTree(dH, resId);
            }
        }
    }

    //Add to global inventory
    IEnumerator AddToHome(string dH, int resId)
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
