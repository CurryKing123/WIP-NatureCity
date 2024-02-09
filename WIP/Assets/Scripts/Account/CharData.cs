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
        public int char_id;
        public int user_id;
        public string user_name;
        public string character_race;
        public string equip_item_1;
        public string equip_item_2;
        public string equip_item_3;
        public string equip_item_4;
        public string equip_item_5;
        public int player_hp;
        public int player_stamina;
        public string player_status;
    }
