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
        panelSet.GetComponent<Image>().enabled = false;
    }

    public void BuildPopup()
    {
        panelSet.GetComponent<Image>().enabled = true;
        popUp = true;
    }

    public void ExitBuildPopup()
    {
        panelSet.GetComponent<Image>().enabled = false;
        popUp = false;
    }
}
