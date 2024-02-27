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
}