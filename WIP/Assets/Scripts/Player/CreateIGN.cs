using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Unity.VisualScripting;
using TMPro;

public class CreateIGN : MonoBehaviour
{
    [SerializeField] private InputField nameField;
    [SerializeField] private GameObject namePrompt;
    [SerializeField] private GameObject createButton;
    [SerializeField] private GameObject playerName;
    [SerializeField] private PlayerController player;
    [SerializeField] private MyNetworkPlayer myNetworkPlayer;


    private CharArray myChar;

    private string userName;
    private int charId;
    public string dH;

    private void Start()
    {
        myChar = new CharArray();
        namePrompt = GameObject.Find("IGN").GetComponent<IGN>().namePrompt;
    }



    public void FindPlayer()
    {

        player = GameObject.Find("Player").GetComponent<PlayerController>();
        myNetworkPlayer = GameObject.Find("Player").transform.parent.GetComponent<MyNetworkPlayer>();
        userName = player.userName;
        if (userName == "" )
        {
            namePrompt.SetActive(true);
        }
        else
        {
            namePrompt.SetActive(false);
        }
    }


    public void ConfirmName()
    {
        StartCoroutine(CreateName());
    }

    //Create New Name For Character
    IEnumerator CreateName()
    {  
        myChar = JsonUtility.FromJson<CharArray>(dH);
        charId = myChar.data[0].char_id;
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
                namePrompt.SetActive(false);
                player = GameObject.Find("Player").GetComponent<PlayerController>();
                myNetworkPlayer.displayName = userName;
            }
        }
    }
}
