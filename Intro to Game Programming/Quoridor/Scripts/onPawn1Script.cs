using UnityEngine;
using System.Collections;

public class onPawn1Script : MonoBehaviour {

	gameController clickScript;
	
	// Use this for initialization
	void Start () 
	{
		//reference to the gameController class
		clickScript = GameObject.Find("GameController").GetComponent<gameController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnMouseDown ()
	{
		//passes the pawn into activatePawn method, if it corresponds to the current player's turn
		if (clickScript.playerTurn == ("Player 1's Turn"))
		{
			clickScript.activatePawn (gameObject);
		}
	}
}
