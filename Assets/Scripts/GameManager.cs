using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
public class GameManager : MonoBehaviour {

	public GameObject pillarPrefab;
	public GameObject pillarHolder; 
	public GameObject cube;
	public GameObject CheerUp;
	public GameObject loseUI,score_game;
	public Text scoreText, finalScoreText, bestScoreText, jumpsText;
	public int pillarsToMake = 20;
	public float minDistance = 3f, maxDistance = 5f;
	public int score, cheer_score, jumps;
	Vector3 spawnPosition;
	public string emailAddress,uid;
	public DatabaseReference reference;
	bool gameOver;

	// Use this for initialization
	void Start () {
		spawnPosition = transform.position;
		makingPillars();
		score = 0;
		gameOver = false;
		CheerUp.SetActive(false);

		//Firebase
        emailAddress = ""+PlayerPrefs.GetString("user");
        uid = ""+PlayerPrefs.GetString("uid");
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://antchatbot.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

		reference.Child("users").Child(""+uid).Child("email").SetValueAsync(""+emailAddress);
        //reference.Child("users").Child(""+uid).Child("score").SetValueAsync(0);
	}

	void FixedUpdate(){
		Time.timeScale = 2;
		cheer_score = PlayerPrefs.GetInt("bestScore",0);
		if(score > cheer_score){
			CheerUp.SetActive(true);
			StartCoroutine(HideCheer());
		}

		if(cube.transform.position.y < -13 && !gameOver){
			gameOver = true;
			Camera.main.gameObject.GetComponent<CameraBehaviour>().gameOver = true;
			StartCoroutine(loseGame());
		}
	}

	public void scoreUp(int points){
		score+= points;
		jumps++;
		scoreText.text = ""+points;
	}

	void makingPillars(){
		for(int i = 0; i < pillarsToMake; i++){
			GameObject tempPillar = Instantiate(pillarPrefab, spawnPosition, pillarPrefab.transform.rotation);
			tempPillar.transform.SetParent(pillarHolder.transform);
			tempPillar.GetComponent<PillarBehaviour>().gameManager = this;
			tempPillar.GetComponent<PillarBehaviour>().cube = cube;
			tempPillar.GetComponent<PillarBehaviour>().score = i+1;
			spawnPosition = new Vector3(spawnPosition.x, Random.Range(-8,-4), 
			spawnPosition.z + Random.Range(minDistance, maxDistance));
		}	
	}

	IEnumerator HideCheer(){
		yield return new WaitForSeconds(1.14f);
		CheerUp.SetActive(false);
	}

	IEnumerator loseGame(){
		cube.GetComponent<CubeBehaviour>().loseGame = true;
		for(int i = 0; i >= 0; i--){
			float k = (float) i/10;
			//pillarHolder.transform.localScale = new Vector3(k,k,k);
			yield return new WaitForSeconds(.05f);
		}
		//pillarHolder.SetActive(false);
		if(score > PlayerPrefs.GetInt("bestScore",0)) PlayerPrefs.SetInt("bestScore",score);
		score_game.SetActive(false);
		loseUI.SetActive(true);

        
        reference.Child("users").Child(""+uid).Child("score").SetValueAsync(score);
		reference.Child("users").Child(""+uid).Child("jump").SetValueAsync(jumps);

		finalScoreText.GetComponent<Text>().text = ""+ score;
		bestScoreText.GetComponent<Text>().text = "HIGH SCORE:"+ PlayerPrefs.GetInt("bestScore",0);
		jumpsText.GetComponent<Text>().text = "JUMPS:"+ jumps;
	}

	public void restart(){
		SceneManager.LoadScene("Main");
	}

	public void RankingScene(){
        SceneManager.LoadScene("Ranking", LoadSceneMode.Single);
    }
}
