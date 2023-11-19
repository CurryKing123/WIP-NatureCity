using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int woodAmount;
    public static int stoneAmount;

    public static void AddWoodAmount(int amount)
    {
        woodAmount += amount;
    }

    public static int GetWoodAmount()
    {
        return woodAmount;
    }
}
