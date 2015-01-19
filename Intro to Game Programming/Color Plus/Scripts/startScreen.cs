using UnityEngine;
using System.Collections;

public class startScreen : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnGUI ()
	{
		if(GUI.Button(new Rect(20,40,80,20), "Start")) 
			{
				Application.LoadLevel("Main Game");
			}
	//display start button
	//when clicked, start button loads "GameScene"
	
	}
}
