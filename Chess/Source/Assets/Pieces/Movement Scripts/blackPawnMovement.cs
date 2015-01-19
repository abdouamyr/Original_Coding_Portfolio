using UnityEngine;
using System.Collections;

public class blackPawnMovement : MonoBehaviour 
{
	//pawn status
	public bool upgradeReady = true;
	public bool activePawn = false;
	public bool pawnInHomeSquare = true;
	public bool timeToDeactivatePawn;
	
	//references to game controlling scripts
	blackPieceTracker blackPieces;
	whitePieceTracker whitePieces;
	boardCreationScript boardTiles;
	alphaMovementScript tileMover;
	turnTracker turn;
	positionScript coordinates; 
	kingMovement myKing;

	//move and occupied squares
	public GameObject currentSquare;
	public GameObject regMoveSquare;
	public GameObject extraMoveSquare;
	public GameObject takePieceLeftSquare;
	public Color colorOfTakeLeftSquare;
	public GameObject takePieceRightSquare;
	public Color colorOfTakeRightSquare;
	public string pieceColor;
	public string pieceTag = "pawn";

	//locations
	public float homePosX;
	public float homePosY; 

	void Awake () {
		//accesses the location of white pieces
		whitePieces = GameObject.Find("White Pieces").GetComponent<whitePieceTracker>();
		
		//accesses the location of black pieces
		blackPieces = GameObject.Find("Black Pieces").GetComponent<blackPieceTracker>();
		
		//accesses the tiles 
		boardTiles = GameObject.Find("BoardCreator").GetComponent<boardCreationScript>();
		tileMover = GameObject.Find("BoardCreator").GetComponent<alphaMovementScript>();
		
		//accesses the current turn
		turn = GameObject.Find("Piece Parent").GetComponent<turnTracker>();
		
		//turns off the piece's halo
		(gameObject.GetComponent("Halo") as Behaviour).enabled = false;
		
		//saves the pawn's original position
		homePosX = gameObject.transform.position.x;
		homePosY = gameObject.transform.position.y;
		
		//accesses piece's location
		coordinates = gameObject.GetComponent<positionScript>();

		myKing = GameObject.Find("Black King").GetComponent<kingMovement>();
		pieceColor = gameObject.transform.tag;
		gameObject.GetComponent<positionScript>().pieceColor = pieceColor;
		gameObject.GetComponent<positionScript>().pieceTag = pieceTag;
		coordinates.xHome = homePosX;
		coordinates.yHome = homePosY;
	}

	// Use this for initialization
	void Start () 
	{
		tileMover.updateLocation(gameObject, pieceTag);
	}
	
	// Update is called once per frame
	void Update () 
	{
		tileMover.updateLocation(gameObject, pieceTag);
		int x = coordinates.xPos;
		int y = coordinates.yPos;

		//saves the current square where the pawn is
		currentSquare = boardTiles.tiles [x,y];

		//if the pawn has performed a move, deactivate it
		if (blackPieces.blackPieceHasMoved && activePawn)
		{
			deactivatePawn();
		}
		
		//if the pawn has earned the tag "Deactivate" (taken a piece or another piece selected), deactivate it
		if (gameObject.transform.tag == "Deactivate")
		{
			//resets the pawn's tag
			gameObject.transform.tag = "Black";
			deactivatePawn();
		}

		if (x == 0 && upgradeReady) {
			tileMover.upgradePawn(gameObject);
			upgradeReady = false;
		}
	}

	void OnMouseDown ()
	{
		//resets pawnPos values
		int pawnPosX = -1;
		int pawnPosY = -1;

		//turns on the piece's halo, if it is currently inactive
		if (activePawn == false && turn.playerTurn == ("Black"))
		{
			//activates the pawn itself
			(gameObject.GetComponent("Halo") as Behaviour).enabled = true;	
			activePawn = true;
			
			//finds the pawns current location in relation to the array of tiles
			int x = coordinates.xPos;
			int y = coordinates.yPos;

			//saves the current square where the pawn is
			currentSquare = boardTiles.tiles [x,y];
			pawnPosX = x;
			pawnPosY = y;
		
			if (x > 0)
			{
				//saves the pawn's forward square 
				regMoveSquare = boardTiles.tiles[x-1, y];
			
				//saves the pawn's leftward diagonal move (if there's a pawn to its left)
				if (y < 7  && whitePieces.whitePieceOnSquare[x-1, y+1])
				{
					takePieceLeftSquare = boardTiles.tiles [x-1, y+1];
				}
				
				//saves the pawn's rightward diagonal move (if there's a pawn to its right)
				if (y > 0 && whitePieces.whitePieceOnSquare[x-1, y-1])
				{
					takePieceRightSquare = boardTiles.tiles [x-1, y-1];
				}
			}
			
			//if the pawn is in its home position, it is awarded its extraMoveSquare
			if (x == 6 && tileMover.pieceOnMoveSquare(x-1, y) == false)
			{
				extraMoveSquare = boardTiles.tiles [x-2, y];
			}
	
			//if there is no piece on the pawn's moveSquare
			if (pawnPosX > 0 && tileMover.pieceOnMoveSquare((pawnPosX-1), pawnPosY) != true  && myKing.safeIf(x, y,  x-1, y, pieceColor))
			{
				//activate the halo on the moveSquare
				(regMoveSquare.GetComponent("Halo") as Behaviour).enabled = true;
			}

			//if the extraMoveSquare is valid and unnocupied 
			if (extraMoveSquare != null && tileMover.pieceOnMoveSquare((pawnPosX-2), pawnPosY) != true  && myKing.safeIf(x, y,  x-2, y, pieceColor))
			{
				//activate its halo
				(extraMoveSquare.GetComponent("Halo") as Behaviour).enabled = true;
			}	

			//if the left capture is valid
			if (takePieceLeftSquare != null && myKing.safeIf(x, y,  x-1, y+1, pieceColor))
			{
				//highlight the square
				(takePieceLeftSquare.GetComponent("Halo") as Behaviour).transform.renderer.material.color = Color.red;
				(takePieceLeftSquare.GetComponent("Halo") as Behaviour).enabled = true;
			}	

			//if the right capture is vlaid
			if (takePieceRightSquare != null && myKing.safeIf(x, y,  x-1, y-1, pieceColor))
			{
				//highlight the square
				(takePieceRightSquare.GetComponent("Halo") as Behaviour).transform.renderer.material.color = Color.red;
				(takePieceRightSquare.GetComponent("Halo") as Behaviour).enabled = true;
			}	
			
			//sends the piece of the blackPieceTracker script
			blackPieces.returnActivePiece(gameObject);
		}
		
		//turns off the piece's halo, if it is currently active
		else if (activePawn)
		{
			deactivatePawn();
		}

		//if the pawn is taken
		else if (tileMover.activePiece != null && tileMover.activePiece.transform.tag == "White" && currentSquare.transform.renderer.material.color == Color.red)
		{
			//pass it through the taken script method
			blackPieces.takenPiece (gameObject, homePosX, homePosY);
		}
	}

	//method to deactivate the pawn
	void deactivatePawn () 
	{
		if (tileMover.enPassantSquare != null && gameObject.transform.position.x == tileMover.enPassantSquare.transform.position.x && gameObject.transform.position.y == tileMover.enPassantSquare.transform.position.y) {
			tileMover.executeEnPassant(pieceColor);
		}
		//turns of the highlighted squares' halos
		(gameObject.GetComponent("Halo") as Behaviour).enabled = false;
		if (regMoveSquare != null)
		{
			(regMoveSquare.GetComponent("Halo") as Behaviour).enabled = false;
		}

		//resets all of the pawn's move squares to null
		currentSquare = null;
		regMoveSquare = null;
		
		//deactivate extra square, if it's currently active
		if (extraMoveSquare != null)
		{
			(extraMoveSquare.GetComponent("Halo") as Behaviour).enabled = false;
			pawnInHomeSquare = false;
			if (gameObject.transform.position.x == extraMoveSquare.transform.position.x && gameObject.transform.position.y == extraMoveSquare.transform.position.y){
				tileMover.enableEnPassant(gameObject, "white", coordinates.xPos, coordinates.yPos, homePosX, homePosY);
			}
			extraMoveSquare = null;
		}

		//deactivates left capture, if it is currently active
		if (takePieceLeftSquare != null)
		{
			(takePieceLeftSquare.GetComponent("Halo") as Behaviour).enabled = false;
			takePieceLeftSquare.transform.renderer.material.color = colorOfTakeLeftSquare;
			takePieceLeftSquare = null;
		}
		
		//deactivates right capture, if it is currently valid 
		if (takePieceRightSquare != null)
		{	
			(takePieceRightSquare.GetComponent("Halo") as Behaviour).enabled = false;
			takePieceRightSquare.transform.renderer.material.color = colorOfTakeRightSquare;
			takePieceRightSquare = null;
		}

		//deactivates the pawn
		activePawn = false;
		tileMover.moveCommitted = false;
		blackPieces.blackPieceHasMoved = false;
	
	}		
}