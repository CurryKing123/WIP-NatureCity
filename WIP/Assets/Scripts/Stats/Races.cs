using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Races
{
    public RaceData[] data;
}

[Serializable]
public struct RaceData
{
    public int race_id;
    public string race_name;
    public float move_speed;
    public string resist;
    public int carry_amount;
}