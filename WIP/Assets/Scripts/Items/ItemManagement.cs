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
    public void CallEquip(string equip1)
    {
        StartCoroutine(GetEquip(equip1));
    }
    public void CallBag(string bagName)
    {
        StartCoroutine(GetBag(bagName));
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

}
