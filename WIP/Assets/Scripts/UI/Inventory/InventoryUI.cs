using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class InventoryUI : MonoBehaviour
{
    private PlayerController playCont;
    private Inventory inv;
    private GetPlayerData getPlayerData;
    private Transform getIcon;
    public bool popUp = false;

    [SerializeField] private Sprite[] sprites;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject invUI;
    [SerializeField] private GameObject invGrid;
    [SerializeField] private Button iconPrefab;

    private Sprite sticks, stone;

    void Start()
    {
        playCont = GetComponent<PlayerController>();
        getPlayerData = GetComponent<GetPlayerData>();
        inv = new Inventory();

        inventoryUI = GameObject.Find("Inventory");
        invGrid = inventoryUI.GetComponent<InventoryUIGroup>().invGrid;
        invUI = inventoryUI.GetComponent<InventoryUIGroup>().invUI;
    }

    public void InvGridUpdate()
    {
        inv = JsonUtility.FromJson<Inventory>(playCont.invDh);
        if (invGrid.transform.childCount < inv.data.Length)
        {
            for(int i = 0; i < inv.data.Length; i++)
            {
                Instantiate(iconPrefab, invGrid.transform);
                getIcon = invGrid.transform.GetChild(i);
                int itemId = inv.data[i].item_id;
                int count = inv.data[i].item_amount;
                AddItemIcon(getIcon, count, itemId);
            }
        }
        else if (invGrid.transform.childCount > inv.data.Length)
        {
            for(int i = 0; i < invGrid.transform.childCount; i++)
            {
                getIcon = invGrid.transform.GetChild(i);
                Destroy(getIcon.gameObject);
            }
        }
        else
        {
            for(int i = 0; i < inv.data.Length; i++)
            {
                getIcon = invGrid.transform.GetChild(i);
                int itemId = inv.data[i].item_id;
                int count = inv.data[i].item_amount;
                AddItemIcon(getIcon, count, itemId);
            }
        }

    }


    private void AddItemIcon(Transform getIcon, int count, int itemId)
    {
        Image image = getIcon.GetComponent<Image>();
        TMP_Text text = getIcon.GetComponentInChildren<TMP_Text>();
        text.text = $"{count}";

        image.sprite = sprites[itemId - 1];
    }

    public void InvPopUp()
    {
        invUI.SetActive(true);
        popUp = true;

    }

    public void ExitInvPopUp()
    {
        invUI.SetActive(false);
        popUp = false;
    }
}
