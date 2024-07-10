using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BlacksmithInventory
{
    public BlacksmithInv[] data;
}

[Serializable]
public class BlacksmithInv
{
    public int item_id;
    public int item_name;
    public int item_amount;
}