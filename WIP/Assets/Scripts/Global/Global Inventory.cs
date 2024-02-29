using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalInventory
{
    public GlobalInventoryData[] data;
}

[Serializable]
public class GlobalInventoryData
{
    public int res_id;
    public int res_amount;
    public string res_name;
}