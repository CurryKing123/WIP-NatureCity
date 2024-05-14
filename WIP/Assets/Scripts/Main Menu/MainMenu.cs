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

    private PlayerId playerId;

    public int userId;
    private UserId userIdJson;

    public enum AccountState {LoggedIn, LoggedOut}
    public AccountState accountState;
    public enum MenuState {Main, Register, Login}
    public MenuState menuState;

    void Start()
    {
        userIdJson = new UserId();
        if (File.Exists(Application.persistentDataPath + "UserId.json"))
        {
            string dH = File.ReadAllText(Application.persistentDataPath + "UserId.json");
            userIdJson = JsonUtility.FromJson<UserId>(dH);
            userId = userIdJson.userId;   
        }
        Debug.Log(Application.persistentDataPath);
        //playerId = GameObject.Find("PlayerId").GetComponent<PlayerId>();

        AccountStateSetup();
        MenuStateSetup();

        menuState = MenuState.Main;

        if (userId <= 0)
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
        //States to change the main menu layout
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
                alertText.text = $"{userId}";
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

    public void SerializeUserId()
    {
        userIdJson.userId = userId;
        string userIdToJson = JsonUtility.ToJson(userIdJson, true);
        File.WriteAllText(Application.persistentDataPath + "UserId.json", userIdToJson);
    }

    public void GoToRegister()
    {
        menuState = MenuState.Register;
        mainMenu.SetActive(false);
        backButton.gameObject.SetActive(true);
    }

    public void LogInOut()
    {
        if (userId > 0)
        {
            File.Delete(Application.persistentDataPath + "UserId.json");
            accountState = AccountState.LoggedOut;
            Debug.Log("Logged Out");
            userId = 0;
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
        SceneManager.LoadScene("GameTest");
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
