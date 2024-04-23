using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt_EmailAddress;
    private void OnEnable()
    {
        AuthenticationManager.event_loadEmailAddressOnUI += DisplayUserInfo;
    }
    private void OnDisable()
    {
        AuthenticationManager.event_loadEmailAddressOnUI -= DisplayUserInfo;
    }
    void DisplayUserInfo()
    {
        txt_EmailAddress.text = "Email: " + PlayerPrefs.GetString("email");

    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
