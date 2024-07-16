using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class BlacksmithUI : MonoBehaviour
{
    [SerializeField] private Sprite [] sprites;
    public GameObject blacksmithUI;
    [SerializeField] private GameObject blacksmithCraftablesGrid;
    [SerializeField] private GameObject blacksmithCraftedGrid;
    [SerializeField] private Button iconPrefab;

    [SerializeField] private Button [] blacksmithButtons;
    [SerializeField] private GameObject [] craftedItems;
    [SerializeField] private GameObject blacksmithMesh;

    private string itemName;
    private float localTime = 0f;
    private float waitTime = 0f;
    public bool isCrafting = false;
    private int woodCost;
    private int stoneCost;
    private int globalWood;
    private int globalStone;
    private float craftTime;
    private int maxItems = 5;

    private Items items;
    private GlobalInventory globalInv;
    private BlacksmithInventory blacksmithInv;
    private InventoryUI invUI;
    private Transform player;
    private Transform getIcon;
    private PlayerController playCont;


    private void Start()
    {
        items = new Items();
        globalInv = new GlobalInventory();
        blacksmithInv = new BlacksmithInventory();
        invUI = new InventoryUI();

        StartBlacksmithInventory();
    }

    private void Update()
    {
        if (isCrafting)
        {
            localTime += Time.deltaTime;
            CraftItem();
        }
        else
        {
            StopCrafting();
        }
    }

    public void FindPlayer()
    {
        playCont = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public void StartCrafting()
    {
        if (!isCrafting)
        {
            Debug.Log("Start Crafting...");
            waitTime = craftTime;
            isCrafting = true;
        }
    }
    private void StopCrafting()
    {
        if (isCrafting)
        {
            Debug.Log("Stop Crafting...");
            localTime = 0f;
            isCrafting = false;
        }
        else
        {
            localTime = 0f;
        }
    }

    public void OnClicked()
    {
        print(EventSystem.current.currentSelectedGameObject.name);
    }

    //Coroutines

    //public void GetCraftedItem()
    //{
    //    StartCoroutine(GetCraftedItem());
    //}

    public void StartBlacksmithInventory()
    {
        StartCoroutine(StartBlacksmithInv());
    }

    public void BlacksmithGridUpdate(string wtd)
    {
        StartCoroutine(BlacksmithInvUpdate(wtd));
    }

    public void AddToBlacksmithInventory(int itemId, int itemAmount, string dH)
    {
        StartCoroutine(AddToBlacksmithInv(itemId, itemAmount, dH));
    }

    public void GetItemData()
    {
        StartCoroutine(GetItem());
    }
    public void CheckGlobalInventory(int woodCost, int stoneCost)
    {
        StartCoroutine(GetGlobalInv(woodCost, stoneCost));
    }
    
    public void TransferItemData()
    {
        StartCoroutine(TransferItem());
    }

    private IEnumerator GetItem()
    {
        itemName = EventSystem.current.currentSelectedGameObject.name;
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/item/get-item-by-name?item_name={itemName}"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            string dH = www.downloadHandler.text;
            items = JsonUtility.FromJson<Items>(dH);
            woodCost = items.data[0].wood_cost;
            stoneCost = items.data[0].stone_cost;
            craftTime = items.data[0].craft_time;
            CheckGlobalInventory(woodCost, stoneCost);
        }
    }

    private IEnumerator GetGlobalInv(int woodCost, int stoneCost)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/global_inventory/get-global_inv"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            string dH = www.downloadHandler.text;

            globalInv = JsonUtility.FromJson<GlobalInventory>(dH);
            globalWood = globalInv.data[0].res_amount;
            globalStone = globalInv.data[1].res_amount;

            if (globalWood < woodCost)
            {
                Debug.Log("Not enough wood");
            }
            else
            {
                if (globalStone < stoneCost)
                {
                    Debug.Log("Not enough stone");
                }
                else
                {
                    StartCrafting();
                }
            }
        }
    }

    private void CraftItem()
    {
        if (localTime < waitTime)
        {
            if (!isCrafting)
            {
                StopCrafting();
            }
            return;
        }
        
        waitTime = localTime + craftTime;
        Debug.Log("Crafted 1...");
        string wtd = "add";
        BlacksmithGridUpdate(wtd);
        isCrafting = false;
    }

    private IEnumerator BlacksmithInvUpdate(string wtd)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/blacksmith_inventory/get-blacksmith_inv-by-name?item_name={itemName}"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            string dH = www.downloadHandler.text;
            blacksmithInv = JsonUtility.FromJson<BlacksmithInventory>(dH);

            int itemId = blacksmithInv.data[0].item_id;

            int itemAmount = blacksmithInv.data[0].item_amount;

            if (wtd == "add")
            {
                //Adding 1 to crafted item amount
                itemAmount = itemAmount + 1;
                AddToBlacksmithInventory(itemId, itemAmount, dH);
            }
            else if (wtd == "sub")
            {
                //Subtracting 1 to crafted item amount
                itemAmount = itemAmount - 1;
                AddToBlacksmithInventory(itemId, itemAmount, dH);
            }

            for (int i = 0; i < 1; i++)
            {
                if (blacksmithButtons[i].name == "axe")
                {
                    getIcon = blacksmithCraftedGrid.transform.GetChild(i);
                    getIcon.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Axes Available: {itemAmount}";
                }
                else
                {
                    Debug.Log("Theres a problem here");
                }
            }
        }
    }

    private IEnumerator AddToBlacksmithInv(int itemId, int itemAmount, string dH)
    {
        blacksmithInv = JsonUtility.FromJson<BlacksmithInventory>(dH);
        blacksmithInv.data[0].item_amount = itemAmount;
        string jsonUse = JsonUtility.ToJson(blacksmithInv.data[0], true);
        using (UnityWebRequest www = UnityWebRequest.Put($"http://localhost:8002/blacksmith_inventory/put-blacksmith_inv?item_id={itemId}", jsonUse))
        {
            www.SetRequestHeader("key", "1");
            www.SetRequestHeader("content-type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }

        }
    }



    private IEnumerator StartBlacksmithInv()
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/blacksmith_inventory/get-blacksmith_inv"))
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
                blacksmithInv = JsonUtility.FromJson<BlacksmithInventory>(dH);

                for (int i = 0; i < 2; i++)
                {
                    if (blacksmithButtons[i].name == "axe")
                    {
                        int itemAmount = blacksmithInv.data[0].item_amount;
                        getIcon = blacksmithCraftedGrid.transform.GetChild(i);
                        getIcon.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Axes Available: {itemAmount}";
                    }
                    else
                    {
                        Debug.Log("Theres a problem here");
                    }
                }
            }
        }
    }

    private IEnumerator TransferItem()
    {
        itemName = EventSystem.current.currentSelectedGameObject.name;
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/item/get-item-by-name?item_name={itemName}"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            string dH = www.downloadHandler.text;
            blacksmithInv = JsonUtility.FromJson<BlacksmithInventory>(dH);

            int itemId = blacksmithInv.data[0].item_id;

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }

            else
            {
                playCont.playerInventory++;
                playCont.CheckInv(playCont.charId, itemId);
                playCont.InvUpdate();

                string wtd = "sub";
                BlacksmithGridUpdate(wtd);
            }
        }
    }

}
