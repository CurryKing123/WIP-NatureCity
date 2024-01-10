using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Text;
using System;

public class Account : MonoBehaviour
{
    public InputField usernameField;
    public InputField passwordField;
    public Button reglogButton;
    public Button backButton;
    public void CallRegister()
    {
        StartCoroutine(Register());
    }
    public void CallLogin()
    {
        StartCoroutine(Login());
    }
    private void CallChar(int myAccountID)
    {
        StartCoroutine(GetChar(myAccountID));
    }
    private void PostChar(int myAccountID)
    {
        StartCoroutine(MakeChar(myAccountID));
    }
    
    ///Register Button Interaction
    IEnumerator Register()
    {
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:8002/account/post-account", "{ \"username\": \"" + usernameField.text + "\", \"password\": \"" + passwordField.text + "\" }", "application/json"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            MyAccount myAccount = new MyAccount();
            

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Registration Complete");
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
                    int myAccountID = myAccount.data[0].user_id;
                    Debug.Log("Login Successful!");
                    Debug.Log($"User: {myAccount.data[0].user_id}");
                    CallChar(myAccountID);
                }
                else
                {
                    Debug.Log("Incorrect Credentials");
                }
            }
        }
    }

    IEnumerator GetChar(int myAccountID)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/char/get-char-by-id?user_id={myAccountID}"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            CharArray myChar = new CharArray();
            string dH = www.downloadHandler.text;
            Debug.Log($"{dH.Length}");
            myChar = JsonUtility.FromJson<CharArray>(dH);

            if (www.result != UnityWebRequest.Result.Success) 
            {
                Debug.Log(www.error);
            }
            
            else
            {
                if(dH.Length>29)
                {
                    Debug.Log("Logged in to something");
                }
                else
                {
                    Debug.Log("Making new character page");
                    PostChar(myAccountID);
                }
            }
        }
    }
    IEnumerator MakeChar(int myAccountID)
    {
        using (UnityWebRequest www = UnityWebRequest.Post($"http://localhost:8002/char/post-char", "{ \"user_id\": \"" + myAccountID + "\"}", "application/json"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            CharArray myChar = new CharArray();
            string dH = www.downloadHandler.text;
            Debug.Log($"{dH}");
            myChar = JsonUtility.FromJson<CharArray>(dH);
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
