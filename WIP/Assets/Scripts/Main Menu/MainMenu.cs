using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
        SceneManager.LoadScene(3);
    }
}
