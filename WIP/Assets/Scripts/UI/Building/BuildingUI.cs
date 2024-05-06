using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingUI : MonoBehaviour
{
    [SerializeField] private GameObject buildUI;
    [SerializeField] private PlayerController player;
    public bool popUp = false;
    

    private void Start()
    {
        buildUI = GameObject.Find("Building");

        player = GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (player.areaState == PlayerController.AreaState.BuildArea)
        {
            PressPopup();
        }
        else
        {
            ExitPressPopup();
        }
    }

    public void PressPopup()
    {
        buildUI.GetComponent<BuildingUIGroup>().pressB.SetActive(true);
    }

    public void ExitPressPopup()
    {
        buildUI.GetComponent<BuildingUIGroup>().pressB.SetActive(false);
    }

    public void BuildPopup()
    {
        buildUI.GetComponent<BuildingUIGroup>().uiPopup.SetActive(true);
        popUp = true;
    }

    public void ExitBuildPopup()
    {
        buildUI.GetComponent<BuildingUIGroup>().uiPopup.SetActive(false);
        popUp = false;
    }
}
