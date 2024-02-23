using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Resource
{
    public ResData[] data;
}

[Serializable]
public class ResData
{
    public int resource_id;
    public string resource_name;
    public int light_cost;
    public string resource_type;
}