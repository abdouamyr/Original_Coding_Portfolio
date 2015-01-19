using UnityEngine;
using System.Collections;

public class player1WinDisplay : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI ()
	{
		//victory description
		GUI.Box (new Rect (400, 100, 150, 100), "Player 1 Wins!");

		//when clicked, loads the game again
		if (GUI.Button(new Rect(400, 300, 150, 100), "Play Again"))
		{
			Application.LoadLevel("Game");
		}
	}
}
