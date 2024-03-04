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

public class PlayerController : MonoBehaviour
{

    public NavMeshAgent agent;
    public bool inResArea = false;
    private bool isMoving = false;
    public bool isMovingToResource = false;
    public bool isGathering = false;
    private float distFromRes;
    private Transform targetResource;
    private ResourceManager resMan;
    public Vector3 playerPos;


    public float speed;
    public int carryAmount;
    public int playerInventory;
    public int charId;
    public string charRace;
    public string invDh;
    public string equip1;
    public string equip2;
    public string equip3;
    public string equip4;
    public string equip5;




    public void CallRace(string race)
    {
        StartCoroutine(GetRace(race));
    }
    public void AddItem(int itemID)
    {
        StartCoroutine(AddItemToInv(itemID));
    }
    public void CallInv(int charId)
    {
        StartCoroutine(GetInv(charId));
    }
    public void AddMoreItem(int itemID)
    {
        StartCoroutine(AddMoreItemToInv(itemID));
    }
    public void CallInvItem(int charId, int itemId)
    {
        StartCoroutine(GetInvItem(charId, itemId));
    }
    public void DepositResource(int charId)
    {
        StartCoroutine(DepoRes(charId));
    }

    private void Start()
    {
        ItemManagement itMan = gameObject.GetComponent<ItemManagement>();
        string dH = (File.ReadAllText(Application.persistentDataPath + "CharData.json"));
        CharArray myChar = new CharArray();
        myChar = JsonUtility.FromJson<CharArray>(dH);
        charRace = myChar.data[0].character_race;
        charId = myChar.data[0].char_id;
        equip1 = myChar.data[0].equip_item_1;
        
        CallRace(charRace);
        CallInv(charId);
        itMan.CallEquip(equip1);



        agent = GetComponent<NavMeshAgent>();
        InvokeRepeating("PlayerPosition", 0f, .3f);
    }
    private void PlayerPosition()
    {
        playerPos = transform.position;
    }
    private void DistanceFromResource()
    {
        distFromRes = Vector3.Distance(targetResource.position, playerPos);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            agent.speed = speed;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            

            if (Physics.Raycast(ray, out hit))
            {
                

                if (hit.collider.CompareTag("Resource"))
                {
                    ResourceManager resource = hit.collider.GetComponent<ResourceManager>();
                    targetResource = hit.transform;
                    isMovingToResource = true;
                    InvokeRepeating("DistanceFromResource", 0f, .3f);
                    
                    
                    if (inResArea == false)
                    {
                        MoveToResource(targetResource);
                        Debug.Log("Moving to resource");
                    }
                    else
                    {
                        if (isMovingToResource && distFromRes < 5)
                        MoveToResource(targetResource);
                        {
                            if (playerInventory == carryAmount)
                            {
                                Debug.Log("Inventory Full");
                            }
                            else
                            {
                                isGathering = true;
                                resource.StartGathering(transform);
                            }

                        }
                    }
                }
                else
                {
                    // Move the player to the clicked position
                    MovePlayer(hit.point);
                    isMovingToResource = false;
                    if (isGathering)
                    {
                        //Figure Out How To Stop Gathering Clicking Away From Resource
                        isGathering = false;
                    }
                }
            }
        }
    }


    //Gathering Trigger


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Resource"))
        {
            Debug.Log("In Resource Area");
            inResArea = true;
            if (isMovingToResource && distFromRes < 5)
                {
                    if (playerInventory == carryAmount)
                    {
                        Debug.Log("Inventory Full");
                    }
                    else
                    {
                        ResourceManager resMan = other.GetComponent<ResourceManager>();
                        isGathering = true;
                        resMan.StartGathering(transform);
                    }
                
                }
        }

        else if (other.CompareTag("HomeTree"))
        {
            DepositResource(charId);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Resource"))
        {
            ResourceManager resMan = other.GetComponent<ResourceManager>();
            resMan.StopGathering();
            Debug.Log("Out of Resource Area");
            inResArea = false;
            isGathering = false;
        }
    }
    



    
    private void MoveToResource(Transform resource)
    {
        agent.SetDestination(resource.position);
    }
    void MovePlayer(Vector3 destination)
    {
        agent.SetDestination(destination);
        isMoving = true;
    }

    //Get race data for current player
    IEnumerator GetRace(string race)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/race/get-race-by-name?race_name={race}"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) 
            {
                Debug.Log(www.error);
            }
            else
            {
                Races charRace = new Races();
                string dH = www.downloadHandler.text;
                charRace = JsonUtility.FromJson<Races>(dH);
                speed = charRace.data[0].move_speed;
                carryAmount = charRace.data[0].carry_amount;
            }
        }
    }

    IEnumerator AddItemToInv(int itemID)
    {
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:8002/inventory/post-inv",
        "{ \"char_id\": \"" + charId + "\", \"item_id\": \"" + itemID + "\", \"item_amount\": \"" + 1 + "\" }", "application/json"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) 
            {
                Debug.Log(www.error);
                CallInvItem(charId, itemID);
            }
        }
    }

        IEnumerator GetInvItem(int charId, int itemId)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/inventory/get-inv-by-id?char_id={charId}&item_id={itemId}"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) 
            {
                Debug.Log(www.error);
            }
            else
            {
                invDh = www.downloadHandler.text;
                Debug.Log(invDh);
                AddMoreItem(itemId);
            }
        }
    }

    IEnumerator AddMoreItemToInv(int itemId)
    {
        Inventory inv = new Inventory();
        inv = JsonUtility.FromJson<Inventory>(invDh);
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
            }
        }
    }

    //Get Inventory At Start
    IEnumerator GetInv(int charId)
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
                invDh = www.downloadHandler.text;
                myInv = JsonUtility.FromJson<Inventory>(invDh);
                for(int i = 0; i < myInv.data.Length; i++)
                {
                    playerInventory += myInv.data[i].item_amount;
                    Debug.Log(myInv.data[i].item_amount);
                }
            }
        }
    }

    //Check Inventory At HomeTree

    IEnumerator DepoRes(int charId)
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
                    Debug.Log("Depositing...");
                    ItemManagement itMan = gameObject.GetComponent<ItemManagement>();
                    int itemId = inv.data[i].item_id;
                    itMan.CallItem(itemId);
                }
            }
        }
    }


}