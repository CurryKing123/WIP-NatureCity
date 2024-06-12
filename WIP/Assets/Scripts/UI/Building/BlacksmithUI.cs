using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BlacksmithUI : MonoBehaviour
{
    [SerializeField] private Sprite [] sprites;
    [SerializeField] private GameObject blacksmithUI;
    [SerializeField] private GameObject blacksmithGrid;
    [SerializeField] private Button iconPrefab;

    [SerializeField] private Button [] blacksmithButtons;
    [SerializeField] private GameObject [] craftedItems;

    private string itemName;
    private float localTime = 0f;
    private float waitTime = 0f;
    private bool isCrafting = false;

    private Items items;
    private GlobalInventory globalInv;


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
    public void CheckGlobalInventory(int woodCost, int stoneCost, string itemName)
    {
        StartCoroutine(GetGlobalInv(woodCost, stoneCost, itemName));
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
            int woodCost = items.data[0].wood_cost;
            int stoneCost = items.data[0].stone_cost;
            CheckGlobalInventory(woodCost, stoneCost, itemName);


        }
    }

    private IEnumerator GetGlobalInv(int woodCost, int stoneCost, string itemName)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/global_inventory/get-global_inv"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            string dH = www.downloadHandler.text;

            globalInv = JsonUtility.FromJson<GlobalInventory>(dH);
            int globalWood = globalInv.data[0].res_amount;
            int globalStone = globalInv.data[1].res_amount;

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
                    StartCrafting(itemName);
                }
            }
        }
    }

    private void StartCrafting(string itemName)
    {
        isCrafting = true;
    }
}
