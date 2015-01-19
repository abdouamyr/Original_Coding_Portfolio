
using UnityEngine;
using System.Collections;

public class queenMovement : MonoBehaviour 
{
	public static GameObject activeWhitePiece;
	public bool activePawn = false;

	blackPieceTracker blackPieces;
	whitePieceTracker whitePieces;
	boardCreationScript boardTiles;
	alphaMovementScript tileMover;
	turnTracker turn;
	positionScript coordinates; 
	kingMovement myKing;

	//for piece movement 
	public GameObject [] vertMoveSquaresUp;
	public GameObject [] vertMoveSquaresDown;
	public GameObject [] horMoveSquaresLeft;
	public GameObject [] horMoveSquaresRight;
	public GameObject [] diagMoveSquaresUpLeft;
	public GameObject [] diagMoveSquaresUpRight;
	public GameObject [] diagMoveSquaresDownLeft;
	public GameObject [] diagMoveSquaresDownRight;
	public GameObject [] attackSquares; 
	public GameObject currentSquare; 
	public Color [] attackSquaresColor;
	public string pieceColor;
	public int maxMoveSquares = 8; 
	public int numAttackSquare;
	public float homePosX;
	public float homePosY;
	public bool pieceActive = false;
	public string pieceTag = "queen";

	void Awake () {
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
		coordinates = gameObject.GetComponent<positionScript>();
		
		attackSquares = new GameObject[maxMoveSquares];
		vertMoveSquaresUp = new GameObject [maxMoveSquares];
		vertMoveSquaresDown = new GameObject[maxMoveSquares];
		horMoveSquaresLeft = new GameObject[maxMoveSquares];
		horMoveSquaresRight = new GameObject [maxMoveSquares];
		diagMoveSquaresUpLeft = new GameObject[maxMoveSquares];
		diagMoveSquaresUpRight = new GameObject[maxMoveSquares];
		diagMoveSquaresDownLeft = new GameObject[maxMoveSquares];
		diagMoveSquaresDownRight = new GameObject[maxMoveSquares];
		
		attackSquaresColor = new Color[maxMoveSquares];	

		homePosX = gameObject.transform.position.x;
		homePosY = gameObject.transform.position.y;
		gameObject.GetComponent<positionScript>().pieceColor = pieceColor;
		gameObject.GetComponent<positionScript>().pieceTag = pieceTag;
	}

	// Use this for initialization
	void Start () 
	{
		tileMover.updateLocation(gameObject, pieceTag);
	}
	
	// Update is called once per frame
	void Update () 
	{
		int x = coordinates.xPos;
		int y = coordinates.yPos;
		coordinates.xHome = homePosX;
		coordinates.yHome = homePosY;
		tileMover.deactivateHalo(gameObject);
		tileMover.updateLocation(gameObject, pieceTag);
		if (tileMover.moveCommitted || coordinates.timeToDeactivate || gameObject.transform.tag == "Deactivate") {
			pieceActive = false; 
			coordinates.timeToDeactivate = false; 
			deactivateQueen();
			gameObject.transform.tag = pieceColor;
		}

		currentSquare = boardTiles.tiles[x,y];

	}


	void OnMouseDown () {
		if ((turn.playerTurn == "Black" && gameObject.transform.tag == "Black") || (turn.playerTurn == "White" && gameObject.transform.tag == "White")) {
			//turns on the piece's halo, if it is currently inactive
			if (pieceActive == false) {
				(gameObject.GetComponent("Halo") as Behaviour).enabled = true;	
				pieceActive = true;
				int x = coordinates.xPos;
				int y = coordinates.yPos;

				//creates variables to track the current moveSquare
				int numLeftMoves = 0;
				int leftLocation = x;
				//continues highlighting squares to the left until there's a piece on the next square
				while (numLeftMoves < 8 && leftLocation > 0 && tileMover.pieceOnMoveSquare(leftLocation-1, y) == false) {
					horMoveSquaresLeft[numLeftMoves] = boardTiles.tiles[leftLocation-1,y];
					if (myKing.safeIf(x, y, leftLocation - 1, y, pieceColor)) {
						(horMoveSquaresLeft[numLeftMoves].GetComponent("Halo") as Behaviour).enabled = true;	
					}
					leftLocation--;
					numLeftMoves++;
				}
				if (myKing.safeIf(x, y, leftLocation - 1, y, pieceColor)) {
					activateAttackSquare(leftLocation-1, y);
				}
				
				//continues highlighting square to the right until there's a piece
				int numRightMoves = 0;
				int rightLocation = x;
				while (numRightMoves < 8 && rightLocation < 7 && tileMover.pieceOnMoveSquare(rightLocation+1, y) == false) {
					horMoveSquaresRight[numRightMoves] = boardTiles.tiles[rightLocation+1,y];
					if (myKing.safeIf(x, y, rightLocation + 1, y, pieceColor)) {
						(horMoveSquaresRight[numRightMoves].GetComponent("Halo") as Behaviour).enabled = true;
					}
					rightLocation++;
					numRightMoves++;
				}
				if (myKing.safeIf(x, y, rightLocation + 1, y, pieceColor)) {
					activateAttackSquare(rightLocation+1, y);
				}

				int numUpMoves = 0;
				int upLocation = y;
				while (numUpMoves < 8 && upLocation < 7 && tileMover.pieceOnMoveSquare(x, upLocation+1) == false) {
					vertMoveSquaresUp[numUpMoves] = boardTiles.tiles[x, upLocation+1];
					if (myKing.safeIf(x, y, x, upLocation+1, pieceColor)) {
						(vertMoveSquaresUp[numUpMoves].GetComponent("Halo") as Behaviour).enabled = true;	
					}
					upLocation++;
					numUpMoves++;
				}
				if (myKing.safeIf(x, y, x, upLocation + 1, pieceColor)) {
					activateAttackSquare(x, upLocation+1);
				}

				int numDownMoves = 0;
				int downLocation = y;
				while (numDownMoves < 8 && downLocation > 0 && tileMover.pieceOnMoveSquare(x, downLocation-1) == false) {
					vertMoveSquaresDown[numDownMoves] = boardTiles.tiles[x, downLocation-1];
					if (myKing.safeIf(x, y, x, downLocation-1, pieceColor)) {
						(vertMoveSquaresDown[numDownMoves].GetComponent("Halo") as Behaviour).enabled = true;	
					}
					downLocation--;
					numDownMoves++;
				}
				if (myKing.safeIf(x, y, x, downLocation - 1, pieceColor)) {
					activateAttackSquare(x, downLocation-1);
				}

				int numUpLeftMoves = 0;
				upLocation = y;
				leftLocation = x;
				while (numUpLeftMoves < 8 && upLocation < 7 && leftLocation > 0 && tileMover.pieceOnMoveSquare(leftLocation-1, upLocation+1) == false) {
					diagMoveSquaresUpLeft[numUpLeftMoves] = boardTiles.tiles[leftLocation-1, upLocation+1];
					if (myKing.safeIf(x, y, leftLocation - 1, upLocation + 1, pieceColor)) {
						(diagMoveSquaresUpLeft[numUpLeftMoves].GetComponent("Halo") as Behaviour).enabled = true;	
					}
					upLocation++;
					leftLocation--;
					numUpLeftMoves++;
				}
				if (myKing.safeIf(x, y, leftLocation - 1, upLocation + 1, pieceColor)) {
				activateAttackSquare(leftLocation-1, upLocation+1);
				}

				int numUpRightMoves = 0;
				upLocation = y;
				rightLocation = x;
				while (numUpRightMoves < 8 && upLocation < 7 && rightLocation < 7 && tileMover.pieceOnMoveSquare(rightLocation+1, upLocation+1) == false) {
					diagMoveSquaresUpRight[numUpRightMoves] = boardTiles.tiles[rightLocation+1, upLocation+1];
					if (myKing.safeIf(x, y, rightLocation + 1, upLocation + 1, pieceColor)) {
						(diagMoveSquaresUpRight[numUpRightMoves].GetComponent("Halo") as Behaviour).enabled = true;	
					}
					upLocation++;
					rightLocation++;
					numUpRightMoves++;
				}
				if (myKing.safeIf(x, y, rightLocation + 1, upLocation + 1, pieceColor)) {
					activateAttackSquare(rightLocation+1, upLocation+1);
				}

				int numDownLeftMoves = 0;
				downLocation = y;
				leftLocation = x;
				while (numDownLeftMoves < 8 && downLocation > 0 && leftLocation > 0 && tileMover.pieceOnMoveSquare(leftLocation-1, downLocation-1) == false) {
					diagMoveSquaresDownLeft[numDownLeftMoves] = boardTiles.tiles[leftLocation-1, downLocation-1];
					if (myKing.safeIf(x, y, leftLocation - 1, downLocation - 1, pieceColor)) {
						(diagMoveSquaresDownLeft[numDownLeftMoves].GetComponent("Halo") as Behaviour).enabled = true;
					}
					downLocation--;
					leftLocation--;
					numDownLeftMoves++;
				}
				if (myKing.safeIf(x, y, leftLocation - 1, downLocation - 1, pieceColor)) {
					activateAttackSquare(leftLocation-1, downLocation-1);
				}

				int numDownRightMoves = 0;
				downLocation = y;
				rightLocation = x;
				while (numDownRightMoves < 8 && downLocation > 0 && rightLocation < 7 && tileMover.pieceOnMoveSquare(rightLocation+1, downLocation-1) == false) {
					diagMoveSquaresDownRight[numDownRightMoves] = boardTiles.tiles[rightLocation+1, downLocation-1];
					if (myKing.safeIf(x, y, rightLocation + 1, downLocation - 1, pieceColor)) {
						(diagMoveSquaresDownRight[numDownRightMoves].GetComponent("Halo") as Behaviour).enabled = true;	
					}
					downLocation--;
					rightLocation++;
					numDownRightMoves++;
				}
				if (myKing.safeIf(x, y, leftLocation + 1, downLocation - 1, pieceColor)) {
					activateAttackSquare(rightLocation+1, downLocation-1);
				}

				if (gameObject.transform.tag == "White") {
					whitePieces.returnActivePiece(gameObject);
					
				} else if (gameObject.transform.tag == "Black") {
					blackPieces.returnActivePiece(gameObject);
				}
			}
			//turns off the piece's halo, if it is currently active
			else if (pieceActive) {
				deactivateQueen();
			}
		}
		else if (tileMover.checkActivePiece(gameObject) == false && currentSquare.transform.renderer.material.color == Color.red) {
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
			print("turned blue");
		}
	}
	
	public void deactivateQueen () {
		(gameObject.GetComponent("Halo") as Behaviour).enabled = false;
		pieceActive = false;
		for (int i = 0; i < 7; i++) {
			deactivateSquare(horMoveSquaresLeft[i]);
			horMoveSquaresLeft[i] = null; 
			deactivateSquare(horMoveSquaresRight[i]);
			horMoveSquaresRight[i] = null;
			deactivateSquare(vertMoveSquaresUp[i]);
			vertMoveSquaresUp[i] = null;
			deactivateSquare(vertMoveSquaresDown[i]);
			vertMoveSquaresDown[i] = null;
			deactivateSquare(diagMoveSquaresUpLeft[i]);
			diagMoveSquaresUpLeft[i] = null;
			deactivateSquare (diagMoveSquaresUpRight[i]);
			diagMoveSquaresUpRight[i] = null;
			deactivateSquare(diagMoveSquaresDownLeft[i]);
			diagMoveSquaresDownLeft[i] = null;
			deactivateSquare(diagMoveSquaresDownRight[i]);
			diagMoveSquaresDownRight[i] = null;

			if (attackSquares[i] != null) {
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
			if (gameObject.transform.tag == "Black" && whitePieces.whitePieceOnSquare[x,y]) {
				attackSquaresColor[numAttackSquare] = boardTiles.tiles[x,y].transform.renderer.material.color;
				boardTiles.tiles[x,y].transform.renderer.material.color = Color.red;
				attackSquares[numAttackSquare] = boardTiles.tiles[x,y];
				numAttackSquare++;
			} else if (gameObject.transform.tag == "White" && blackPieces.blackPieceOnSquare[x,y]) {
				attackSquaresColor[numAttackSquare] = boardTiles.tiles[x,y].transform.renderer.material.color;
				boardTiles.tiles[x,y].transform.renderer.material.color = Color.red;
				attackSquares[numAttackSquare] = boardTiles.tiles[x,y];
				numAttackSquare++;
			}
		}
	}
}