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

public class PlayerController : MonoBehaviour
{

    public bool inResArea = false;
    private bool isMoving = false;
    public bool isMovingToResource = false;
    public bool isGathering = false;
    private bool inBuildArea = false;

    public NavMeshAgent agent;
    private float distFromRes;
    private Transform targetResource;
    private ResourceManager resMan;
    private BuildingUI buildUI;
    private InventoryUI invUI;
    public Vector3 playerPos;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI pressB;
    private ItemManagement itMan;
    private GetPlayerData getPlayerData;
    private Animator anim;

    public enum ActionState {Walking, NotMoving}
    public ActionState actionState;


    private int userId;
    public float speed;
    public int carryAmount;
    public int playerInventory;
    public int charId;
    public string charRace;
    public string userName;
    private string dH;
    public string invDh;
    public string[] equip;
    //public string equip2;
    //public string equip3;
    //public string equip4;
    //public string equip5;




    
    public void CheckInv(int charId, int itemId)
    {
        StartCoroutine(CheckInvForDupe(charId, itemId));
    }
    public void AddMoreItem(int itemID)
    {
        StartCoroutine(AddMoreItemToInv(itemID));
    }
    public void AddNewInvItem(int itemId)
    {
        StartCoroutine(AddNewItem(itemId));
    }
    public void DepositResource(int charId)
    {
        StartCoroutine(DepoRes(charId));
    }


    private void Start()
    {
        ActionStateSetup();

        userId = UserId.user_id;
        getPlayerData = gameObject.GetComponent<GetPlayerData>();
        getPlayerData.CallChar(userId);
        anim = GetComponent<Animator>();

        actionState = ActionState.NotMoving;

        
        
        itMan = gameObject.GetComponent<ItemManagement>();
        buildUI = GetComponent<BuildingUI>();
        invUI = GetComponent<InventoryUI>();
        agent = GetComponent<NavMeshAgent>();
        InvokeRepeating("PlayerPosition", 0f, .3f);
    }

    public void GetPlayerData()
    {
        charRace = getPlayerData.charRace;
        charId = getPlayerData.charId;
        userName = getPlayerData.userName;
        speed = getPlayerData.speed;
        carryAmount = getPlayerData.carryAmount;

        playerName.text = userName;

        equip = getPlayerData.equip;
    }




    IEnumerator wait()
    {
        yield return new WaitForSeconds(3f);
    }

    private void PlayerPosition()
    {
        playerPos = transform.position;
    }
    private void DistanceFromResource()
    {
        if (targetResource != null)
        {
            distFromRes = Vector3.Distance(targetResource.position, playerPos);
        }
    }
    private void Update()
    {
        StartCoroutine(GetInvUpdate(charId));
        
        if (agent.velocity.sqrMagnitude > 0)
        {
            actionState = ActionState.Walking;
            anim.SetBool("Walking", true);
        }
        else
        {
            actionState = ActionState.NotMoving;
            anim.SetBool("Walking", false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            agent.speed = speed;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                

                if (hit.collider.CompareTag("ResourceNode"))
                {
                    ResourceManager resource = hit.collider.GetComponent<ResourceManager>();
                    targetResource = hit.transform;
                    isMovingToResource = true;

                    //updates distance between player and resource
                    InvokeRepeating("DistanceFromResource", 0f, .3f);
                    
                    
                    if (targetResource != null)
                    {
                        MoveToResource(targetResource);
                        Debug.Log("Moving to resource");
                        resMan.isBeingGathered = false;
                        if (isMovingToResource && distFromRes < 1 && inResArea)
                        {

                            if (playerInventory >= carryAmount)
                            {
                                Debug.Log("Inventory Full");
                                isGathering = false;
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

        //Build UI Popup
        if (Input.GetKeyDown(KeyCode.B) && inBuildArea)
        {
            if (buildUI.popUp == false)
            {
                buildUI.BuildPopup();
            }
            else
            {
                buildUI.ExitBuildPopup();
            }
        }

        //Inventory UI Popup
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (invUI.popUp == false)
            {
                invUI.InvPopUp();
            }
            else
            {
                invUI.ExitInvPopUp();
            }
        }
    }


    //Gathering Trigger


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ResourceNode"))
        {
            
            Debug.Log("In Resource Area");
            inResArea = true;
            if (isMovingToResource && distFromRes < 5)
                {
                    if (playerInventory >= carryAmount)
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

        else if (other.CompareTag("BuildArea"))
        {
            Debug.Log("Entering Build Area");
            inBuildArea = true;
            pressB.gameObject.SetActive(true);
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

        if (other.CompareTag("BuildArea"))
        {
            Debug.Log("Exiting Build Area");
            inBuildArea = false;
            if (buildUI.popUp)
            {
                buildUI.ExitBuildPopup();
            }
            pressB.gameObject.SetActive(false);
        }


    }
    



    
    private void MoveToResource(Transform resource)
    {
        isGathering = false;
        agent.SetDestination(resource.position);
    }
    void MovePlayer(Vector3 destination)
    {
        agent.SetDestination(destination);
        isMoving = true;
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
                invDh = www.downloadHandler.text;
                Inventory inv = new Inventory();
                inv = JsonUtility.FromJson<Inventory>(invDh);
                if (inv.data.Length == 0)
                {
                    AddNewInvItem(itemId);
                    Debug.Log("Adding New Item");
                }
                else
                {
                    AddMoreItem(itemId);
                    Debug.Log("Adding Existing Item");
                }
            }
        }
    }

        IEnumerator AddNewItem(int itemID)
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
            }
        }
    }

    IEnumerator AddMoreItemToInv(int itemId)
    {
        Inventory inv = new Inventory();
        inv = JsonUtility.FromJson<Inventory>(invDh);
        inv.data[0].item_amount = inv.data[0].item_amount + 1;
        string jsonUse = JsonUtility.ToJson(inv.data[0], true);
        Debug.Log(jsonUse);
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
                invDh = www.downloadHandler.text;
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
                    ItemManagement itMan = gameObject.GetComponent<ItemManagement>();
                    int itemId = inv.data[i].item_id;
                    Debug.Log($"Depositing itemId: {itemId}");
                    itMan.CallItem(itemId);
                }
            }
        }
    }

    private void ActionStateSetup()
    {
        switch (actionState)
        {
            case ActionState.Walking:
            break;

            case ActionState.NotMoving:
            break;
        }
    }


}