using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Text;
using System;
using System.IO;
using System.Threading;
using Unity.VisualScripting;
using Palmmedia.ReportGenerator.Core;


public class ResourceManager : MonoBehaviour
{

    private bool isBeingGathered = false;
    public int resNodeId;
    public int resAmount;
    public float gatherTime;
    public int resId;
    public string resType;
    public int itemId;
    public void Start()
    {

        StartCoroutine(GetResNode(resNodeId));
    }
    public void StartGathering(Transform player)
    {
        
        if (!isBeingGathered)
        {
            isBeingGathered = true;
            StartCoroutine(GatherResource(player));
        }
    }

    
    
//Gathering Resources
    private IEnumerator GatherResource(Transform player)
    {
        while(resAmount > 0)
        {
            PlayerController playCont = player.GetComponent<PlayerController>();
            Debug.Log("Gathering...");
        // Play the gathering animation


            yield return new WaitForSeconds(gatherTime);

            resAmount--;
            playCont.playerInventory++;

            if (resAmount <= 0)
            {
                // Resource depleted
                Destroy(gameObject);
                Debug.Log("Gathering Finished");
                isBeingGathered = false;
                break;
            }
            else if(playCont.playerInventory == playCont.carryAmount)
            {
                Debug.Log("Inventory Full");
                isBeingGathered = false;
                break;
            }
        }
    }

    IEnumerator GetResNode(int resNodeId)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/resource_node/get-resource_node-by-id?resource_node_id={resNodeId}"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                ResourceNodes resNodes = new ResourceNodes();
                string dH = www.downloadHandler.text;
                Debug.Log(dH);
                resNodes = JsonUtility.FromJson<ResourceNodes>(dH);
                resAmount = resNodes.data[0].resource_amount;
                gatherTime = resNodes.data[0].gathering_time;
                resId = resNodes.data[0].resource_id;
                StartCoroutine(GetResourceData(resId));
            }
        }
    }
    IEnumerator GetResourceData(int resId)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/resource/get-resource-by-id?resource_id={resId}"))
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
                Resource res = new Resource();
                res = JsonUtility.FromJson<Resource>(dH);
                resType = res.data[0].resource_type;
                StartCoroutine(GetItemData(resType));
            }
        }
    }
    IEnumerator GetItemData(string resType)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/item/get-item-by-type?item_type={resType}"))
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
                itemId = inv.data[0].item_id;
            }
        }
    }
}