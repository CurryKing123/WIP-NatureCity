using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI woodInv;
    [SerializeField] private TextMeshProUGUI stoneInv;
    [SerializeField] private TextMeshProUGUI carryCap;
    [SerializeField] private GameObject player;

    void Update()
    {
        PlayerController playCont = player.GetComponent<PlayerController>(); 
        carryCap.text = $"Capacity: {playCont.carryAmount}";
    }
}
