using UnityEngine;
using System.Collections;

public class knightMovement : MonoBehaviour 
{
	blackPieceTracker blackPieces;
	whitePieceTracker whitePieces;
	boardCreationScript boardTiles;
	alphaMovementScript tileMover;
	turnTracker turn;
	positionScript coordinates; 
	kingMovement myKing;

	public float homePosX;
	public float homePosY;
	public bool pieceActive = false;
	
	public GameObject squareRightUp;
	public GameObject squareRightDown;
	public GameObject squareLeftUp;
	public GameObject squareLeftDown;
	public GameObject squareUpLeft;
	public GameObject squareUpRight;
	public GameObject squareDownLeft;
	public GameObject squareDownRight;
	public GameObject currentSquare;
	public Color[] squareColors;
	public GameObject[] squares;
	
	int maxMoveSquares = 8;
	public int moveNumber = 0;
	public string pieceColor;
	public string pieceTag = "knight";

	void Awake () { 
		pieceColor = gameObject.transform.tag;
		//turns off the piece's halo
		(gameObject.GetComponent("Halo") as Behaviour).enabled = false;
		if (pieceColor == "Black") {
			myKing = GameObject.Find("Black King").GetComponent<kingMovement>();
		} else if (pieceColor == "White") {
			myKing = GameObject.Find("White King").GetComponent<kingMovement>();
		}
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
		
		homePosX = gameObject.transform.position.x;
		homePosY = gameObject.transform.position.y;
		
		squareColors = new Color[maxMoveSquares];
		squares = new GameObject[maxMoveSquares];
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
		currentSquare = boardTiles.tiles[x,y];
		if (gameObject != tileMover.activePiece && pieceActive) {
			deactivateKnight();
		}
	}
	
	void OnMouseDown ()
	{
		int x = coordinates.xPos;
		int y = coordinates.yPos; 
		
		if (turn.playerTurn == gameObject.transform.tag) {
			//turns on the piece's halo, if it is currently inactive
			if (pieceActive == false)
			{
				(gameObject.GetComponent("Halo") as Behaviour).enabled = true;
				
				squareRightUp = assignSquare(x+2, y+1);
				squareRightDown = assignSquare(x+2, y-1);
				squareLeftUp = assignSquare(x-2, y+1);
				squareLeftDown = assignSquare(x-2, y-1);
				squareUpLeft = assignSquare(x-1, y+2);
				squareUpRight = assignSquare(x+1, y+2);
				squareDownLeft = assignSquare(x-1, y-2);
				squareDownRight = assignSquare(x+1, y-2);
				
				checkSquare(squareRightUp, x+2, y+1);
				checkSquare(squareRightDown, x+2, y-1);
				checkSquare(squareLeftUp, x-2, y+1);
				checkSquare(squareLeftDown, x-2, y-1);
				checkSquare(squareUpLeft, x-1, y+2);
				checkSquare(squareUpRight, x+1, y+2);
				checkSquare(squareDownLeft, x-1, y-2);
				checkSquare(squareDownRight, x+1, y-2);
				
				if (gameObject.transform.tag == "White") {
					whitePieces.returnActivePiece(gameObject);
					
				} else if (gameObject.transform.tag == "Black") {
					blackPieces.returnActivePiece(gameObject);
				}
				
				pieceActive = true;
			}
			
			//turns off the piece's halo, if it is currently active
			else if (pieceActive)
			{
				deactivateKnight();
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
	
	public void checkSquare (GameObject square, int x, int y) {
		if (x < 8 && x > -1 && y < 8 && y > -1 && square != null) {
			if (isPieceEnemy(x,y)) { 
				square.transform.renderer.material.color = Color.red; 
			} else if (isPieceAlly(x,y)) { } else if (myKing.safeIf(coordinates.xPos, coordinates.yPos, x, y, pieceColor)) {
				(square.GetComponent("Halo") as Behaviour).enabled = true; 
			}
		}
	}
	
	public bool isPieceEnemy (int x, int y) {
		if (gameObject.transform.tag == "Black" && whitePieces.whitePieceOnSquare[x,y]) {
			return true;
		} else if (gameObject.transform.tag == "White" && blackPieces.blackPieceOnSquare[x,y]) {
			return true;
		} else {
			return false;
		}
	}
	
	public bool isPieceAlly (int x, int y) {
		if (gameObject.transform.tag == "Black" && blackPieces.blackPieceOnSquare[x,y]) {
			return true;
		} else if (gameObject.transform.tag == "White" && whitePieces.whitePieceOnSquare[x,y]) {
			return true;
		} else {
			return false;
		}
	}
	
	public GameObject assignSquare (int x, int y) {
		if (x < 8 && x > -1 && y < 8 && y > -1 && boardTiles.tiles[x,y] != null) {
			squares[moveNumber] = boardTiles.tiles[x,y];
			squareColors[moveNumber] = boardTiles.tiles[x,y].transform.renderer.material.color;
			moveNumber++;
			return boardTiles.tiles[x,y];
		} else {
			return null;
		}
	}
	
	public void deactivateSquare (GameObject square, int i) {
		if (square != null) {
			(square.GetComponent("Halo") as Behaviour).enabled = false;
			square.transform.renderer.material.color = squareColors[i];
		} 
	}
	
	public void deactivateKnight () {
		moveNumber = 0;
		(gameObject.GetComponent("Halo") as Behaviour).enabled = false;
		for (int i = 0; i < maxMoveSquares; i++) {
			deactivateSquare(squares[i], i);
			squares[i] = null;
			squareColors[i] = Color.grey;
			squareRightUp = null;
			squareRightDown = null;
			squareLeftUp = null;
			squareLeftDown = null;
			squareUpLeft = null;
			squareUpRight = null;
			squareDownLeft = null;
			squareDownRight = null;
		}
		pieceActive = false;
		gameObject.transform.tag = pieceColor;
	}
}