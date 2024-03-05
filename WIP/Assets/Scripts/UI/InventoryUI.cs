using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public bool popUp = false;
    [SerializeField] private GameObject Inventory;

    public void InvPopUp()
    {
        Inventory.SetActive(true);
        popUp = true;
    }

    public void ExitInvPopUp()
    {
        Inventory.SetActive(false);
        popUp = false;
    }
}
