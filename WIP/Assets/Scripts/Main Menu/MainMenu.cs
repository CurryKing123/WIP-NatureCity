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
    [SerializeField] private Button playButton;
    [SerializeField] private Button logButton;
    [SerializeField] private Button regButton;


    void Awake()
    {
        if (File.Exists(Application.persistentDataPath + "CharData.json"))
        {
            string dH = (File.ReadAllText(Application.persistentDataPath + "CharData.json"));
            CharArray myChar = new CharArray();
            myChar = JsonUtility.FromJson<CharArray>(dH);
            alertText.text = "Welcome Back " + myChar.data[0].user_name;
            playButton.gameObject.SetActive(true);
            regButton.gameObject.SetActive(false);
            logButton.GetComponentInChildren<Text>().text = "Logout";
        }
        else
        {
            alertText.text = "No User Found";
            playButton.gameObject.SetActive(false);
            regButton.gameObject.SetActive(true);
            logButton.GetComponentInChildren<Text>().text = "Login";
        }
    }
    public void GoToRegister()
    {
        SceneManager.LoadScene(1);
    }

    public void LogInOut()
    {
        if (File.Exists(Application.persistentDataPath + "CharData.json"))
        {
            File.Delete(Application.persistentDataPath + "CharData.json");
            Awake();
        }
        else
        {
            SceneManager.LoadScene(2);
        }
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
        Debug.Log($"{myChar.data[0].user_name}");
        
        if (myChar.data[0].user_name == "")
        {
            Debug.Log("New Character");
            SceneManager.LoadScene(4);
        }
        else
        {
            Debug.Log($"Welcome Back {myChar.data[0].user_name}");
            SceneManager.LoadScene(3);
        }
    }
}
