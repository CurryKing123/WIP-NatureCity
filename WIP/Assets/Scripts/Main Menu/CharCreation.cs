using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Text;
using System;
using System.IO;
using System.Threading;
using Unity.VisualScripting;
public class CharCreation : MonoBehaviour
{
    [SerializeField] private InputField nameField;
    [SerializeField] private Button createButton;
    [SerializeField] private Button backButton;



    //public void CreateUser()
    //{
    //    StartCoroutine(CreateUsername());
    //}
    //public void CreateName()
    //{
    //    if (nameField.text.Length > 1 && nameField.text.Length < 9)
    //    {
    //        //Reading data from json made in Menu.cs
    //        string dH = File.ReadAllText(Application.persistentDataPath + "CharData.json");
//
    //        //Replacing the blank username with the one typed
    //        CharArray myChar = new CharArray();
    //        myChar = JsonUtility.FromJson<CharArray>(dH);
    //        myChar.data[0].user_name = nameField.text;
    //        myChar.data[0].character_race = "fox";
    //        string json = JsonUtility.ToJson(myChar, true);
//
    //        //Replacing old json with new one that has a new username
    //        File.WriteAllText(Application.persistentDataPath + "CharData.json", json);
    //        Debug.Log("Username Acceptable");
    //        Debug.Log(myChar.data[0].char_id);
    //        CreateUser();
    //    }
    //    else 
    //    {
    //        Debug.Log("Username has to be between 2 and 8 characters long.");
    //    }
    //}
//
    //IEnumerator CreateUsername()
    //{
    //    //Reading new json with username
    //    string json = File.ReadAllText(Application.persistentDataPath + "CharData.json");
//
    //    //Setting the array body as a string for the PUT request
    //    CharArray myChar = new CharArray();
    //    myChar = JsonUtility.FromJson<CharArray>(json);
    //    string jsonUse = JsonUtility.ToJson(myChar.data[0], true);
//
    //    //Setting the char_id to be the same as the one in db
    //    int charID = myChar.data[0].char_id;
    //    Debug.Log(jsonUse);
//
    //    //PUT request
    //    using (UnityWebRequest www = UnityWebRequest.Put($"http://localhost:8002/char/put-char?char_id={charID}", jsonUse))
    //    {
    //        www.SetRequestHeader("key", "1");
    //        www.SetRequestHeader("content-type", "application/json");
    //        yield return www.SendWebRequest();
//
    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log(www.error);
    //        }
    //        else
    //        {
    //            Debug.Log(www.downloadHandler.text);
    //        }
    //    }
    //}

    public void Play()
    {
        SceneManager.LoadScene("GameTest");
    }
    public void GoBack()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
