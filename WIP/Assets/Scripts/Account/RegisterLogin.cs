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

public class RegisterLogin : MonoBehaviour
{
    private MainMenu mainMenu;
    public int userId;
    private GetPlayerData getPlayerData;

    [SerializeField] private InputField usernameField;
    [SerializeField] private InputField passwordField;
    [SerializeField] private Button reglogButton;
    [SerializeField] private Button backButton;
    [SerializeField] private DDUserID dduserId;

    private void Start()
    {
        mainMenu = GetComponent<MainMenu>();
        getPlayerData = GetComponent<GetPlayerData>();
        dduserId = GameObject.Find("UserID").GetComponent<DDUserID>();
    }
    public void RegOrLog()
    {
        if (mainMenu.menuState == MainMenu.MenuState.Register)
        {
            CallRegister();
        }
        else
        {
            CallLogin();
        }
    }

    public void CallRegister()
    {
        StartCoroutine(Register());
    }
    public void CallLogin()
    {
        StartCoroutine(Login());
    }
    
    ///Register Button Interaction
    IEnumerator Register()
    {
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:8002/account/post-account", 
        "{ \"username\": \"" + usernameField.text + "\", \"password\": \"" + passwordField.text + "\" }", "application/json"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Registration Complete");
                mainMenu.menuState = MainMenu.MenuState.Main;
            }
        }
        
    }

    IEnumerator Login()
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/account/get-account-login?username={usernameField.text}&password={passwordField.text}"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            MyAccount myAccount = new MyAccount();
            string dH = www.downloadHandler.text;
            myAccount = JsonUtility.FromJson<MyAccount>(dH);
            

            if (www.result != UnityWebRequest.Result.Success) 
            {
                Debug.Log(www.error);
            }
            else 
            {
                if(dH.Contains($"{usernameField.text}"))
                {
                    userId = myAccount.data[0].user_id;
                    Debug.Log("Login Successful!");
                    Debug.Log($"User: {userId}");
                    dduserId.userId = userId;
                    mainMenu.accountState = MainMenu.AccountState.LoggedIn;
                    mainMenu.menuState = MainMenu.MenuState.Main;

                }
                else
                {
                    Debug.Log("Incorrect Credentials");
                }
            }
        }
    }


    

    
    ///Need 8 Characters To Interact With The Button
    public void VerifyInputs()
    {
        reglogButton.interactable = (usernameField.text.Length >= 8 && passwordField.text.Length >= 8);
    }

    public void GoBack()
    {
        SceneManager.LoadScene(0);
    }

}
