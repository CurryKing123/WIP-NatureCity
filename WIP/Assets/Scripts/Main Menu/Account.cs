using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEditor.PackageManager.Requests;

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
                Debug.Log("Form upload complete!");
            }
        }
    }

    IEnumerator Login()
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{"http://localhost:8002/account/get-account"}?rUsername={usernameField.text}&rPassword{passwordField.text}"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();
            

            if (www.result != UnityWebRequest.Result.Success) 
            {
                Debug.Log(www.error);
            }
            else 
            {
                GameAccount returnedAccount = JsonUtility.FromJson<GameAccount>(www.downloadHandler.text);
                Debug.Log($"{returnedAccount.user_id} Welcome " + returnedAccount.username);
            }
        }
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
