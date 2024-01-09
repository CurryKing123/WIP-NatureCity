using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataJSON : MonoBehaviour
{
    private PlayerData playerData;

    void Start()
    {
        
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(playerData);
    }

    public void LoadData()
    {

    }
}
