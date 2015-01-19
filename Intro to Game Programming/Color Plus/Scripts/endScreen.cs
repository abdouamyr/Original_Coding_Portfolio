using UnityEngine;
using System.Collections;

public class endScreen : MonoBehaviour {
	public static int totalScore;
	public static bool gameWin;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI ()
	{
		if(GUI.Button(new Rect(20,40,80,20), "Play Again")) 
			{
				Application.LoadLevel("Main Game");
			}
		GUI.Box(new Rect(20,80,80,20), "Score:" + totalScore.ToString ());
		
		if (gameWin == true)
			{
				GUI.Box(new Rect(20,120,80,20), "Victory!");
			}
		else 
			{
				GUI.Box(new Rect(20,120,80,20), "Defeat!");
			}
	//display "win" or "lose"
	//when clicked, replay button loads "GameScene"
	//display final score
	
	}
	
}
