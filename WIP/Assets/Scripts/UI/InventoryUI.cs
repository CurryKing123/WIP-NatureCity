using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private PlayerController playCont;
    private Inventory inv;
    public bool popUp = false;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject invGrid;
    [SerializeField] private Button iconPrefab;

    private Sprite sticks;
    private Sprite stone;

    void Start()
    {
        playCont = GetComponent<PlayerController>();
        inv = new Inventory();
        SpriteLoad();
    }

    void SpriteLoad()
    {
        sticks = Resources.Load<Sprite>("sticks_placeholder");
        stone = Resources.Load<Sprite>("stone_placeholder");
    }

    void Update()
    {
        inv = JsonUtility.FromJson<Inventory>(playCont.invDh);
        if (invGrid.transform.childCount < inv.data.Length)
        {
            for(int i = 0; i < inv.data.Length; i++)
            {
                Instantiate(iconPrefab, invGrid.transform);
                Transform getIcon = invGrid.transform.GetChild(i);
                int itemId = inv.data[0].item_id;
                int count = inv.data[0].item_amount;
                AddItemIcon(getIcon, count, itemId);
            }
        }

    }

    private void AddItemIcon(Transform getIcon, int count, int itemId)
    {
        Image image = getIcon.GetComponent<Image>();

        image.sprite = sticks;
    }

    public void InvPopUp()
    {
        inventoryUI.SetActive(true);
        popUp = true;

    }

    public void ExitInvPopUp()
    {
        inventoryUI.SetActive(false);
        popUp = false;
    }
}
