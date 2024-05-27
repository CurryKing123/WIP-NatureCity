using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.TextCore.Text;

public class GetPlayerData : MonoBehaviour
{

    private int userId;
    public int charId;
    public float speed;
    public int carryAmount;
    public int playerInventory;
    public string charRace;
    public string userName;
    public string dH;
    public string invDh;
    public string[] equip;

    private CharArray myChar;
    private ItemManagement itMan;
    private CharacterInfo charInfo;
    private PlayerController player;
    private MyNetworkPlayer myNetworkPlayer;
    private PlayerId playerId;
    private CreateIGN createIgn;


    void Start()
    {
        myChar = new CharArray();
        itMan = gameObject.GetComponent<ItemManagement>();
        player = gameObject.GetComponent<PlayerController>();
        createIgn = GameObject.Find("Player UI").gameObject.GetComponent<CreateIGN>();
        //playerId = GameObject.Find("PlayerId").GetComponent<PlayerId>();
        myNetworkPlayer = this.transform.parent.gameObject.GetComponent<MyNetworkPlayer>();
    }

    public void CallChar(int userId)
    {
        Debug.Log($"UserId: {userId}");
        StartCoroutine(GetChar(userId));
    }
    private void PostChar(int userId)
    {
        StartCoroutine(MakeChar(userId));
    }

    public void CallRace(string race)
    {
        StartCoroutine(GetRace(race));
    }
    public void CallInv(int charId)
    {
        StartCoroutine(GetInv(charId));
    }

    //Get Character Data From UserId
    IEnumerator GetChar(int userId)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/char/get-char-by-id?user_id={userId}"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            dH = www.downloadHandler.text;
            player.dH = dH;
            createIgn.dH = dH;

            myChar = JsonUtility.FromJson<CharArray>(dH);

            if (www.result != UnityWebRequest.Result.Success) 
            {
                Debug.Log(www.error);
            }
            
            else
            {
                if(myChar.data.Length == 0)
                {
                    Debug.Log("New Character");
                    PostChar(userId);
                }
                else
                {
                    Debug.Log("Found Character");

                    //Player Character ID
                    charId = myChar.data[0].char_id;
                    player.charId = charId;

                    //Player Character Race
                    charRace = myChar.data[0].character_race;
                    player.charRace = charRace;

                    CallRace(charRace);

                    //Player In Game Name
                    player.userName = myChar.data[0].user_name;
                    myNetworkPlayer.displayName = myChar.data[0].user_name;

                    //Player Equipment
                    equip = new string[]{myChar.data[0].equip_item_1,
                    myChar.data[0].equip_item_2,
                    myChar.data[0].equip_item_3,
                    myChar.data[0].equip_item_4,
                    myChar.data[0].equip_item_5};
                    
                    player.equip = equip;


                    for(int i=0; i<5; i++)
                    {
                        itMan.CallEquip(equip[i]);
                    }

                    CallInv(charId);
                }
            }
        }
        player.CreateIGN();
    }
    IEnumerator MakeChar(int userId)
    {
        using (UnityWebRequest www = UnityWebRequest.Post($"http://localhost:8002/char/post-char", "{ \"user_id\": \"" + userId + "\"}", "application/json"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) 
            {
                Debug.Log(www.error);
            }
            else
            {
                CallChar(userId);
            }
        }
    }

    //Get Race Info
    IEnumerator GetRace(string race)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/race/get-race-by-name?race_name={race}"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) 
            {
                Debug.Log(www.error);
            }
            else
            {
                Races charRace = new Races();
                string dH = www.downloadHandler.text;
                Debug.Log(dH);
                charRace = JsonUtility.FromJson<Races>(dH);

                //Player Character Race Stats
                player.speed = charRace.data[0].move_speed;
                player.carryAmount = charRace.data[0].carry_amount;
            }
        }
    }

    //Get Inventory At Start
    IEnumerator GetInv(int charId)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/inventory/get-inv-by-id?char_id={charId}"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) 
            {
                Debug.Log(www.error);
            }
            else
            {
                Inventory myInv = new Inventory();
                invDh = www.downloadHandler.text;
                player.invDh = invDh;
                Debug.Log(invDh);
                myInv = JsonUtility.FromJson<Inventory>(invDh);
                for(int i = 0; i < myInv.data.Length; i++)
                {
                    player.playerInventory += myInv.data[i].item_amount;
                }
            }
        }
    }
}
