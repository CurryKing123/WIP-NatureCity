using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using UnityEditor;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private Button playButton;
    [SerializeField] private Button logButton;
    [SerializeField] private Button regButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button reglogButton;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject reglogScreen;

    public enum AccountState {LoggedIn, LoggedOut}
    public AccountState accountState;
    public enum MenuState {Main, Register, Login}
    public MenuState menuState;

    void Start()
    {
        AccountStateSetup();
        MenuStateSetup();

        menuState = MenuState.Main;

        if (UserId.user_id == 0)
        {
            accountState = AccountState.LoggedOut;
        }
        else
        {
            accountState = AccountState.LoggedIn;
        }
    }

    void Update()
    {
        if (menuState == MenuState.Register)
        {
            mainMenu.SetActive(false);
            backButton.gameObject.SetActive(true);
            reglogScreen.SetActive(true);
            reglogButton.GetComponentInChildren<Text>().text = "Register";
        }
        else if (menuState == MenuState.Login)
        {
            mainMenu.SetActive(false);
            backButton.gameObject.SetActive(true);
            reglogScreen.SetActive(true);
            reglogButton.GetComponentInChildren<Text>().text = "Login";
        }
        else if (menuState == MenuState.Main)
        {
            reglogScreen.SetActive(false);
            mainMenu.SetActive(true);
            if (accountState == AccountState.LoggedIn)
            {
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

    }

    public void GoToRegister()
    {
        menuState = MenuState.Register;
        mainMenu.SetActive(false);
        backButton.gameObject.SetActive(true);
    }

    public void LogInOut()
    {
        if (UserId.user_id > 0)
        {
            accountState = AccountState.LoggedOut;
            Debug.Log("Logged Out");
            UserId.user_id = 0;
        }
        else
        {
            menuState = MenuState.Login;
        }
    }

    public void GoBack()
    {
        menuState = MenuState.Main;
        mainMenu.SetActive(true);
        backButton.gameObject.SetActive(false);
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
            SceneManager.LoadScene(3);
        }
        else
        {
            Debug.Log($"Welcome Back {myChar.data[0].user_name}");
            SceneManager.LoadScene(4);
        }
    }
    private void AccountStateSetup()
    {
        switch (accountState)
        {
            case AccountState.LoggedIn:
                break;
            
            case AccountState.LoggedOut:
                break;
        }
    }

    private void MenuStateSetup()
    {
        switch (menuState)
        {
            case MenuState.Main:
                break;
            
            case MenuState.Register:
                break;

            case MenuState.Login:
                break;
        }
    }
}
