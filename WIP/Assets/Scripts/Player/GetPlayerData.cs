using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class GetPlayerData : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private GameObject namePrompt;

    private int userId;
    private int charId;
    public string dH;

    private CharArray myChar;

    void Start()
    {
        userId = UserId.user_id;
        myChar = new CharArray();
    }

    public void CallChar()
    {
        StartCoroutine(GetChar());
    }
    private void PostChar(int userId)
    {
        StartCoroutine(MakeChar(userId));
    }
    private void ConfirmName(string dH)
    {
        StartCoroutine(CreateName(dH));
    }

    IEnumerator GetChar()
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
                    yield return GetChar();
                }
                else
                {
                    if (myChar.data[0].user_name == "")
                    {
                        namePrompt.SetActive(true);
                    }
                    Debug.Log("Found Character");
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
        }
    }

    //Create New Name For Character
    IEnumerator CreateName(string dH)
    {  
        myChar = JsonUtility.FromJson<CharArray>(dH);
        myChar.data[0].user_name = nameField.text;
        charId = myChar.data[0].char_id;
        string jsonUse = JsonUtility.ToJson(myChar.data[0], true);

        using (UnityWebRequest www = UnityWebRequest.Put($"http://localhost:8002/char/put-char?{charId}", jsonUse))
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
                namePrompt.SetActive(false);
            }
        }
    }
}
