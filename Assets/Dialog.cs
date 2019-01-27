using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Dialog : MonoBehaviour
{
    public Text UIText,UIText2,UIText3;
    public Button UIButton,UIButton2,UIButton3;
	public string TextToType,TextToType2,TextToType3;
    public GameObject woman,man,cheer;
    public float TimeToType = 3.0f;
	public bool endWord = false;
    private float textPercentage = 0f;
    void Start()
    {
        UIButton.gameObject.SetActive(true);
        UIButton2.gameObject.SetActive(false);
        UIButton3.gameObject.SetActive(false);
        woman.SetActive(true);
        man.SetActive(false);
        cheer.SetActive(false);
        TextToType ="I can't figure out when this started to happen, but the result is - 127.0.0.1 is not working on any port from anywhere. What else can I do to identify the source of problem and fix it?";
       }

    // Update is called once per frame
    void Update()
    {
        callTyping(TextToType);
    }

    void callTyping(string TextToType){
		int numberOfLettersToShow = (int)(TextToType.Length * textPercentage);
		UIText.text = TextToType.Substring(0, numberOfLettersToShow);
		textPercentage += Time.deltaTime / TimeToType;
		textPercentage = Mathf.Min(1.0f, textPercentage);
		//Debug.Log("%: "+textPercentage);
		if(textPercentage >= 1){
			endWord = true;
			
		}
	}

    public void conver_2(){
        textPercentage = 0f;
        endWord = false;

        UIButton.gameObject.SetActive(false);
        UIButton2.gameObject.SetActive(true);
        UIButton3.gameObject.SetActive(false);
        woman.SetActive(false);
        man.SetActive(true);
        cheer.SetActive(false);
        TextToType ="Can you clarify what you mean by 'working' - what are you expecting to happen? Do you have a server running on the local machine? On which port?";
        callTyping(TextToType);
    }

    public void conver_3(){
        textPercentage = 0f;
        endWord = false;
        UIButton.gameObject.SetActive(false);
        UIButton2.gameObject.SetActive(false);
        UIButton3.gameObject.SetActive(true);
        woman.SetActive(false);
        man.SetActive(true);
        cheer.SetActive(true);
        TextToType ="The line in the hosts file doesn't change anything. I'll send Bit Cat to fixed it.";
        callTyping(TextToType);
    }

    public void skipStory(){
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
