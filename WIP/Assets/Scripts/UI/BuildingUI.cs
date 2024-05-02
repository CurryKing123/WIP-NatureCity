using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingUI : MonoBehaviour
{
    [SerializeField] private GameObject panelSet;
    public bool popUp = false;
    

    private void Start()
    {
        panelSet = GameObject.Find("UI Popup");
        panelSet.SetActive(false);
    }

    public void BuildPopup()
    {
        panelSet.SetActive(true);
        popUp = true;
    }

    public void ExitBuildPopup()
    {
        panelSet.SetActive(false);
        popUp = false;
    }
}
