using UnityEngine;
using System.Collections;

public class alphaMovementScript : MonoBehaviour {
	//move piece
	public GameObject activePiece;
	public GameObject blackTakenPieceParent;
	public GameObject whiteTakenPieceParent;
	public GameObject enPassantSquare;
	public GameObject enPassantPawn;
	public GameObject pieceToMove;
	public float enPassantHomePosX;
	public float enPassantHomePosY;
	public GameObject checkingSquare;
	public GameObject[,] miniMap = new GameObject[8,8];
	
	public bool moveCommitted = false;
	public bool blackKingCheck = false;
	public bool whiteKingCheck = false;
	public bool blackKingInCheck = false;
	public bool whiteKingInCheck = false;
	public bool pawnEnPassant = false; 

	//script references
	blackPieceTracker blackPieces;
	whitePieceTracker whitePieces;
	boardCreationScript coordinates;
	positionScript pieceLocator; 
	positionScript tileLocator;
	alphaUpgradeSelector upgradeControl;
	boardCreationScript boardTiles;

	void Awake () {
		//accesses the location of white pieces
		whitePieces = GameObject.Find("White Pieces").GetComponent<whitePieceTracker>();
		
		//accesses the locations of black pieces
		blackPieces = GameObject.Find("Black Pieces").GetComponent<blackPieceTracker>();
		
		upgradeControl = GameObject.Find("Alpha Upgrader").GetComponent<alphaUpgradeSelector>();

		boardTiles = GameObject.Find("BoardCreator").GetComponent<boardCreationScript>();
		
		//accesses the tile locations 
		coordinates = gameObject.GetComponent<boardCreationScript>();
	}

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{	
		if (activePiece != null && (activePiece.GetComponent("Halo") as Behaviour).enabled == false) {
			activePiece = null;
		}

		if (blackKingCheck && whiteKingCheck) {
			moveCommitted = false;
			blackKingCheck = false;
			whiteKingCheck = false;
		}
	}

	//method: detects if there is another piece on the movesquare
	public bool pieceOnMoveSquare (int x, int y)
	{
		if (x < 8 && y < 8) {
			if (blackPieces.blackPieceOnSquare[x, y] == true || whitePieces.whitePieceOnSquare [x, y] == true) {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}
	
	public void updateLocation (GameObject piece, string tileTag) {
		pieceLocator = piece.GetComponent<positionScript>();
		for (int x = 0; x < 8; x++) {
			for (int y = 0; y < 8; y++) {
				if (matchPosition(piece, coordinates.tiles[x,y])) {
					tileLocator = coordinates.tiles[x,y].GetComponent<positionScript>();
					boardTiles.tiles[pieceLocator.xPos, pieceLocator.yPos].transform.tag = "No Piece";
					pieceLocator.xPos = tileLocator.xPos;
					pieceLocator.yPos = tileLocator.yPos;
					boardTiles.tiles[pieceLocator.xPos, pieceLocator.yPos].transform.tag = pieceLocator.pieceColor + " " +tileTag;
				}
			}
		}
	}
					
	public bool matchPosition (GameObject piece, GameObject tile) {
		if (piece.transform.position.x == tile.transform.position.x && piece.transform.position.y == tile.transform.position.y) {
			return true; 
		} else {
			return false; 				
		}		
	}

	public bool checkActivePiece (GameObject piece) {
		if (activePiece == piece) {
			return true;
		} else {
			return false; 
		}
	}

	public void deactivateHalo (GameObject piece) {
		pieceLocator = piece.GetComponent<positionScript>();
		if (checkActivePiece(piece) == false) {
			(piece.GetComponent("Halo") as Behaviour).enabled = false;
			pieceLocator.timeToDeactivate = true; 
		} 
	}

	public void upgradePawn (GameObject pawn) {
		if (pawn.transform.tag == "White") {
			upgradeControl.beginWhiteUpgrade = true;
		} else if (pawn.transform.tag == "Black") {
			upgradeControl.beginBlackUpgrade = true;
		}
		upgradeControl.pawn = pawn;
	}

	public bool positionChanged (int pieceX, int pieceY, int tileX, int tileY) {
		if ((pieceX != tileX || pieceY != tileY) && pieceX != -1 && pieceY != -1) {
			return true;
		} else {
			return false;
		}
	}

	public void enableEnPassant (GameObject pawn, string color, int x, int y, float xHome, float yHome) {
		int xMod = 1;
		bool [,] enemies = whitePieces.whitePieceOnSquare;
		if (color == "black") { 
			xMod *= -1;
			enemies = blackPieces.blackPieceOnSquare;
		}
		if ((y > 0 && enemies[x, y-1]) || (y < 7 && enemies[x, y+1])) {
			enPassantPawn = pawn;
			enPassantHomePosX = xHome;
			enPassantHomePosY = yHome;
			enPassantSquare = boardTiles.tiles[x + xMod, y];
			(boardTiles.tiles[x + xMod, y].GetComponent("Halo") as Behaviour).enabled = true;
			boardTiles.tiles[x + xMod, y].transform.renderer.material.color = Color.red;
		}
	}

	public void executeEnPassant (string color) {
		if (color == "Black") {
			whitePieces.takenPiece(enPassantPawn, enPassantHomePosX, enPassantHomePosY);
		} else if (color == "White") {
			blackPieces.takenPiece(enPassantPawn, enPassantHomePosX, enPassantHomePosY);
		}
		(enPassantSquare.GetComponent("Halo") as Behaviour).enabled = false;
		enPassantPawn = null;
		enPassantSquare = null;
		enPassantHomePosY = 0;
		enPassantHomePosX = 0;

	}

	public bool kingSafe (string color) {
		if (color == "Black" && blackKingInCheck) {
			return false;
		} else if (color == "White" && whiteKingInCheck){
			return false;
		} else {
			return true;
		}
	}

	public bool whiteVictory() {
		bool victory = true;
		foreach (Transform child in GameObject.Find("Black Pieces").transform) {
			if (child.tag != "taken") {
				if (child.GetComponent<positionScript>().validMoves()) {
					victory = false;
					print (child.name);
				}
			}
		}
		return victory;
	}

	public bool blackVictory() {
		bool victory = true;
		foreach (Transform child in GameObject.Find("White Pieces").transform) {
			if (child.tag != "taken") {
				if (child.GetComponent<positionScript>().validMoves()) {
					victory = false;
					print (child.name);
				}
			}
		}
		return victory;
	}

	public void newGame() {
		positionScript currentKing = GameObject.Find ("Black King").GetComponent<positionScript>();
		if ((currentKing.xPos%2 == 0 && currentKing.yPos%2 == 0) || (currentKing.xPos%2 != 0 && currentKing.yPos%2 != 0)) {
			boardTiles.tiles[currentKing.xPos, currentKing.yPos].transform.renderer.material.color = Color.black;
		} else {
			boardTiles.tiles[currentKing.xPos, currentKing.yPos].transform.renderer.material.color = Color.white;
		}
		currentKing = GameObject.Find ("White King").GetComponent<positionScript>();
		if ((currentKing.xPos%2 == 0 && currentKing.yPos%2 == 0) || (currentKing.xPos%2 != 0 && currentKing.yPos%2 != 0)) {
			boardTiles.tiles[currentKing.xPos, currentKing.yPos].transform.renderer.material.color = Color.black;
		} else {
			boardTiles.tiles[currentKing.xPos, currentKing.yPos].transform.renderer.material.color = Color.white;
		}
		foreach (Transform child in GameObject.Find ("blackTakenPieceParent").transform) {
			child.transform.parent = GameObject.Find ("Black Pieces").transform;
		}
		foreach (Transform child in GameObject.Find ("whiteTakenPieceParent").transform) {
			child.transform.parent = GameObject.Find ("White Pieces").transform;
		}
		foreach (Transform child in GameObject.Find("White Pieces").transform) {
			if (child.tag != "taken") {
				child.GetComponent<positionScript>().resetPosition();
			}
		}
		foreach (Transform child in GameObject.Find("Black Pieces").transform) {
			if (child.tag != "taken") {
				child.GetComponent<positionScript>().resetPosition();
			}
		}
	}
}
