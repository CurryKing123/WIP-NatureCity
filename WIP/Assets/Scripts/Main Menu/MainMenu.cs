using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI alertText;
    public void GoToRegister()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToLogin()
    {
        SceneManager.LoadScene(2);
    }

    public void GoBack()
    {
        SceneManager.LoadScene(0);
    }

    public void GoPlay()
    {
        string dH = (File.ReadAllText(Application.persistentDataPath + "CharData.json"));
        Debug.Log(dH);
        CharArray myChar = new CharArray();
        myChar = JsonUtility.FromJson<CharArray>(dH);
        Debug.Log($"{myChar.data[0].character_race}");
        if (myChar.data[0].character_race == "")
        {
            Debug.Log("New Character");
            SceneManager.LoadScene(4);
        }
    }
}
