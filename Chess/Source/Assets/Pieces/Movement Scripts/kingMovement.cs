using UnityEngine;
using System.Collections;

public class kingMovement : MonoBehaviour 
{
	//script references 
	blackPieceTracker blackPieces;
	whitePieceTracker whitePieces;
	boardCreationScript boardTiles;
	alphaMovementScript tileMover;
	turnTracker turn;
	positionScript coordinates; 

	//keeps track of the piece's original location
	public float homePosX;
	public float homePosY;

	//tracks whether piece is active
	public bool pieceActive = false;

	//tracks squares
	public GameObject squareRight;
	public GameObject squareLeft;
	public GameObject squareUp;
	public GameObject squareDown;
	public GameObject squareUpLeft;
	public GameObject squareUpRight;
	public GameObject squareDownLeft;
	public GameObject squareDownRight;
	public Color[] squareColors;
	public GameObject[] squares;
	public Color[] checkSquareColors;
	public GameObject [] checkSquares;
	int maxMoveSquares = 8;

	//tracks pieceLocation
	public int xPos;
	public int yPos;

	//piece information
	public int moveNumber = 0;
	public int checkNumber = 0;
	public string pieceColor;
	public string pieceTag = "king";

	public bool kingMovingCheck;

	void Awake () { 
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

		//sets the original position
		homePosX = gameObject.transform.position.x;
		homePosY = gameObject.transform.position.y;

		//creates the arrays to track movesquares 
		squareColors = new Color[maxMoveSquares];
		squares = new GameObject[maxMoveSquares];
		checkSquareColors = new Color[maxMoveSquares + 1];
		checkSquares = new GameObject[maxMoveSquares + 1];

		//stores the pieces color
		pieceColor = gameObject.transform.tag;
		coordinates.pieceColor = pieceColor;
		coordinates.pieceTag = pieceTag;
		coordinates.xHome = homePosX;
		coordinates.yHome = homePosY;

	}

	// Use this for initialization
	void Start () 
	{
		//establishes piece's location
		tileMover.updateLocation(gameObject, pieceTag);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//updates piece's location
		tileMover.updateLocation(gameObject, pieceTag);
		//checks whether the king is in check
		if (tileMover.moveCommitted) {
			//sets check squares back to their original colors
			resetCheckSquares();
			//determines piece color and makes the appropriate checks 
			if (pieceColor == "Black") {
				if (kingInCheck(coordinates.xPos, coordinates.yPos, whitePieces.whitePieceOnSquare, blackPieces.blackPieceOnSquare)) {
					addCheckSquares();
					boardTiles.tiles[coordinates.xPos, coordinates.yPos].transform.renderer.material.color = Color.red;
					tileMover.blackKingInCheck = true;
				} else {
					tileMover.blackKingCheck = false;
				}
				tileMover.blackKingCheck = true;
			} else if (pieceColor == "White") {
				if (kingInCheck(coordinates.xPos, coordinates.yPos, blackPieces.blackPieceOnSquare, whitePieces.whitePieceOnSquare)) {
					addCheckSquares();
					boardTiles.tiles[coordinates.xPos, coordinates.yPos].transform.renderer.material.color = Color.red;
					tileMover.whiteKingInCheck = true;
				} else {
					tileMover.whiteKingInCheck = false;
				}
				tileMover.whiteKingCheck = true;
			}
			//deactivates king if it is not the active piece
			if (gameObject != tileMover.activePiece && pieceActive) {
				deactivateKing();
			}
			//updates the local coordinate values once the checks for king in check have been completed 
			xPos = coordinates.xPos;
			yPos = coordinates.yPos;
		}
	}

	void LateUpdate () {
		//checks whether the king is in check
//		if (tileMover.moveCommitted) {
//			//sets check squares back to their original colors
//			resetCheckSquares();
//			//determines piece color and makes the appropriate checks 
//			if (pieceColor == "Black") {
//				if (kingInCheck(coordinates.xPos, coordinates.yPos, whitePieces.whitePieceOnSquare, blackPieces.blackPieceOnSquare)) {
//					addCheckSquares();
//					boardTiles.tiles[coordinates.xPos, coordinates.yPos].transform.renderer.material.color = Color.red;
//					tileMover.blackKingInCheck = true;
//				} else {
//					tileMover.blackKingCheck = false;
//				}
//				tileMover.blackKingCheck = true;
//			} else if (pieceColor == "White") {
//				if (kingInCheck(coordinates.xPos, coordinates.yPos, blackPieces.blackPieceOnSquare, whitePieces.whitePieceOnSquare)) {
//					addCheckSquares();
//					boardTiles.tiles[coordinates.xPos, coordinates.yPos].transform.renderer.material.color = Color.red;
//					tileMover.whiteKingInCheck = true;
//				} else {
//					tileMover.whiteKingInCheck = false;
//				}
//				tileMover.whiteKingCheck = true;
//			}
//			//deactivates king if it is not the active piece
//			if (gameObject != tileMover.activePiece && pieceActive) {
//				deactivateKing();
//			}
//			//updates the local coordinate values once the checks for king in check have been completed 
//			xPos = coordinates.xPos;
//			yPos = coordinates.yPos;
//		}
	}

	void OnMouseDown ()
	{
		//updates piece location
		int x = coordinates.xPos;
		int y = coordinates.yPos; 

		//activates the piece if it is currently the player's turn
		if (turn.playerTurn == gameObject.transform.tag) {
			//turns on the piece's halo, if it is currently inactive
			if (pieceActive == false)
			{
				print ("turned on");
				(gameObject.GetComponent("Halo") as Behaviour).enabled = true;

				//assigns the movement squares
				squareRight = assignSquare(x+1, y);
				squareLeft = assignSquare(x-1, y);
				squareUp = assignSquare(x, y+1);
				squareDown = assignSquare(x-1, y);
				squareUpLeft = assignSquare(x-1, y+1);
				squareUpRight = assignSquare(x+1, y+1);
				squareDownLeft = assignSquare(x-1, y-1);
				squareDownRight = assignSquare(x+1, y-1);

				//checks squares for valid moves
				checkSquare(squareRight, x+1, y);
				checkSquare(squareLeft, x-1, y);
				checkSquare(squareUp, x, y+1);
				checkSquare(squareDown, x-1, y);
				checkSquare(squareUpLeft, x-1, y+1);
				checkSquare(squareUpRight, x+1, y+1);
				checkSquare(squareDownLeft, x-1, y-1);
				checkSquare(squareDownRight, x+1, y-1);


				//sets activte piece
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
				deactivateKing();
			}
		}
	}
	public bool safeIf (int xPos, int yPos, int xTar, int yTar, string color) {
		bool notSafe;
		bool pieceTaken = false;
		bool[,]enemies = null;
		bool[,]allies = null;
		int kingX = coordinates.xPos;
		int kingY = coordinates.yPos;
		if (kingMovingCheck) {
			kingX = xTar;
			kingY = yTar;
		}
		if (pieceColor == "Black") {
			enemies = whitePieces.whitePieceOnSquare;
			allies = blackPieces.blackPieceOnSquare;
		} else if (pieceColor == "White") {
			enemies = blackPieces.blackPieceOnSquare;
			allies = whitePieces.whitePieceOnSquare;
		}
		if (inBounds(xPos) && inBounds(yPos) && inBounds(xTar) && inBounds(yTar)) {
			string originSquare = boardTiles.tiles[xPos, yPos].tag;
			string targetSquare = boardTiles.tiles[xTar, yTar].tag;
			boardTiles.tiles[xPos,yPos].transform.tag = "No Piece";
			boardTiles.tiles[xTar,yTar].transform.tag = originSquare;
			allies[xPos, yPos] = false;
			allies[xTar, yTar] = true;
			if (enemies[xTar, yTar]) {
				enemies[xTar, yTar] = false;
				pieceTaken = true;
			}
			notSafe = kingInCheck(kingX, kingY, enemies, allies);
			boardTiles.tiles[xPos,yPos].transform.tag = originSquare;
			boardTiles.tiles[xTar,yTar].transform.tag = targetSquare;
			allies[xPos, yPos] = true;
			allies[xTar, yTar] = false;
			if (pieceTaken) {
				enemies[xTar, yTar] = true;
			}
			return !notSafe;
		} else {
			return false;
		}
	}

	public bool inBounds (int i) {
		if (i >= 0 && i <= 7) {
			return true;
		} else {
			return false;
		}
	} 
	//function to check if there is an enemy or an ally on the move square
	public void checkSquare (GameObject square, int x, int y) {
		if (x < 8 && x > -1 && y < 8 && y > -1 && square != null) {
			kingMovingCheck = true;
			if (isPieceEnemy(x,y)) { 

				if (safeIf (coordinates.xPos, coordinates.yPos, x, y, pieceColor)) {
					square.transform.renderer.material.color = Color.red; 
				}
			} else if (isPieceAlly(x,y)) { 
				//do nothing
			} else {
				if (safeIf (coordinates.xPos, coordinates.yPos, x, y, pieceColor)) {
					(square.GetComponent("Halo") as Behaviour).enabled = true; 
				}
			}
			kingMovingCheck = false;
		}
	}

	//function to identify if a piece on designated square is an enemy
	public bool isPieceEnemy (int x, int y) {
		if (gameObject.transform.tag == "Black" && whitePieces.whitePieceOnSquare[x,y]) {
			return true;
		} else if (gameObject.transform.tag == "White" && blackPieces.blackPieceOnSquare[x,y]) {
			return true;
		} else {
			return false;
		}
	}

	//identifies if piece on designated square is an ally
	public bool isPieceAlly (int x, int y) {
		if (gameObject.transform.tag == "Black" && blackPieces.blackPieceOnSquare[x,y]) {
			return true;
		} else if (gameObject.transform.tag == "White" && whitePieces.whitePieceOnSquare[x,y]) {
			return true;
		} else {
			return false;
		}
	}

	//sets move squares
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

	//deactivates move squares
	public void deactivateSquare (GameObject square, int i) {
		if (square != null) {
			(square.GetComponent("Halo") as Behaviour).enabled = false;
			square.transform.renderer.material.color = squareColors[i];
		} 
	}

	//deactivates the king and all its move squares
	public void deactivateKing () {
		moveNumber = 0;
		(gameObject.GetComponent("Halo") as Behaviour).enabled = false;
		for (int i = 0; i < maxMoveSquares; i++) {
			deactivateSquare(squares[i], i);
			squares[i] = null;
			squareColors[i] = Color.grey;
			squareRight = null;
			squareLeft = null;
			squareUp = null;
			squareDown = null;
			squareUpLeft = null;
			squareUpRight = null;
			squareDownLeft = null;
			squareDownRight = null;
		}
		pieceActive = false;
		gameObject.transform.tag = pieceColor;
	}

	//determines whether the king is in check
	public bool kingInCheck (int x, int y, bool[,] enemies, bool[,] allies) {
		if (checkDiagonals(x, y, enemies, allies) ||
		checkParallels(x, y, enemies, allies) ||
		checkforKnights(x, y, enemies)) {
			return true;
		} else {
			return false;
		}
	}

	//checks the diagonal lanes for adjacent enemy pieces
	public bool checkDiagonals (int x, int y, bool [,] enemies, bool [,] allies) {
		resetCoordinates(x,y);
		string checkRightUp = "None";
		string checkLeftUp = "None";
		string checkRightDown = "None";
		string checkLeftDown = "None";

		while (xPos < 7 && yPos < 7 && checkRightUp == "None") {
			checkRightUp = performLinearCheck(enemies, allies, "Right", "Up", x, y);
		}
		resetCoordinates(x,y);
		while (xPos > 0 && yPos < 7 && checkLeftUp == "None") {
			checkLeftUp = performLinearCheck(enemies, allies, "Left", "Up", x, y);
			//print (pieceColor + " " + checkLeftUp + " at: " + x + "," + y); 
		}
		resetCoordinates(x,y);
		while (xPos < 7 && yPos > 0 && checkRightDown == "None") {
			checkRightDown = performLinearCheck(enemies, allies, "Right", "Down", x, y);
		}
		resetCoordinates(x,y);
		while (xPos > 0 && yPos > 0 && checkLeftDown == "None") {
			checkLeftDown = performLinearCheck(enemies, allies, "Left", "Down", x, y);
		}
//		print (gameObject.transform.tag + " checkRightUp: " + checkRightUp);
//		print (gameObject.transform.tag + " checkLeftUp: " + checkLeftUp);
//		print (gameObject.transform.tag + " checkLeftDown: " + checkLeftDown);
//		print (gameObject.transform.tag + " checkRightDown: " + checkRightDown);
		if (checkRightUp == "Enemy" || checkRightDown == "Enemy" || checkLeftUp == "Enemy" || checkLeftDown == "Enemy") {
			return true;
		} else {
			return false;
		}
	}

	//checks up, down, left, and right for enemy pieces
	public bool checkParallels (int x, int y, bool [,] enemies, bool [,] allies) {
		resetCoordinates(x,y);
		string checkRight = "None";
		string checkLeft = "None";
		string checkUp = "None";
		string checkDown = "None";
		
		while (xPos < 7 && checkRight == "None") {
			checkRight = performLinearCheck(enemies, allies, "Right", "None", x, y);
		}
		resetCoordinates(x,y);
		while (xPos > 0 && checkLeft == "None") {
			checkLeft = performLinearCheck(enemies, allies, "Left", "None", x, y);
		}
		resetCoordinates(x,y);
		while (yPos < 7 && checkUp == "None") {
			checkUp = performLinearCheck(enemies, allies, "None", "Up", x, y);
		}
		resetCoordinates(x,y);
		while (yPos > 0 && checkDown == "None") {
			checkDown = performLinearCheck(enemies, allies, "None", "Down", x, y);
		}
//		print (gameObject.transform.tag + " checkRight: " + checkRight);
//		print (gameObject.transform.tag + " checkLeft: " + checkLeft);
//		print (gameObject.transform.tag + " checkUp: " + checkUp);
//		print (gameObject.transform.tag + " checkDown: " + checkDown);
		if (checkRight == "Enemy" || checkLeft == "Enemy" || checkUp == "Enemy" || checkDown == "Enemy") {
			return true;
		} else {
			return false;
		}
	}

	//checks for enemy knights
	public bool checkforKnights (int x, int y, bool [,] enemies) {
		if (checkCoordinates(x+2, y+1, enemies) || checkCoordinates(x+2, y-1, enemies) || checkCoordinates(x-2, y+1, enemies) || checkCoordinates(x-2, y-1, enemies) ||
		    checkCoordinates(x+1, y+2, enemies) || checkCoordinates(x-1, y+2, enemies) || checkCoordinates(x+1, y-2, enemies) || checkCoordinates(x-1, y-2, enemies)) {
			return true;
		}
		else {
			return false;
		}
	}

	//checks the designated square and returns what kind of piece is on it, if any
	public string performLinearCheck (bool [,] enemies, bool [,] allies, string xDir, string yDir, int x, int y) {
		int xMod = 0;
		int yMod = 0;
		string direction;
		int dirNum;
		if (xDir == "Left") {
			xMod = -1;
		} else if (xDir == "Right") {
			xMod = 1;
		} else if (xDir == "None") {
			xMod = 0;
		}
		if (yDir == "Down") {
			yMod = -1;
		} else if (yDir == "Up") {
			yMod = 1;
		} else if (yDir == "None") {
			yMod = 0;
		}
		dirNum = Mathf.Abs(xMod) + Mathf.Abs(yMod);
		if (dirNum == 1) { 
			direction = "Straight";
		} else if (dirNum == 2) { 
			direction = "Diagonal";
		} else {
			direction = "None";
		}
		changeCoordinates (xMod, yMod);
		if (enemies[xPos,yPos] && 
		    ((direction == "Straight" && checkValidParallelAttack(xPos, yPos, x, y, boardTiles.tiles[xPos, yPos], enemies))
		    || (direction == "Diagonal" && checkValidDiagonalAttack(xPos, yPos, x, y, boardTiles.tiles[xPos, yPos], enemies)))) { 
			return "Enemy";
		} else if (allies[xPos,yPos]) {
			//print ("xDir: " + xMod + " yDir: " + yMod);
			//print (direction + " " + pieceColor + "ally at: " + xPos + "," + yPos);
			return "Ally";
		} else {
			return "None";
		}

	}

	//checks if the piece on the attack square can check the king in a horizontal or vertical direction
	public bool checkValidParallelAttack (int xPos, int yPos, int x, int y, GameObject tile, bool[,] enemies) {
		if (enemies[xPos, yPos]) {
			if (tile.transform.tag.Contains("rook") || tile.transform.tag.Contains("queen")) {
//				print ("rook or queen");
				return true;
			}  else if (tile.transform.tag.Contains("king") && ((yPos - 1 == y || yPos +1 == y) && xPos == x) || ((xPos + 1 == x || xPos - 1 == x) && yPos == y)) { 
//				print ("king");	
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}

	//checks if the piece on the attack square can check the king in a diagonal direction
	public bool checkValidDiagonalAttack (int xPos, int yPos, int x, int y, GameObject tile, bool[,] enemies) {
		if (enemies[xPos, yPos]) {
			if (tile.transform.tag.Contains("queen") || tile.transform.tag.Contains("bishop")) {
//				print ("queen");
				return true;
			} else if (tile.transform.tag.Contains("pawn")) {
				if (pieceColor == "White" && enemies[xPos, yPos] && yPos - 1 == y && (xPos + 1 == x || xPos - 1 == x)) {
					print ("pawn: " + xPos + ", " + yPos + "|" + x + ", " + y);
					return true;
				} else if (pieceColor == "Black" && enemies[xPos, yPos] && yPos + 1 == y && (xPos + 1 == x || xPos - 1 == x)) {
					print ("pawn: " + xPos + ", " + yPos + "|" + x + ", " + y);
					return true;
				} else {
					return false;
				}
			} else if (tile.transform.tag.Contains("king") && ((yPos - 1 == y || yPos +1 == y) && (xPos + 1 == x || xPos - 1 == x))) {
				print ("king");	
				return true;
			} else {
					return false;
			}
		} else {
			return false;
		}
	}

	//resets the x and y pos coordinates in between checks 
	public void resetCoordinates (int x, int y) {
		xPos = x;
		yPos = y;
	}

	//increases the x and y coordinate for each check, based on the direction the check is performed in
	public void changeCoordinates (int xMod, int yMod) {
		xPos += xMod;
		yPos += yMod;
	}

	//checks coordinates for an enemy knight
	public bool checkCoordinates (int x, int y, bool [,] enemies) {
		if (x < 8 && x > 0 && y < 8 && y > 0) {
			if (enemies[x,y] && boardTiles.tiles[x,y].transform.tag == "knight") {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}

	//identifies the check squares
	public void addCheckSquares () { 
		checkSquareColors[checkNumber] = boardTiles.tiles[coordinates.xPos, coordinates.yPos].transform.renderer.material.color;
		checkSquares[checkNumber] = boardTiles.tiles[coordinates.xPos, coordinates.yPos];
		checkNumber++;
	}

	//resets check squares to their original color
	public void resetCheckSquares () {
		for (int i = 0; i < checkNumber; i++) {
			checkSquares[i].transform.renderer.material.color = checkSquareColors[i];
			checkSquares[i] = null;
			checkSquareColors[i] = Color.grey;
		}
		checkNumber = 0;
	}
}