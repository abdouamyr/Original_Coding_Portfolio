using UnityEngine;
using System.Collections;

public class rookMovement : MonoBehaviour 
{
	//rook's status
	public bool pieceActive = false;
	public string pieceColor;
	public string pieceTag = "rook";
	
	//references to other scripts
	blackPieceTracker blackPieces;
	whitePieceTracker whitePieces;
	boardCreationScript boardTiles;
	alphaMovementScript tileMover;
	turnTracker turn;
	positionScript coordinates; 
	kingMovement myKing;

	//rook's location
	public GameObject currentSquare;
	public GameObject [] vertMoveSquaresUp;
	public GameObject [] vertMoveSquaresDown;
	public GameObject [] horMoveSquaresLeft;
	public GameObject [] horMoveSquaresRight;
	public GameObject [] attackSquares;
	public Color [] attackSquaresColor;
	public float homePosX;
	public float homePosY;
	public int maxMoveSquares = 8;
	public int numAttackSquare = 0;
	public int maxAttackSquares = 4;
	// Use this for initialization
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
		
		//saves the pawn's original position and accesses its current one
		homePosX = gameObject.transform.position.x;
		homePosY = gameObject.transform.position.y;
		coordinates = gameObject.GetComponent<positionScript>();
		
		//creates the arrays for moveSquares
		vertMoveSquaresUp = new GameObject[maxMoveSquares];
		vertMoveSquaresDown = new GameObject[maxMoveSquares];
		horMoveSquaresLeft = new GameObject[maxMoveSquares];
		horMoveSquaresRight = new GameObject[maxMoveSquares];
		attackSquares = new GameObject[maxAttackSquares];
		attackSquaresColor = new Color[maxAttackSquares];

		gameObject.GetComponent<positionScript>().pieceColor = pieceColor;
		gameObject.GetComponent<positionScript>().pieceTag = pieceTag;
	}

	void Start () 
	{
		tileMover.updateLocation(gameObject, pieceTag);
		coordinates.xHome = homePosX;
		coordinates.yHome = homePosY;
	}
	
	// Update is called once per frame
	void Update () 
	{
		tileMover.deactivateHalo(gameObject);
		tileMover.updateLocation(gameObject, pieceTag);
		if (tileMover.moveCommitted || coordinates.timeToDeactivate || gameObject.transform.tag == "Deactivate") {
			pieceActive = false; 
			coordinates.timeToDeactivate = false; 
			deactivateRook();
			gameObject.transform.tag = pieceColor;
		}
		
		//locates the rook
		for (int x = 0; x < 8; x++)
		{
			for (int y = 0; y < 8; y++)
			{
			
				if (boardTiles.tiles[x,y].transform.position.x == gameObject.transform.position.x
					&& boardTiles.tiles[x,y].transform.position.y == gameObject.transform.position.y)
				{
					//saves the current square that the rook occupies
					currentSquare = boardTiles.tiles [x,y];
				}
			}
		}

	}

	void OnMouseDown () {
		if ((turn.playerTurn == "Black" && gameObject.transform.tag == "Black") || (turn.playerTurn == "White" && gameObject.transform.tag == "White")) {
			//turns on the piece's halo, if it is currently inactive
			if (pieceActive == false) {
				(gameObject.GetComponent("Halo") as Behaviour).enabled = true;	
				pieceActive = true;
				
				for (int x = 0; x < 8; x++) {
					for (int y = 0; y < 8; y++) {
						//locates the pawn
						if (boardTiles.tiles[x,y] == currentSquare) {

							//creates variables to track the current moveSquare
							int numLeftMoves = 0;
							int leftLocation = x;
							//continues highlighting squares to the left until there's a piece on the next square
							while (numLeftMoves < 8 && leftLocation > 0 && tileMover.pieceOnMoveSquare(leftLocation-1, y) == false) {
								horMoveSquaresLeft[numLeftMoves] = boardTiles.tiles[leftLocation-1,y];
								if (myKing.safeIf(x, y,  leftLocation-1, y, pieceColor)) {
									(horMoveSquaresLeft[numLeftMoves].GetComponent("Halo") as Behaviour).enabled = true;	
								}
								leftLocation--;
								numLeftMoves++;
							}
							if (myKing.safeIf(x, y,  leftLocation-1, y, pieceColor)) {
								activateAttackSquare(leftLocation-1, y);
							}
			
							//continues highlighting square to the right until there's a piece
							int numRightMoves = 0;
							int rightLocation = x;
							while (numRightMoves < 8 && rightLocation < 7 && tileMover.pieceOnMoveSquare(rightLocation+1, y) == false) {
								horMoveSquaresRight[numRightMoves] = boardTiles.tiles[rightLocation+1,y];
								if (myKing.safeIf(x, y, rightLocation+1, y, pieceColor)) {
									(horMoveSquaresRight[numRightMoves].GetComponent("Halo") as Behaviour).enabled = true;	
								}
								rightLocation++;
								numRightMoves++;
							}
							if (myKing.safeIf(x, y,  rightLocation+1, y, pieceColor)) {
								activateAttackSquare(rightLocation+1, y);
							}

							int numUpMoves = 0;
							int upLocation = y;
							while (numUpMoves < 8 && upLocation < 7 && tileMover.pieceOnMoveSquare(x, upLocation+1) == false) {
								vertMoveSquaresUp[numUpMoves] = boardTiles.tiles[x, upLocation+1];
								if (myKing.safeIf(x, y,  x, upLocation+1, pieceColor)) {
									(vertMoveSquaresUp[numUpMoves].GetComponent("Halo") as Behaviour).enabled = true;
								}
								upLocation++;
								numUpMoves++;
							}
							if (myKing.safeIf(x, y,  x, upLocation+1, pieceColor)) {
								activateAttackSquare(x, upLocation+1);
							}

							int numDownMoves = 0;
							int downLocation = y;
							while (numDownMoves < 8 && downLocation > 0 && tileMover.pieceOnMoveSquare(x, downLocation-1) == false) {
								vertMoveSquaresDown[numDownMoves] = boardTiles.tiles[x, downLocation-1];
								if (myKing.safeIf(x, y,  x, downLocation-1, pieceColor)) {
									(vertMoveSquaresDown[numDownMoves].GetComponent("Halo") as Behaviour).enabled = true;	
								}
								downLocation--;
								numDownMoves++;
							}
							if (myKing.safeIf(x, y,  x, downLocation-1, pieceColor)) {
								activateAttackSquare(x, downLocation-1);
							}
						
						}
					}
				}
			
				if (gameObject.transform.tag == "White") {
					whitePieces.returnActivePiece(gameObject);
				} else if (gameObject.transform.tag == "Black") {
					blackPieces.returnActivePiece(gameObject);
				}
			}
			//turns off the piece's halo, if it is currently active
			else if (pieceActive) {
				deactivateRook();
			}
		} else if (tileMover.checkActivePiece(gameObject) == false && currentSquare.transform.renderer.material.color == Color.red) {
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

	public void deactivateRook () {
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
			if (pieceColor == "Black" && whitePieces.whitePieceOnSquare[x,y]) {
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

