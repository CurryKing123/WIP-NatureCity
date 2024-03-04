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

    public bool isBeingGathered = false;
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
    }

    public void StartGathering(Transform tPlayer)
    {
        if (!isBeingGathered)
        {
            Debug.Log("Start Gathering...");
            waitTime = gatherTime;
            player = tPlayer;
            isBeingGathered = true;
        }
    }

    public void StopGathering()
    {
        if (isBeingGathered)
        {
            Debug.Log("Stop Gathering...");
            localTime = 0;
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

    private void GatherResource(Transform player)
    {
        PlayerController playCont = player.GetComponent<PlayerController>();
        if(resAmount > 0 && playCont.inResArea)
        {
            if (localTime < waitTime) 
            {
                //Checking to see if Player is gathering
                if (playCont.isGathering == false)
                {
                    //Stops Gathering When Clicking Away
                    StopGathering();
                }
                return;
            }

            waitTime = localTime + gatherTime;
            Debug.Log("Gathered 1...");

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
                StartCoroutine(GetItemData(resId));
            }
        }
    }
    IEnumerator GetItemData(int resId)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/item/get-item-by-res?res_id={resId}"))
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
                Items item = new Items();
                item = JsonUtility.FromJson<Items>(dH);
                itemId = item.data[0].item_id;
            }
        }
    }
}