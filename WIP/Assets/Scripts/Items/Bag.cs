using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Bag
{
    public BagData[] data;
}

[Serializable]
public class BagData
{
    public int bag_id;
    public string bag_name;
    public int item_bag_capacity;
}