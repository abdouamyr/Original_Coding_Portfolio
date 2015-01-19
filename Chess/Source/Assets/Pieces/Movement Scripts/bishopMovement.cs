using UnityEngine;
using System.Collections;

public class bishopMovement : MonoBehaviour 
{
	//script references
	blackPieceTracker blackPieces;
	whitePieceTracker whitePieces;
	boardCreationScript boardTiles;
	alphaMovementScript tileMover;
	turnTracker turn;
	positionScript coordinates; 
	kingMovement myKing;

	//move square arrays
	public GameObject [] diagMoveSquaresUpLeft;
	public GameObject [] diagMoveSquaresUpRight;
	public GameObject [] diagMoveSquaresDownLeft;
	public GameObject [] diagMoveSquaresDownRight;
	public GameObject [] attackSquares; 
	public GameObject currentSquare; 
	public Color [] attackSquaresColor;

	//piece information and status
	public string pieceColor;
	public string pieceTag = "bishop";
	public int maxMoveSquares = 8; 
	public int numAttackSquare;
	public float homePosX;
	public float homePosY;
	public bool pieceActive = false;
	
	void Awake () {
		//stores piece's color
		pieceColor = gameObject.transform.tag;
		if (pieceColor == "Black") {
			myKing = GameObject.Find("Black King").GetComponent<kingMovement>();
		} else if (pieceColor == "White") {
			myKing = GameObject.Find("White King").GetComponent<kingMovement>();
		}
		
		//turns off the piece's halo
		(gameObject.GetComponent("Halo") as Behaviour).enabled = false;
		
		//accesses the location of white pieces
		whitePieces = GameObject.Find("White Pieces").GetComponent<whitePieceTracker>();
		
		//accesses the location of black pieces
		blackPieces = GameObject.Find("Black Pieces").GetComponent<blackPieceTracker>();
		
		//accesses the tiles 
		boardTiles = GameObject.Find("BoardCreator").GetComponent<boardCreationScript>();
		tileMover = GameObject.Find("BoardCreator").GetComponent<alphaMovementScript>();
		
		//accesses the current turn
		turn = GameObject.Find("Piece Parent").GetComponent<turnTracker>();
		
		//accesses piece's location
		homePosX = gameObject.transform.position.x;
		homePosY = gameObject.transform.position.y;
		gameObject.GetComponent<positionScript>().xHome = homePosX;
		gameObject.GetComponent<positionScript>().yHome = homePosY;
		coordinates = gameObject.GetComponent<positionScript>();

		//creates the move and attack square arrays
		attackSquares = new GameObject[maxMoveSquares];
		diagMoveSquaresUpLeft = new GameObject[maxMoveSquares];
		diagMoveSquaresUpRight = new GameObject[maxMoveSquares];
		diagMoveSquaresDownLeft = new GameObject[maxMoveSquares];
		diagMoveSquaresDownRight = new GameObject[maxMoveSquares];
		attackSquaresColor = new Color[maxMoveSquares];

		gameObject.GetComponent<positionScript>().pieceColor = pieceColor;
		gameObject.GetComponent<positionScript>().pieceTag = pieceTag;
	}

	// Use this for initialization
	void Start () 
	{
		//updates piece location
		tileMover.updateLocation(gameObject, pieceTag);
		coordinates.xHome = homePosX;
		coordinates.yHome = homePosY;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//updates piece location
		tileMover.updateLocation(gameObject, pieceTag);
		int x = coordinates.xPos;
		int y = coordinates.yPos;

		//deactivates bishop if it is not the active piece
		if (tileMover.moveCommitted || coordinates.timeToDeactivate || gameObject.transform.tag == "Deactivate") {
			pieceActive = false; 
			coordinates.timeToDeactivate = false; 
			deactivateBishop();
			gameObject.transform.tag = pieceColor;
		}

		//updates the square the bishop is occupying
		currentSquare = boardTiles.tiles[x,y];
	}

	void OnMouseDown ()
	{
		//actives/deactivates the bishop if clicked at a valid time
		if ((turn.playerTurn == "Black" && gameObject.transform.tag == "Black") || (turn.playerTurn == "White" && gameObject.transform.tag == "White")) {
			//turns on the piece's halo, if it is currently inactive
			if (pieceActive == false) {
				(gameObject.GetComponent("Halo") as Behaviour).enabled = true;	
				pieceActive = true;
				int x = coordinates.xPos;
				int y = coordinates.yPos;
				
				int upLocation;
				int downLocation;
				int leftLocation;
				int rightLocation;

				int numUpLeftMoves = 0;
				upLocation = y;
				leftLocation = x;
				while (numUpLeftMoves < 8 && upLocation < 7 && leftLocation > 0 && tileMover.pieceOnMoveSquare(leftLocation-1, upLocation+1) == false) {
					diagMoveSquaresUpLeft[numUpLeftMoves] = boardTiles.tiles[leftLocation-1, upLocation+1];
					if (myKing.safeIf(x, y,  leftLocation-1, upLocation+1, pieceColor)) {
						(diagMoveSquaresUpLeft[numUpLeftMoves].GetComponent("Halo") as Behaviour).enabled = true;
					}
					upLocation++;
					leftLocation--;
					numUpLeftMoves++;
				}
				if (myKing.safeIf(x, y, leftLocation-1, upLocation+1, pieceColor)) {
					activateAttackSquare(leftLocation-1, upLocation+1);
//					print ("time to attack");
				}
				
				int numUpRightMoves = 0;
				upLocation = y;
				rightLocation = x;
				while (numUpRightMoves < 8 && upLocation < 7 && rightLocation < 7 && tileMover.pieceOnMoveSquare(rightLocation+1, upLocation+1) == false) {
					diagMoveSquaresUpRight[numUpRightMoves] = boardTiles.tiles[rightLocation+1, upLocation+1];
					if (myKing.safeIf(x, y,  rightLocation+1, upLocation+1, pieceColor)) {
						(diagMoveSquaresUpRight[numUpRightMoves].GetComponent("Halo") as Behaviour).enabled = true;	
					}
					upLocation++;
					rightLocation++;
					numUpRightMoves++;
				}
				if (myKing.safeIf(x, y,  rightLocation+1, upLocation+1, pieceColor)) {
					activateAttackSquare(rightLocation+1, upLocation+1);
//					print ("time to attack");
				}
				
				int numDownLeftMoves = 0;
				downLocation = y;
				leftLocation = x;
				while (numDownLeftMoves < 8 && downLocation > 0 && leftLocation > 0 && tileMover.pieceOnMoveSquare(leftLocation-1, downLocation-1) == false) {
					diagMoveSquaresDownLeft[numDownLeftMoves] = boardTiles.tiles[leftLocation-1, downLocation-1];
					if (myKing.safeIf(x, y,  leftLocation-1, downLocation-1, pieceColor)) {
						(diagMoveSquaresDownLeft[numDownLeftMoves].GetComponent("Halo") as Behaviour).enabled = true;	
					}
					downLocation--;
					leftLocation--;
					numDownLeftMoves++;
				}
				if (myKing.safeIf(x, y,  leftLocation-1, downLocation-1, pieceColor)) {
					activateAttackSquare(leftLocation-1, downLocation-1);
//					print ("time to attack");
				}
				
				int numDownRightMoves = 0;
				downLocation = y;
				rightLocation = x;
				while (numDownRightMoves < 8 && downLocation > 0 && rightLocation < 7 && tileMover.pieceOnMoveSquare(rightLocation+1, downLocation-1) == false) {
					diagMoveSquaresDownRight[numDownRightMoves] = boardTiles.tiles[rightLocation+1, downLocation-1];
					if (myKing.safeIf(x, y,  rightLocation+1, downLocation-1, pieceColor)) {
						(diagMoveSquaresDownRight[numDownRightMoves].GetComponent("Halo") as Behaviour).enabled = true;	
					}
					downLocation--;
					rightLocation++;
					numDownRightMoves++;
				}
				if (myKing.safeIf(x, y,  rightLocation+1, downLocation-1, pieceColor)) {
					activateAttackSquare(rightLocation+1, downLocation-1);
//					print ("time to attack");
				}

				if (gameObject.transform.tag == "White") {
					whitePieces.returnActivePiece(gameObject);
					
				} else if (gameObject.transform.tag == "Black") {
					blackPieces.returnActivePiece(gameObject);
				}
			}
			//turns off the piece's halo, if it is currently active
			else if (pieceActive) {
				deactivateBishop();
			}
		}
		if (currentSquare.transform.renderer.material.color == Color.red) {
			if (gameObject.transform.tag == "White") {
				whitePieces.takenPiece(gameObject, homePosX, homePosY);
			} else {
				blackPieces.takenPiece(gameObject, homePosX, homePosY);
			}
		}
	}

	public void deactivateSquare (GameObject square) {
		if (square != null) {
			(square.GetComponent("Halo") as Behaviour).enabled = false;
		}
	}
	
	public void testSquare (GameObject square) {
		if (square != null) {
			square.transform.renderer.material.color = Color.blue;
		}
	}
	
	public void deactivateBishop () {
		(gameObject.GetComponent("Halo") as Behaviour).enabled = false;
		pieceActive = false;
		for (int i = 0; i < 7; i++) {
			deactivateSquare(diagMoveSquaresUpLeft[i]);
			diagMoveSquaresUpLeft[i] = null;
			deactivateSquare (diagMoveSquaresUpRight[i]);
			diagMoveSquaresUpRight[i] = null;
			deactivateSquare(diagMoveSquaresDownLeft[i]);
			diagMoveSquaresDownLeft[i] = null;
			deactivateSquare(diagMoveSquaresDownRight[i]);
			diagMoveSquaresDownRight[i] = null;
			
			if (i < 4 && attackSquares[i] != null) {
				attackSquares[i].transform.renderer.material.color = attackSquaresColor[i];
				attackSquares[i] = null;
				attackSquaresColor[i] = Color.grey;
			}
			numAttackSquare = 0;
		}
	}
	
	public bool pieceHasMoved () {
		if (gameObject.tag == "Black" && blackPieces.blackPieceHasMoved) {
			return true; 
		} else if (gameObject.tag == "White" && whitePieces.whitePieceHasMoved) {
			return true;
		} else {
			return false; 
		}
	}
	
	public void activateAttackSquare (int x, int y) {
		if (x > -1 && x < 8 && y> -1 && y < 8) {
			if (pieceColor == "Black"  && whitePieces.whitePieceOnSquare[x,y]) {
				attackSquaresColor[numAttackSquare] = boardTiles.tiles[x,y].transform.renderer.material.color;
				boardTiles.tiles[x,y].transform.renderer.material.color = Color.red;
				attackSquares[numAttackSquare] = boardTiles.tiles[x,y];
				numAttackSquare++;
			} else if (pieceColor == "White" && blackPieces.blackPieceOnSquare[x,y]) {
				attackSquaresColor[numAttackSquare] = boardTiles.tiles[x,y].transform.renderer.material.color;
				boardTiles.tiles[x,y].transform.renderer.material.color = Color.red;
				attackSquares[numAttackSquare] = boardTiles.tiles[x,y];
				numAttackSquare++;
			}
		}
	}
}