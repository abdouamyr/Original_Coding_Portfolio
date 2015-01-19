using UnityEngine;
using System.Collections;

public class tileBehaviorScript : MonoBehaviour 
{
	
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
		//moves pawn to clicked tile, if the tile is a valid move location
		if (clickScript.pawnIsActive == true)
		{
			clickScript.movePawn(gameObject);
		}
	}
}
