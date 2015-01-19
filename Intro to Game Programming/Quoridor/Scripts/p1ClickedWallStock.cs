using UnityEngine;
using System.Collections;

public class p1ClickedWallStock : MonoBehaviour 
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
		//activates wall if it corresponds to playerTurn
		if (clickScript.playerTurn == ("Player 1's Turn"))
		{
			clickScript.activateClickedStockWall(gameObject);
		}
	}
}
