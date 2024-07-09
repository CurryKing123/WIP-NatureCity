using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BlacksmithUI : MonoBehaviour
{
    [SerializeField] private Sprite [] sprites;
    public GameObject blacksmithUI;
    [SerializeField] private GameObject blacksmithGrid;
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
    private Transform player;


    private void Start()
    {
        items = new Items();
        globalInv = new GlobalInventory();
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

    public void StartCrafting()
    {
        if (!isCrafting)
        {
            Debug.Log("Start Crafting...");
            waitTime = craftTime;
            isCrafting = true;
            CraftItem();
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

    public void GetItemData()
    {
        StartCoroutine(GetItem());
    }
    public void CheckGlobalInventory(int woodCost, int stoneCost)
    {
        StartCoroutine(GetGlobalInv(woodCost, stoneCost));
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
        Instantiate(craftedItems[0], new Vector3(0, 0, 0), Quaternion.identity);
    }
}
