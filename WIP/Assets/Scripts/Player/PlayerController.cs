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
    public float velocity;
    private float distFromRes;
    private float staticDistFromRes;
    private Vector3 targetResource;


    private ResourceManager resMan;
    private BuildingUI buildUI;
    private InventoryUI invUI;
    private BlacksmithUI blacksmithUI;
    private InventoryUpdater invUp;


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
        invUp = GetComponent<InventoryUpdater>();

        getPlayerData = GetComponent<GetPlayerData>();
        localPlayer = gameObject.transform.parent.GetComponent<MyNetworkPlayer>().isLocalPlayer;

        pressB = GameObject.Find("Building").GetComponent<BuildingUIGroup>().pressB;
        blacksmithUI = GameObject.Find("Blacksmith").GetComponent<BlacksmithUI>();

        getPlayerData.CallChar(userId);

        createIGN = GameObject.Find("Player UI");
        
    }


    public void CreateIGN()
    {
        createIGN.GetComponent<CreateIGN>().FindPlayer();
        blacksmithUI.FindPlayer();
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

        if (localPlayer)
        {
            velocity = agent.velocity.magnitude;

        
            if (velocity > .5f)
            {
                anim.SetBool("Idle", false);
                anim.SetBool("Walking", true);
            }
            else
            {
                anim.SetBool("Walking", false);
                anim.SetBool("Idle", true);
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
                    //Moving To Resource
                    if (hit.collider.CompareTag("ResourceNode"))
                    {
                        targetResource = hit.transform.position;
                        staticDistFromRes = Vector3.Distance(targetResource, playerPos);
                        //Debug.Log($"Target Resource: " + targetResource);
                        //Debug.Log($"Distance From Resource: " + distFromRes);
                        ResourceManager resource = hit.collider.GetComponent<ResourceManager>();
                        MoveToResource(targetResource, resource);

                        if (targetResource != null)
                        {
                            Debug.Log("Moving to resource");
                            if (actionState == ActionState.MovingToResource && staticDistFromRes < 1 && areaState == AreaState.ResourceArea)
                            {
                                ResNodeCheck(resource);
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


                        

                        //Stop Gathering When Moving
                        if (isGathering)
                        {
                            //Figure Out How To Stop Gathering Clicking Away From Resource
                            isGathering = false;
                        }


                        //Stop Crafting When Moving
                        if (blacksmithUI.isCrafting == true)
                        {
                            blacksmithUI.isCrafting = false;
                        }
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
                    blacksmithUI.blacksmithUI.SetActive(true);
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
                        ResourceManager resMan = other.GetComponent<ResourceManager>();

                        ResNodeCheck(resMan);

                        Debug.Log("Should be gathering");

                    }
            }

            else if (other.CompareTag("HomeTree"))
            {
                invUp.CheckHomeInventory(charId);
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
                blacksmithUI.blacksmithUI.SetActive(false);
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

    private void GatherCheck(ResourceManager resource)
    {
        if (targetResource != null)
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
    }

    private void ResNodeCheck(ResourceManager resource)
    {
        if (hit.collider.name == "Stone Spawner" || hit.collider.name == "Stick Spawner")
        {
            GatherCheck(resource);
        }

        if (hit.collider.name == "Tree Spawner")
        {
            if (invDh.Contains("Axe"))
            {
                GatherCheck(resource);
            }
            else
            {
                Debug.Log("Don't have correct tool");
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