using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Module_fb_Auth : MonoBehaviour
{
    private FirebaseAuth auth;
    public GameObject signup_toggle,signin_toggle;
    public string UID,uid_string;
    public InputField email_input, password_input;
    public Button SignupButton, SigninButton;
    public Text ErrorText;
   
    void Start()
    {
        uid_string = PlayerPrefs.GetString("uid");
        UID = PlayerPrefs.GetString("uid");
        if(uid_string == ""){
            Debug.Log("Register");
            //StartCoroutine(LoadNewScene());
        }else{
            StartCoroutine(LoadNewScene());
        }
        signup_toggle.SetActive(true);
        signin_toggle.SetActive(false);
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        //auth = FirebaseAuth.DefaultInstance;
        SignupButton.onClick.AddListener(() => Signup(email_input.text, password_input.text));
        SigninButton.onClick.AddListener(() => LoginAction(email_input.text, password_input.text));
    }

   public void Signup(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)){
            //Error handling
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync error: " + task.Exception);
                if (task.Exception.InnerExceptions.Count > 0)
                    UpdateErrorMessage(task.Exception.InnerExceptions[0].Message);
                return;
            }

            FirebaseUser newUser = task.Result; // Firebase user has been created.
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            UpdateErrorMessage("Signup Success");
            StartCoroutine(LoadNewScene());
            PlayerPrefs.SetString("email", ""+newUser.DisplayName);
            PlayerPrefs.SetString("user", ""+email_input.text);
            PlayerPrefs.SetString("uid", ""+newUser.UserId);
        });
    }

    private void UpdateErrorMessage(string message)
    {
        ErrorText.text = message;
        Invoke("ClearErrorMessage", 3);
    }

    void ClearErrorMessage()
    {
        ErrorText.text = "";
    }
    public void LoginAction(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync error: " + task.Exception);
                if (task.Exception.InnerExceptions.Count > 0)
                    UpdateErrorMessage(task.Exception.InnerExceptions[0].Message);
                return;
            }

            FirebaseUser user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);

            PlayerPrefs.SetString("LoginUser", user != null ? user.Email : "Unknown");
            StartCoroutine(LoadNewScene());
            PlayerPrefs.SetString("email", ""+email_input.text);

        });
    }
    IEnumerator LoadNewScene() {

        yield return new WaitForSeconds(0.5f);

        AsyncOperation async = SceneManager.LoadSceneAsync("Title");
        while (!async.isDone) {
            yield return null;
        }
        //SceneManager.LoadScene("Story", LoadSceneMode.Single);
    }

    public void SkipPlay(){
        PlayerPrefs.SetString("uid", "sample_game_bitcat_uid");
        PlayerPrefs.SetString("email", "prayut.chan-ocha@thai.land");
        SceneManager.LoadScene("Story", LoadSceneMode.Single);
    }

    public void ToggleSignup(){
        signup_toggle.SetActive(false);
        signin_toggle.SetActive(true);
    }
}
