using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class BuildingUI : MonoBehaviour
{
    public GameObject panelSet;
    public bool popUp = false;

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
