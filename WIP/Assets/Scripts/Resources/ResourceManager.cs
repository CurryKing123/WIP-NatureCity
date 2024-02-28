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
    private float localTime = 0f;
    private float waitTime = 0f;
    private Transform player;

    public void Start()
    {
        StartCoroutine(GetResNode(resNodeId));
        waitTime = gatherTime;
    }

    public void StartGathering(Transform tPlayer)
    {
        if (!isBeingGathered)
        {
            player = tPlayer;
            isBeingGathered = true;
        }
    }

    public void StopGathering()
    {
        if (!isBeingGathered)
        {
            isBeingGathered = false;
        }
    }

    void Update()
    {

        if (isBeingGathered)
        {
            localTime += Time.deltaTime;
            GatherResource(player);
        }
    }
    //public void StartGathering(Transform player)
    //{
    //    if (!isBeingGathered)
    //    {
    //        isBeingGathered = true;
    //        StartCoroutine(GatherResource(player));
    //    }
    //}
    
    //public void StopGathering(Transform player)
    //{
    //    isBeingGathered = false;
    //    StopCoroutine(GatherResource(player));
    //}

    private void GatherResource(Transform player)
    {
        PlayerController playCont = player.GetComponent<PlayerController>();
        if(resAmount > 0 && playCont.inResArea)
        {
            if (localTime < waitTime) 
            {
                return;
            }

            waitTime = localTime + gatherTime;
            Debug.Log("Gathering...");

            resAmount--;
            playCont.playerInventory++;
            playCont.AddItem(itemId);

            if (resAmount <= 0)
           {
               // Resource depleted
               Destroy(gameObject);
               Debug.Log("Gathering Finished");
               isBeingGathered = false;
               return;
           }
           else if(playCont.playerInventory == playCont.carryAmount)
           {
               Debug.Log("Inventory Full");
               isBeingGathered = false;
               return;
           }
       }
    }
    

    
    
//Gathering Resources
    //private IEnumerator GatherResource(Transform player)
    //{
    //    PlayerController playCont = player.GetComponent<PlayerController>();
    //    while(resAmount > 0)
    //    {
    //        Debug.Log("Gathering...");
    //    // Play the gathering animation
//
    //        //for(int i = 0; i <= gatherTime; i++)
    //        //{
    //        //    if(playCont.agent.speed == 0)
    //        //    {
    //        //        yield return new WaitForSeconds(i);
    //        //    }
    //        //    else
    //        //    {
    //        //        break;
    //        //    }
    //        //}
//
    //        yield return new WaitForSeconds(gatherTime);
//
//
//
    //        resAmount--;
    //        playCont.playerInventory++;
    //        playCont.AddItem(itemId);
//
    //        if (resAmount <= 0)
    //        {
    //            // Resource depleted
    //            Destroy(gameObject);
    //            Debug.Log("Gathering Finished");
    //            isBeingGathered = false;
    //            break;
    //        }
    //        else if(playCont.playerInventory == playCont.carryAmount)
    //        {
    //            Debug.Log("Inventory Full");
    //            isBeingGathered = false;
    //            break;
    //        }
    //    }
    //}

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