using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserId : MonoBehaviour
{
    public int user_id;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
