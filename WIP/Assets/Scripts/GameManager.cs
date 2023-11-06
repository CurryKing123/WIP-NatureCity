using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int totalResources = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddResources(int amount)
    {
        totalResources += amount;

        // Update your UI or resource display here
    }
}
