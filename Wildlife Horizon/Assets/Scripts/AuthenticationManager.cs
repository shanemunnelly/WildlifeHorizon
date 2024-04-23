using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine.UI;
using System;
struct UserInformation
{
    public string email;
    public string password;
}
public class AuthenticationManager : MonoBehaviour
{
    [SerializeField] private DependencyStatus dependencyStatus;
    FirebaseAuth auth;
    [Header("Input field")]
    [SerializeField] private TMP_InputField txt_Email;
    [SerializeField] private TMP_InputField txt_Passowrd;

    [SerializeField] private TextMeshProUGUI txt_LoggedInResult;

    [SerializeField] private Button btn_Login;
    [SerializeField] private Button btn_Signup;

    [SerializeField] private Transform loading;
    [SerializeField] private Transform loginPanel;
    [SerializeField] private Transform mainMenuPanel;

    private UserInformation userInformation;

    public static event Action event_loadEmailAddressOnUI;

    [SerializeField] Button btn_Signout, btn_Loginagain;

    [SerializeField] Transform emailBox;
    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                print("available");
                InitializeFirebase();

            }
            else
            {

                Debug.LogError("could not resolve all fire base dependency" + dependencyStatus);
            }
        });

    }
    private void Start()
    {
        if (PlayerPrefs.GetInt("loggedIn", 0) == 1)
        {
            btn_Signout.gameObject.SetActive(true);
            btn_Loginagain.gameObject.SetActive(false);
            emailBox.gameObject.SetActive(true);

            SetLoginAndMenuPanelState(false, true);
            LoadUserInformation();
            Invoke("WaitForAuthentication", 2f);
        }
        else
        {
            btn_Signout.gameObject.SetActive(false);
            btn_Loginagain.gameObject.SetActive(true);
            emailBox.gameObject.SetActive(false);
        }

    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;


    }
    public void LoginToExistingTask()
    {
        StartCoroutine(Login(new UserInformation { email = txt_Email.text, password = txt_Passowrd.text }));
    }
    void WaitForAuthentication()
    {
        StartCoroutine(Login(userInformation));

    }
    IEnumerator Login(UserInformation info)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(info.email, info.password);

        btn_Signup.gameObject.SetActive(false);
        btn_Login.gameObject.SetActive(false);
        loading.gameObject.SetActive(true);

        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);




        if (loginTask.Exception != null)
        {

            btn_Signup.gameObject.SetActive(true);
            btn_Login.gameObject.SetActive(true);
            loading.gameObject.SetActive(false);

            Debug.LogWarning(message: $"Failed to login with {loginTask.Exception}");

            var firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            var errorCode = (AuthError)firebaseException.ErrorCode;

            txt_LoggedInResult.gameObject.SetActive(true);
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    txt_LoggedInResult.text = "Missing Email Address";
                    break;
                case AuthError.MissingPassword:
                    txt_LoggedInResult.text = "Missing Password";
                    break;
                case AuthError.InvalidEmail:
                    txt_LoggedInResult.text = "Invalid Email Address";
                    break;
                case AuthError.WrongPassword:
                    txt_LoggedInResult.text = "Wrong Passowrd";
                    break;
            }
        }
        else
        {
            if (PlayerPrefs.GetInt("loggedIn", 0) == 0)
            {
                SetLoginAndMenuPanelState(false, true);
                SafeUserInformation(new UserInformation { email = txt_Email.text, password = txt_Passowrd.text });
                btn_Signout.gameObject.SetActive(true);
                btn_Loginagain.gameObject.SetActive(false);
                emailBox.gameObject.SetActive(true);

                PlayerPrefs.SetInt("loggedIn", 1);
            }

            event_loadEmailAddressOnUI?.Invoke();



            AuthResult result = loginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
        }

    }

    void LoadUserInformation()
    {

        userInformation.email = PlayerPrefs.GetString("email");
        userInformation.password = PlayerPrefs.GetString("password");


        event_loadEmailAddressOnUI?.Invoke();

    }
    void SafeUserInformation(UserInformation info)
    {
        PlayerPrefs.SetString("email", info.email);
        PlayerPrefs.SetString("password", info.password);

    }
    void SetLoginAndMenuPanelState(bool isLoginPanel, bool isMenuPanel)
    {
        loginPanel.gameObject.SetActive(isLoginPanel);
        mainMenuPanel.gameObject.SetActive(isMenuPanel);

    }
    public void Signout()
    {
        auth.SignOut();

        btn_Signout.gameObject.SetActive(false);
        btn_Loginagain.gameObject.SetActive(true);
        emailBox.gameObject.SetActive(false);
        PlayerPrefs.SetInt("loggedIn", 0);

    }
}
