using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    public PlayerInv[] data;
}

[Serializable]
public class PlayerInv
{
    public int char_id;
    public int item_id;
    public int item_amount;
}