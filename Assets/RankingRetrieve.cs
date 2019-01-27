using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
public class RankingRetrieve : MonoBehaviour
{
    public DatabaseReference reference;
	public List<string> rankingMembers = new List<string>();
    public List<string> ScoreArray = new List<string>();
	public Text Rank1,Rank2,Rank3,Rank4,Rank5;
    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://antchatbot.firebaseio.com/");
  		reference = FirebaseDatabase.DefaultInstance.RootReference;	

  		//Add Name
  		FirebaseDatabase.DefaultInstance
	        .GetReference("users").OrderByChild("score").LimitToFirst(5)
	        .ValueChanged += HandleValueChanged;
    }

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
	{
	    if (args.DatabaseError != null)
	    {
	        Debug.LogError(args.DatabaseError.Message);
	        return;
	    }
	    var livingRoomItems = args.Snapshot.Value as Dictionary<string, object>;
	    foreach (var item in livingRoomItems)
	    {
	        //Debug.Log(item.Key); // Kdq6...
	        var values = item.Value as Dictionary<string, object>;
	        foreach (var v in values)
	        {
	            //Debug.Log(v.Key + ":" + v.Value);
	            if(v.Key=="email"){
	            	rankingMembers.Add(""+v.Value);
	            }
                
	            
	        }
	    }

		     Rank1.text = ""+rankingMembers[0].ToString();
		     Rank2.text = ""+rankingMembers[1].ToString();
		     Rank3.text = ""+rankingMembers[2].ToString();
		     Rank4.text = ""+rankingMembers[3].ToString();
		     Rank5.text = ""+rankingMembers[4].ToString();
	}

    public void backtoGame(){
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
