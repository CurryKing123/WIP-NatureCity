using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [Serializable]
    public class CharArray
    {
        public CharData[] data;
    }
    [Serializable]
    public class CharData
    {
        public string username;
        public string password;
        public int user_id;
    }
