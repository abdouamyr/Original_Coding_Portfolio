using UnityEngine;
using System.Collections;

public class startScreen : MonoBehaviour {

	public GUIStyle PlayButton;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	//creates the play button that loads the main game
	void OnGUI ()
	{
		PlayButton.fontSize = 100;

		if (GUI.Button(new Rect(400, 200, 150, 100), "Play",PlayButton))
		{
		    Application.LoadLevel("Game");
		}
	}
}
