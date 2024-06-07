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

    [SerializeField] private GameObject pressB;

    public bool isGathering = false;
    private bool blacksmithUIPopup = false;


    public NavMeshAgent agent;
    private float distFromRes;
    private float staticDistFromRes;
    private Vector3 targetResource;
    private ResourceManager resMan;
    private BuildingUI buildUI;
    private InventoryUI invUI;
    private BlacksmithUI blacksmithUI;
    [SerializeField] private BlacksmithUIGroup blacksmithUIGroup;
    private GameObject createIGN;
    public Vector3 playerPos;
    public TextMeshProUGUI playerName;
    private ItemManagement itMan;
    private GetPlayerData getPlayerData;
    private Animator anim;
    public Camera cam;
    private PlayerId playerId;
    private UserId userIdJson;
    private bool localPlayer;
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
    public void CheckHomeInventory(int charId)
    {
        StartCoroutine(CheckHomeInv(charId));
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

        targetResource = new Vector3();

        
        cam = FindAnyObjectByType<Camera>();
        itMan = gameObject.GetComponent<ItemManagement>();
        buildUI = GetComponent<BuildingUI>();
        invUI = GetComponent<InventoryUI>();
        agent = GetComponent<NavMeshAgent>();
        getPlayerData = GetComponent<GetPlayerData>();
        blacksmithUI = GetComponent<BlacksmithUI>();
        localPlayer = gameObject.transform.parent.GetComponent<MyNetworkPlayer>().isLocalPlayer;

        pressB = GameObject.Find("Building").GetComponent<BuildingUIGroup>().pressB;
        blacksmithUIGroup = GameObject.Find("Blacksmith").GetComponent<BlacksmithUIGroup>();


        

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
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }

        if(targetResource != null)
        {
            distFromRes = Vector3.Distance(targetResource, playerPos);
            //Debug.Log($"Distance From Resource: " + distFromRes);
        }

        if (Input.GetMouseButtonDown(1))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            agent.speed = speed;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("ResourceNode"))
                {
                    actionState = ActionState.MovingToResource;
                    targetResource = hit.transform.position;
                    staticDistFromRes = Vector3.Distance(targetResource, playerPos);
                    Debug.Log($"Target Resource: " + targetResource);
                    Debug.Log($"Distance From Resource: " + distFromRes);
                    ResourceManager resource = hit.collider.GetComponent<ResourceManager>();
                    MoveToResource(targetResource, resource);
                    
                    
                    if (targetResource != null)
                    {
                        
                        Debug.Log("Moving to resource");
                        if (actionState == ActionState.MovingToResource && staticDistFromRes < 1 && areaState == AreaState.ResourceArea)
                        {

                            if (playerInventory >= carryAmount)
                            {
                                Debug.Log("Inventory Full");
                                isGathering = false;
                                resource.isBeingGathered = false;
                            }
                            else if (isGathering)
                            {
                                isGathering = false;
                            }
                            else
                            {
                                isGathering = true;
                                resource.StartGathering(transform);
                            }

                        }
                        else
                        {
                            isGathering = false;
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
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                //Blacksmith UI Interaction
                if (hit.collider.CompareTag("Blacksmith") && areaState == AreaState.Blacksmith)
                
                {
                    blacksmithUIGroup.blacksmithUI.SetActive(true);
                    blacksmithUIPopup = true;
                }
                
            }

        }

        if (areaState == AreaState.BuildArea)
        {
            pressB.SetActive(true);
        }

        else
        {
            pressB.SetActive(false);
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
        if (localPlayer)
        {
            if (other.CompareTag("ResourceNode"))
            {

                Debug.Log("In Resource Area");
                areaState = AreaState.ResourceArea;
                if (actionState == ActionState.MovingToResource && distFromRes < 5)
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
                CheckHomeInventory(charId);
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
        else
        {
            Debug.Log("Not local player");
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

        if (other.CompareTag("Blacksmith"))
        {
            if (blacksmithUIPopup)
            {
                blacksmithUIGroup.blacksmithUI.SetActive(false);
                blacksmithUIPopup = false;
            }
        }


    }
    



    
    private void MoveToResource(Vector3 resNode, ResourceManager resource)
    {

        agent.SetDestination(resNode);
        actionState = ActionState.MovingToResource;
    }
    void MovePlayer(Vector3 destination)
    {
        agent.SetDestination(destination);
        actionState = ActionState.Walking;
        isGathering = false;
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

    IEnumerator CheckHomeInv(int charId)
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
                    itMan.CallItem(itemId, charId);
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