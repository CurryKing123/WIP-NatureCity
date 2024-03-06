using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResourceNodes
{
    public ResNodeData[] data;
}

[Serializable]
public class ResNodeData
{
    public int resource_node_id;
    public string resource_node_name;
    public int resource_amount;
    public float gathering_time;
    public float respawn_time;
    public int resource_id;
}