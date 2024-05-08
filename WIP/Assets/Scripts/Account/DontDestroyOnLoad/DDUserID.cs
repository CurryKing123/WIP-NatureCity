using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDUserID : MonoBehaviour
{
    public int userId;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
