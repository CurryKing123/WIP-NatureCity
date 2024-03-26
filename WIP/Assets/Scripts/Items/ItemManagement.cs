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

public class ItemManagement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI carryCap;
    PlayerController playCont;
    public string itemType;

    public void CallEquip(string equip)
    {
        StartCoroutine(GetEquip(equip));
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
    public void AddToHomeTree(string dH, int resId, int resAmount)
    {
        StartCoroutine(AddToHome(dH, resId, resAmount));
    }
    public void CallHomeTree(int resId, int resAmount)
    {
        StartCoroutine(GetHomeTree(resId, resAmount));
    }

    IEnumerator GetEquip(string equip)
    {
        if(equip != "")
        {
            using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/item/get-item-by-name?item_name={equip}"))
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
                    
                    if(items.data[0].item_type == "bag")
                    {
                        CallBag(items.data[0].item_name);
                        Debug.Log("Getting bag");
                    }
                    else
                    {
                        PlayerController player = gameObject.GetComponent<PlayerController>();
                        carryCap.text = "Capacity: " + player.carryAmount;
                    }

                }
            }
        }
        else
        {
            Debug.Log("Nothing There");
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
                int resAmount = inv.data[0].item_amount;
                player.playerInventory -= resAmount;

                //Get Home Tree Data
                CallHomeTree(resId, resAmount);
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

    IEnumerator GetHomeTree(int resId, int resAmount)
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
                AddToHomeTree(dH, resId, resAmount);
            }
        }
    }

    //Add to global inventory
    IEnumerator AddToHome(string dH, int resId, int resAmount)
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
