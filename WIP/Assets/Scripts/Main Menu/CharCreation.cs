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
using Unity.VisualScripting;
public class CharCreation : MonoBehaviour
{
    [SerializeField] private InputField nameField;
    [SerializeField] private Button createButton;
    [SerializeField] private Button backButton;
    public void CreateName()
    {
        if (nameField.text.Length > 1 && nameField.text.Length < 9)
        {
            string dH = File.ReadAllText(Application.persistentDataPath + "CharData.json");
            CharArray myChar = new CharArray();
            myChar = JsonUtility.FromJson<CharArray>(dH);
            myChar.data[0].user_name = nameField.text;
            string json = JsonUtility.ToJson(myChar, true);
            File.WriteAllText(Application.persistentDataPath + "CharData.json", json);
            Debug.Log("Username Acceptable");
            Debug.Log(myChar.data[0].char_id);
            StartCoroutine(CreateUsername());
        }
        else 
        {
            Debug.Log("Username has to be between 2 and 8 characters long.");
        }
    }

    IEnumerator CreateUsername()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "CharData.json");
        CharArray myChar = new CharArray();
        myChar = JsonUtility.FromJson<CharArray>(json);
        string jsonUse = JsonUtility.ToJson(myChar.data[0], true);
        int charID = myChar.data[0].char_id;
        using (UnityWebRequest www = UnityWebRequest.Put($"http://localhost:8002/char/put-char?char_id={charID}", jsonUse))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Upload complete!");
            }
        }
    }
    public void GoBack()
    {
        SceneManager.LoadScene(0);
    }

}
