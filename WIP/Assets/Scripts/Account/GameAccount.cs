using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [Serializable]
    public class MyAccount
    {

        public Data[] data;
    }
    [Serializable]
    public class Data
    {
        public string username;
        public string password;
        public int user_id;
    }