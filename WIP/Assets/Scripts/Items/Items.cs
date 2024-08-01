using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Items
{
    public ItemData[] data;
}

[Serializable]
public class ItemData
{
    public int item_id;
    public string item_type;
    public string item_description;
    public string item_name;
    public int res_id;
    public int res_cost;
    public int wood_cost;
    public int stone_cost;
    public int craft_time;
    public int durability;
}