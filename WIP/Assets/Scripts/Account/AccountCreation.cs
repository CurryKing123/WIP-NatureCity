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

public class AccountCreation : MonoBehaviour
{
    private MainMenu mainMenu;
    public int userId;

    [SerializeField] private InputField usernameField;
    [SerializeField] private InputField passwordField;
    [SerializeField] private Button reglogButton;
    [SerializeField] private Button backButton;

    private void Start()
    {
        mainMenu = GetComponent<MainMenu>();
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
    private void CallChar(int userId)
    {
        StartCoroutine(GetChar(userId));
    }
    private void PostChar(int userId)
    {
        StartCoroutine(MakeChar(userId));
    }
    public static void GetAccData(string dH)
    {
        File.WriteAllText(Application.persistentDataPath + "CharData.json", dH);
        Debug.Log(Application.persistentDataPath);
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
            Debug.Log(dH);
            

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
                    UserId.user_id = userId;
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

    IEnumerator GetChar(int userId)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/char/get-char-by-id?user_id={userId}"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            CharArray myChar = new CharArray();
            string dH = www.downloadHandler.text;
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
                    Debug.Log("Logging in to new account");
                    Debug.Log(Application.persistentDataPath);
                    PostChar(userId);
                    SceneManager.LoadScene("Character Creation");
                }
                else
                {
                    Debug.Log("Logged in to something");
                    SceneManager.LoadScene("Game Test");
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
