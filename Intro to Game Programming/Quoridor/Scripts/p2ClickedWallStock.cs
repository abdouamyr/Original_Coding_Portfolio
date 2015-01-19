using UnityEngine;
using System.Collections;

public class p2ClickedWallStock : MonoBehaviour
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

	//activates wall, if it corresponds to playerTUrn
	void OnMouseDown ()
	{
		if (clickScript.playerTurn == ("Player 2's Turn"))
		{
			clickScript.activateClickedStockWall(gameObject);
		}
	}
}
