using UnityEngine;
using System.Collections;

public class movementScript : MonoBehaviour 
{
	public Vector3 tileLocation;

	boardCreationScript boardTiles;
	alphaMovementScript mover; 
	whitePieceTracker whitePieces;
	blackPieceTracker blackPieces;
	turnTracker turn;
	
	
	// Use this for initialization
	void Start () 
	{
		//accesses the location of white pieces
		whitePieces = GameObject.Find("White Pieces").GetComponent<whitePieceTracker>();

		//accesses the location of black pieces
		blackPieces = GameObject.Find("Black Pieces").GetComponent<blackPieceTracker>();

		boardTiles = GameObject.Find("BoardCreator").GetComponent<boardCreationScript>();
		mover = GameObject.Find("BoardCreator").GetComponent<alphaMovementScript>();
		turn = GameObject.Find("Piece Parent").GetComponent<turnTracker>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		for (int x = 0; x < 8; x++)
		{
			for (int y = 0; y < 8; y++)
			{
				if (gameObject.transform.position == boardTiles.tileLocations[x,y])
				{
					tileLocation = boardTiles.tileLocations[x,y];
				}
			}
		}
	}

	void OnMouseDown ()
	{
		if ((gameObject.GetComponent("Halo") as Behaviour).enabled == true && mover.activePiece != null)
		{
			mover.activePiece.transform.position = tileLocation;
			if (mover.activePiece.transform.tag == "White")
			{
				whitePieces.whitePieceHasMoved = true;
				turn.playerTurn = "Black";
				
			}

			if (mover.activePiece.transform.tag == "Black")
			{
				blackPieces.blackPieceHasMoved = true;
				turn.playerTurn = "White";
			}
			
			mover.activePiece = null;
		}
	}
}
