using UnityEngine;
using System.Collections;

public class whitePieceTracker : MonoBehaviour {
	//acesses the other scripts
	boardCreationScript boardTiles;
	alphaMovementScript mover; 
	turnTracker turn;

	//tracks the white pieces
	public bool[,]whitePieceOnSquare;
	public bool whitePieceHasMoved;

	public GameObject takenPieceParent;
	// Use this for initialization
	void Start () 
	{
		//reference to the other scripts
		boardTiles = GameObject.Find("BoardCreator").GetComponent<boardCreationScript>();
		mover = GameObject.Find("BoardCreator").GetComponent<alphaMovementScript>();
		turn = GameObject.Find("Piece Parent").GetComponent<turnTracker>();
		
		//creates the boolean array for tracking white pieces
		whitePieceOnSquare = new bool[8, 8];
	}
	
	// Update is called once per frame
	void Update () 
	{
		//identifies which squares are occupied by white pieces
		for (int x = 0; x < 8; x++)
		{
			for (int y = 0; y < 8; y++)
			{
				whitePieceOnSquare[x,y] = false;
				foreach(Transform child in transform)
				{
					if (boardTiles.tiles[x,y].transform.position.x == child.gameObject.transform.position.x
						&& boardTiles.tiles[x,y].transform.position.y == child.gameObject.transform.position.y)
					{
						whitePieceOnSquare[x,y] = true;
					}
				}
			}
		}	
	}
	
	//method: deactivates the currently active piece and activates the newly passed one
	public void returnActivePiece (GameObject passedPiece)
	{
		if (mover.activePiece != null && mover.activePiece != passedPiece)
		{
			mover.moveCommitted = true;
			mover.activePiece.transform.tag = "Deactivate";
			whitePieceHasMoved = true;
			mover.activePiece = null;
		}
		mover.activePiece = passedPiece;
	}

	//method: moves the taken piece to its taken position, and deactivates the active piece
	public void takenPiece (GameObject passedPiece, float x, float y) 
	{
		if (passedPiece != mover.enPassantPawn) { 
			mover.activePiece.transform.position = passedPiece.transform.position;
			mover.activePiece.transform.tag = "Deactivate";
			mover.activePiece = null;
		}
		turn.playerTurn = "White";
		passedPiece.transform.position = new Vector3 (x+24, y, 0);	
		passedPiece.transform.parent = takenPieceParent.transform;
	}
}
