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
    public Button registerButton;
    public Button backButton;
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
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:8002/account/post-account", "{ \"username\": \"" + usernameField.text + "\", \"password\": \"" + passwordField.text + "\" }", "application/json"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Registration Complete!");
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
                    Debug.Log("Login Successful!");
                    Debug.Log(www.downloadHandler.text);
                    Debug.Log(myAccount.data[0].user_id);
                }
                else
                {
                    Debug.Log("Incorrect Credentials");
                }
            }
        }
    }
    [Serializable]
    public class MyAccount
    {

        public Data[] data;
    }
    [Serializable]
    public class Data
    {
        public string username;
        public string password;
        public int user_id;
    }

    
    ///Need 8 Characters To Interact With The Button
     void VerifyInputs()
    {
        registerButton.interactable = (usernameField.text.Length >= 8 && passwordField.text.Length >= 8);
    }

    public void GoBack()
    {
        SceneManager.LoadScene(0);
    }

}
