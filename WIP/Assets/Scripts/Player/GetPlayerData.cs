using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class GetPlayerData : MonoBehaviour
{
    [SerializeField] private InputField nameField;
    [SerializeField] private GameObject namePrompt;

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
    private PlayerController playCont;

    void Start()
    {
        userId = UserId.user_id;
        myChar = new CharArray();
        itMan = gameObject.GetComponent<ItemManagement>();
        playCont = gameObject.GetComponent<PlayerController>();
    }

    public void CallChar(int userId)
    {
        StartCoroutine(GetChar(userId));
    }
    private void PostChar(int userId)
    {
        StartCoroutine(MakeChar(userId));
    }

    public void ConfirmName()
    {
        StartCoroutine(CreateName());
    }

    public void CallRace(string race)
    {
        StartCoroutine(GetRace(race));
    }
    public void CallInv(int charId)
    {
        StartCoroutine(GetInv(charId));
    }

    IEnumerator GetChar(int userId)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/char/get-char-by-id?user_id={userId}"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            dH = www.downloadHandler.text;
            Debug.Log(dH);
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
                    if (myChar.data[0].user_name == "")
                    {
                        namePrompt.SetActive(true);
                    }

                    charId = myChar.data[0].char_id;
                    charRace = myChar.data[0].character_race;
                    CallRace(charRace);

                    equip = new string[]{myChar.data[0].equip_item_1,
                    myChar.data[0].equip_item_2,
                    myChar.data[0].equip_item_3,
                    myChar.data[0].equip_item_4,
                    myChar.data[0].equip_item_5};

                    for(int i=0; i<5; i++)
                    {
                        itMan.CallEquip(equip[i]);
                    }

                    CallInv(charId);

                    playCont.GetPlayerData();
                }
            }
        }
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
                charRace = JsonUtility.FromJson<Races>(dH);
                speed = charRace.data[0].move_speed;
                carryAmount = charRace.data[0].carry_amount;
                playCont.GetPlayerData();
            }
        }
    }


    //Create New Name For Character
    IEnumerator CreateName()
    {  
        myChar = JsonUtility.FromJson<CharArray>(dH);
        userName = nameField.text;

        myChar.data[0].user_name = userName;
        string jsonUse = JsonUtility.ToJson(myChar.data[0], true);

        using (UnityWebRequest www = UnityWebRequest.Put($"http://localhost:8002/char/put-char?char_id={charId}", jsonUse))
        {
            www.SetRequestHeader("key", "1");
            www.SetRequestHeader("content-type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) 
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("New Name Created");
                playCont.GetPlayerData();
                namePrompt.SetActive(false);
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
                Debug.Log(invDh);
                myInv = JsonUtility.FromJson<Inventory>(invDh);
                for(int i = 0; i < myInv.data.Length; i++)
                {
                    playerInventory += myInv.data[i].item_amount;
                }
                playCont.GetPlayerData();
            }
        }
    }
}
