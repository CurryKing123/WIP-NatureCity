using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.AI;
using System.Threading;
using System.Linq;
using TMPro;
using Mirror;

public class PlayerController : MonoBehaviour
{

    public bool isGathering = false;


    public NavMeshAgent agent;
    private float distFromRes;
    private Transform targetResource;
    private ResourceManager resMan;
    private BuildingUI buildUI;
    private InventoryUI invUI;
    private GameObject createIGN;
    public Vector3 playerPos;
    public TextMeshProUGUI playerName;
    private ItemManagement itMan;
    private GetPlayerData getPlayerData;
    private Animator anim;
    public Camera cam;
    private PlayerId playerId;
    private UserId userIdJson;
    private MyNetworkPlayer myNetworkPlayer;
    private Ray ray;
    private RaycastHit hit;



    public enum ActionState {Walking, NotMoving, MovingToResource}
    public ActionState actionState;
    public enum AreaState {BuildArea, ResourceArea, Blacksmith, None}
    public AreaState areaState;


    private int userId;
    public float speed;
    public int carryAmount;
    public int playerInventory;
    public int charId;
    public string charRace;
    public string userName;
    public string dH;
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
    public void InvUpdate()
    {
        StartCoroutine(GetInvUpdate());
    }


    private void Start()
    {
        //playerId = GameObject.Find("PlayerId").GetComponent<PlayerId>();
        
        ActionStateSetup();
        AreaStateSetup();

        //Getting UserId From Written Json File
        string dH = File.ReadAllText(Application.persistentDataPath + "UserId.json");
        userIdJson = new UserId();
        userIdJson = JsonUtility.FromJson<UserId>(dH);
        userId = userIdJson.userId;

        anim = GetComponent<Animator>();

        actionState = ActionState.NotMoving;
        areaState = AreaState.None;

        
        cam = FindAnyObjectByType<Camera>();
        itMan = gameObject.GetComponent<ItemManagement>();
        buildUI = GetComponent<BuildingUI>();
        invUI = GetComponent<InventoryUI>();
        agent = GetComponent<NavMeshAgent>();
        getPlayerData = GetComponent<GetPlayerData>();
        myNetworkPlayer = gameObject.transform.parent.GetComponent<MyNetworkPlayer>();

        

        getPlayerData.CallChar(userId);



        createIGN = GameObject.Find("Player UI");
        
        
    }


    public void CreateIGN()
    {
        createIGN.GetComponent<CreateIGN>().FindPlayer();
    }



    IEnumerator wait()
    {
        yield return new WaitForSeconds(3f);
    }


    private void FixedUpdate()
    {
        //player position
        playerPos = transform.position;


    }
    private void Update()
    {
        
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
            ray = cam.ScreenPointToRay(Input.mousePosition);
            agent.speed = speed;

            if (Physics.Raycast(ray, out hit))
            {
                

                if (hit.collider.CompareTag("ResourceNode"))
                {
                    ResourceManager resource = hit.collider.GetComponent<ResourceManager>();
                    targetResource = hit.transform;
                    actionState = ActionState.MovingToResource;
                    
                    
                    if (targetResource != null)
                    {
                        distFromRes = Vector3.Distance(targetResource.position, playerPos);
                        MoveToResource(targetResource);
                        Debug.Log("Moving to resource");
                        resource.isBeingGathered = false;
                        if (actionState == ActionState.MovingToResource && distFromRes < 1 && areaState == AreaState.ResourceArea)
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
                    actionState = ActionState.Walking;
                    if (isGathering)
                    {
                        //Figure Out How To Stop Gathering Clicking Away From Resource
                        isGathering = false;
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Blacksmith") && areaState == AreaState.Blacksmith)
                {
                    
                }
            }

        }

        //Build UI Popup
        if (Input.GetKeyDown(KeyCode.B) && areaState == AreaState.BuildArea)
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
            areaState = AreaState.ResourceArea;
            if (actionState == ActionState.MovingToResource)
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
                    Debug.Log("Should be gathering");
                
                }
        }

        else if (other.CompareTag("HomeTree"))
        {
            DepositResource(charId);
        }

        else if (other.CompareTag("BuildArea"))
        {
            Debug.Log("Entering Build Area");
            areaState = AreaState.BuildArea;
        }

        else if (other.CompareTag("Blacksmith"))
        {
            Debug.Log("Entering Blacksmith Area");
            areaState = AreaState.Blacksmith;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        areaState = AreaState.None;
        if (other.CompareTag("Resource"))
        {
            ResourceManager resMan = other.GetComponent<ResourceManager>();
            resMan.StopGathering();
            Debug.Log("Out of Resource Area");
            areaState = AreaState.None;
            isGathering = false;
        }

        if (other.CompareTag("BuildArea"))
        {
            Debug.Log("Exiting Build Area");
            if (buildUI.popUp)
            {
                buildUI.ExitBuildPopup();
            }
            buildUI.ExitPressPopup();
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
        actionState = ActionState.Walking;
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
                InvUpdate();
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
                InvUpdate();
            }
        }
    }

    //Get Updated Inventory
    IEnumerator GetInvUpdate()
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
                invUI.InvGridUpdate();
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

            case ActionState.MovingToResource:
            break;
        }
    }

    private void AreaStateSetup()
    {
        switch (areaState)
        {
            case AreaState.BuildArea:
            break;

            case AreaState.ResourceArea:
            break;

            case AreaState.Blacksmith:
            break;

            case AreaState.None:
            break;
        }
    }


}