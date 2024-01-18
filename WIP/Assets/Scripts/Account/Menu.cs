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

public class Menu : MonoBehaviour
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
    public void GetAccData(string dH)
    {
        System.IO.File.WriteAllText(Application.persistentDataPath + "CharData.json", dH);
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
            Debug.Log(dH);
            

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
                    Debug.Log($"User: {myAccountID}");
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
                if(dH.Length > 29)
                {
                    int myCharID = myChar.data[0].user_id;
                    if(myCharID == myAccountID)
                    {
                        Debug.Log("Logged in to something");
                        GetAccData(dH);
                        SceneManager.LoadScene(0);
                    }
                    else
                    {
                        Debug.Log(www.error);
                    }
                }
                else
                {
                    Debug.Log("Logging in to new account");
                    PostChar(myAccountID);
                    SceneManager.LoadScene(4);
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
