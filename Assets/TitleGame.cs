using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TitleGame : MonoBehaviour
{
    public AudioSource clickSound;
    public string uid_string,email;
    void Start(){
        uid_string = PlayerPrefs.GetString("uid");
        email = PlayerPrefs.GetString("user");
    }
    void Update()
    {
        if (Input.anyKey)
        {
            clickSound.Play();
            Debug.Log("A key or mouse click has been detected");
            SceneManager.LoadScene("Story", LoadSceneMode.Single);
        }
    }
}
