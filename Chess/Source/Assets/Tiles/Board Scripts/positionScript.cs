using UnityEngine;
using System.Collections;

public class positionScript : MonoBehaviour {
	public int xPos = -1;
	public int yPos = -1; 
	public float yHome;
	public float xHome;
	public bool timeToDeactivate; 
	public bool createdFromPawn = false;
	public string pieceTag;
	public string pieceColor;
	public bool[,]allies;
	public bool[,]enemies;
	kingMovement myKing;
	alphaMovementScript tileMover;
	boardCreationScript boardTiles;
	// Use this for initialization
	void Start () {
		if(pieceColor == "White") {
			myKing = GameObject.Find ("White King").GetComponent<kingMovement>();
		} else if(pieceColor == "Black") {
			myKing = GameObject.Find ("Black King").GetComponent<kingMovement>();
		}
		tileMover = GameObject.Find("BoardCreator").GetComponent<alphaMovementScript>();
		boardTiles = GameObject.Find("BoardCreator").GetComponent<boardCreationScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if (pieceColor == "White") {
			allies = GameObject.Find("White Pieces").GetComponent<whitePieceTracker>().whitePieceOnSquare;
			enemies = GameObject.Find("Black Pieces").GetComponent<blackPieceTracker>().blackPieceOnSquare;
		} else if (pieceColor == "Black") {
			enemies = GameObject.Find("White Pieces").GetComponent<whitePieceTracker>().whitePieceOnSquare;
			allies = GameObject.Find("Black Pieces").GetComponent<blackPieceTracker>().blackPieceOnSquare;
		}
	}
	public void resetPosition () {
		gameObject.transform.position = new Vector3 (xHome, yHome, 0);
	}
	public bool validMoves () {
		string pieceType = pieceTag;
		bool ableToMove = false;
		switch (pieceType){
		case "pawn":
			ableToMove = pawnCheck();
			break;
		case "king":
			ableToMove = kingCheck();
			break;
		case "queen":
			ableToMove = queenCheck();
			break;
		case "knight":
			ableToMove = knightCheck();
			break;
		case "rook":
			ableToMove = rookCheck();
			break;
		case "bishop":
			ableToMove = bishopCheck();
			break;
		}
		return ableToMove;
	}
	bool pawnCheck () {
		bool canMove = false;
		int direction = 1;
		int homeX = 1;
		if (pieceColor == "Black") {
			direction = -1;
			homeX = 6;
		} 
		//check forward
		if (inBounds(xPos + direction) && !allies[xPos + direction, yPos] && !enemies[xPos + direction, yPos] && myKing.safeIf(xPos, yPos, xPos + direction, yPos, pieceColor)) {
			canMove = true;
		} else if (xPos == homeX && 
		           !allies[xPos + direction, yPos] && !enemies[xPos + direction, yPos] &&
		           !allies[xPos + 2 * direction, yPos] && !enemies[xPos + 2 * direction, yPos] &&
		           myKing.safeIf(xPos, yPos, xPos + 2 * direction, yPos, pieceColor)) { //check home square move
			canMove = true;
		} else if (inBounds (yPos + direction) && enemies[xPos, yPos + direction] && myKing.safeIf(xPos, yPos, xPos + direction, yPos, pieceColor)) { //check take left
			canMove = true;
		} else if (inBounds (yPos - direction) && enemies[xPos, yPos - direction] && myKing.safeIf(xPos, yPos, xPos + direction, yPos, pieceColor)) { //check take right
			canMove = true;
		}
		return canMove;
	}
	bool kingCheck () {
		myKing.kingMovingCheck = true;
		if (moveCheck(xPos, yPos, xPos + 1, yPos + 1) ||
		    moveCheck(xPos, yPos, xPos + 1, yPos - 1) ||
		    moveCheck(xPos, yPos, xPos - 1, yPos + 1) ||
		    moveCheck(xPos, yPos, xPos - 1, yPos - 1) ||
		    moveCheck(xPos, yPos, xPos + 1, yPos) ||
		    moveCheck(xPos, yPos, xPos - 1, yPos) ||
		    moveCheck(xPos, yPos, xPos, yPos + 1) ||
		    moveCheck(xPos, yPos, xPos, yPos - 1)) {
			myKing.kingMovingCheck = false;
			return true;
		} else {
			myKing.kingMovingCheck = false;
			return false;
		}
	}
	bool queenCheck () {
		bool canMove = false;
		int x = xPos;
		int y = yPos;
		
		//creates variables to track the current moveSquare
		int numLeftMoves = 0;
		int leftLocation = x;
		//continues highlighting squares to the left until there's a piece on the next square
		while (numLeftMoves < 8 && leftLocation > 0 && tileMover.pieceOnMoveSquare(leftLocation-1, y) == false) {
			if (myKing.safeIf(x, y, leftLocation - 1, y, pieceColor)) {
				canMove = true;
			}	
			leftLocation--;
			numLeftMoves++;
		}
		if (myKing.safeIf(x, y, leftLocation - 1, y, pieceColor)) {
			canMove = true;
		}
		
		//continues highlighting square to the right until there's a piece
		int numRightMoves = 0;
		int rightLocation = x;
		while (numRightMoves < 8 && rightLocation < 7 && tileMover.pieceOnMoveSquare(rightLocation+1, y) == false) {
			if (myKing.safeIf(x, y, rightLocation + 1, y, pieceColor)) {
				canMove = true;
			}
			rightLocation++;
			numRightMoves++;
		}
		if (myKing.safeIf(x, y, rightLocation + 1, y, pieceColor)) {
			canMove = true;
		}
		
		int numUpMoves = 0;
		int upLocation = y;
		while (numUpMoves < 8 && upLocation < 7 && tileMover.pieceOnMoveSquare(x, upLocation+1) == false) {
			if (myKing.safeIf(x, y, x, upLocation+1, pieceColor)) {
				canMove = true;			}
			upLocation++;
			numUpMoves++;
		}
		if (myKing.safeIf(x, y, x, upLocation + 1, pieceColor)) {
			canMove = true;
		}
		
		int numDownMoves = 0;
		int downLocation = y;
		while (numDownMoves < 8 && downLocation > 0 && tileMover.pieceOnMoveSquare(x, downLocation-1) == false) {
			if (myKing.safeIf(x, y, x, downLocation-1, pieceColor)) {
				canMove = true;			}
			downLocation--;
			numDownMoves++;
		}
		if (myKing.safeIf(x, y, x, downLocation - 1, pieceColor)) {
			canMove = true;
		}
		
		int numUpLeftMoves = 0;
		upLocation = y;
		leftLocation = x;
		while (numUpLeftMoves < 8 && upLocation < 7 && leftLocation > 0 && tileMover.pieceOnMoveSquare(leftLocation-1, upLocation+1) == false) {
			if (myKing.safeIf(x, y, leftLocation - 1, upLocation + 1, pieceColor)) {
				canMove = true;			
			}
			upLocation++;
			leftLocation--;
			numUpLeftMoves++;
		}
		if (myKing.safeIf(x, y, leftLocation - 1, upLocation + 1, pieceColor)) {
			canMove = true;
		}
		
		int numUpRightMoves = 0;
		upLocation = y;
		rightLocation = x;
		while (numUpRightMoves < 8 && upLocation < 7 && rightLocation < 7 && tileMover.pieceOnMoveSquare(rightLocation+1, upLocation+1) == false) {
			if (myKing.safeIf(x, y, rightLocation + 1, upLocation + 1, pieceColor)) {
				canMove = true;
			}
			upLocation++;
			rightLocation++;
			numUpRightMoves++;
		}
		if (myKing.safeIf(x, y, rightLocation + 1, upLocation + 1, pieceColor)) {
			canMove = true;
		}
		
		int numDownLeftMoves = 0;
		downLocation = y;
		leftLocation = x;
		while (numDownLeftMoves < 8 && downLocation > 0 && leftLocation > 0 && tileMover.pieceOnMoveSquare(leftLocation-1, downLocation-1) == false) {
			if (myKing.safeIf(x, y, leftLocation - 1, downLocation - 1, pieceColor)) {
				canMove = true;
			}
			downLocation--;
			leftLocation--;
			numDownLeftMoves++;
		}
		if (myKing.safeIf(x, y, leftLocation - 1, downLocation - 1, pieceColor)) {
			canMove = true;
		}
		
		int numDownRightMoves = 0;
		downLocation = y;
		rightLocation = x;
		while (numDownRightMoves < 8 && downLocation > 0 && rightLocation < 7 && tileMover.pieceOnMoveSquare(rightLocation+1, downLocation-1) == false) {
			if (myKing.safeIf(x, y, rightLocation + 1, downLocation - 1, pieceColor)) {
				canMove = true;
			}
			downLocation--;
			rightLocation++;
			numDownRightMoves++;
		}
		if (myKing.safeIf(x, y, leftLocation + 1, downLocation - 1, pieceColor)) {
			canMove = true;
		}
		return canMove;
	}
	bool knightCheck () {
		bool canMove = false;
		if (moveCheck(xPos, yPos, xPos + 1, yPos + 2) ||
		    moveCheck(xPos, yPos, xPos + 1, yPos - 2) ||
		    moveCheck(xPos, yPos, xPos - 1, yPos + 2) ||
		    moveCheck(xPos, yPos, xPos - 1, yPos - 2) ||
		    moveCheck(xPos, yPos, xPos + 2, yPos + 1) ||
		    moveCheck(xPos, yPos, xPos + 2, yPos - 1) ||
		    moveCheck(xPos, yPos, xPos - 2, yPos + 1) ||
		    moveCheck(xPos, yPos, xPos - 2, yPos - 1)) {
			canMove = true;
		}
		return canMove;
	}
	bool moveCheck (int x, int y, int xTar, int yTar) {
		if (inBounds(xTar) && inBounds(yTar)) {
			if (!allies[xTar,yTar] && !boardTiles.tiles[xTar, yTar].tag.Contains(myKing.pieceColor) && myKing.safeIf(x, y, xTar, yTar, pieceColor)) {
				if (myKing.kingMovingCheck) {
					print (boardTiles.tiles[xTar, yTar].tag);
					print ("current: " + xPos + "," + yPos + "target: " + xTar + "," + yTar);
				}
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}
	bool rookCheck () {
		bool canMove = false;
		int x = xPos;
		int y = yPos;
		
		//creates variables to track the current moveSquare
		int numLeftMoves = 0;
		int leftLocation = x;
		//continues highlighting squares to the left until there's a piece on the next square
		while (numLeftMoves < 8 && leftLocation > 0 && tileMover.pieceOnMoveSquare(leftLocation-1, y) == false) {
			if (myKing.safeIf(x, y, leftLocation - 1, y, pieceColor)) {
				canMove = true;
			}	
			leftLocation--;
			numLeftMoves++;
		}
		if (myKing.safeIf(x, y, leftLocation - 1, y, pieceColor)) {
			canMove = true;
		}
		
		//continues highlighting square to the right until there's a piece
		int numRightMoves = 0;
		int rightLocation = x;
		while (numRightMoves < 8 && rightLocation < 7 && tileMover.pieceOnMoveSquare(rightLocation+1, y) == false) {
			if (myKing.safeIf(x, y, rightLocation + 1, y, pieceColor)) {
				canMove = true;
			}
			rightLocation++;
			numRightMoves++;
		}
		if (myKing.safeIf(x, y, rightLocation + 1, y, pieceColor)) {
			canMove = true;
		}
		
		int numUpMoves = 0;
		int upLocation = y;
		while (numUpMoves < 8 && upLocation < 7 && tileMover.pieceOnMoveSquare(x, upLocation+1) == false) {
			if (myKing.safeIf(x, y, x, upLocation+1, pieceColor)) {
				canMove = true;			}
			upLocation++;
			numUpMoves++;
		}
		if (myKing.safeIf(x, y, x, upLocation + 1, pieceColor)) {
			canMove = true;
		}
		
		int numDownMoves = 0;
		int downLocation = y;
		while (numDownMoves < 8 && downLocation > 0 && tileMover.pieceOnMoveSquare(x, downLocation-1) == false) {
			if (myKing.safeIf(x, y, x, downLocation-1, pieceColor)) {
				canMove = true;			}
			downLocation--;
			numDownMoves++;
		}
		if (myKing.safeIf(x, y, x, downLocation - 1, pieceColor)) {
			canMove = true;
		}
		return canMove;
	}
	bool bishopCheck () {
		bool canMove = false;
		int x = xPos;
		int y = yPos;
		int upLocation;
		int downLocation;
		int leftLocation;
		int rightLocation;
		int numUpLeftMoves = 0;
		upLocation = y;
		leftLocation = x;
		while (numUpLeftMoves < 8 && upLocation < 7 && leftLocation > 0 && tileMover.pieceOnMoveSquare(leftLocation-1, upLocation+1) == false) {
			if (myKing.safeIf(x, y, leftLocation - 1, upLocation + 1, pieceColor)) {
				canMove = true;			
			}
			upLocation++;
			leftLocation--;
			numUpLeftMoves++;
		}
		if (myKing.safeIf(x, y, leftLocation - 1, upLocation + 1, pieceColor)) {
			canMove = true;
		}
		
		int numUpRightMoves = 0;
		upLocation = y;
		rightLocation = x;
		while (numUpRightMoves < 8 && upLocation < 7 && rightLocation < 7 && tileMover.pieceOnMoveSquare(rightLocation+1, upLocation+1) == false) {
			if (myKing.safeIf(x, y, rightLocation + 1, upLocation + 1, pieceColor)) {
				canMove = true;
			}
			upLocation++;
			rightLocation++;
			numUpRightMoves++;
		}
		if (myKing.safeIf(x, y, rightLocation + 1, upLocation + 1, pieceColor)) {
			canMove = true;
		}
		
		int numDownLeftMoves = 0;
		downLocation = y;
		leftLocation = x;
		while (numDownLeftMoves < 8 && downLocation > 0 && leftLocation > 0 && tileMover.pieceOnMoveSquare(leftLocation-1, downLocation-1) == false) {
			if (myKing.safeIf(x, y, leftLocation - 1, downLocation - 1, pieceColor)) {
				canMove = true;
			}
			downLocation--;
			leftLocation--;
			numDownLeftMoves++;
		}
		if (myKing.safeIf(x, y, leftLocation - 1, downLocation - 1, pieceColor)) {
			canMove = true;
		}
		
		int numDownRightMoves = 0;
		downLocation = y;
		rightLocation = x;
		while (numDownRightMoves < 8 && downLocation > 0 && rightLocation < 7 && tileMover.pieceOnMoveSquare(rightLocation+1, downLocation-1) == false) {
			if (myKing.safeIf(x, y, rightLocation + 1, downLocation - 1, pieceColor)) {
				canMove = true;
			}
			downLocation--;
			rightLocation++;
			numDownRightMoves++;
		}
		if (myKing.safeIf(x, y, leftLocation + 1, downLocation - 1, pieceColor)) {
			canMove = true;
		}
		return canMove;
	}
	bool inBounds (int i) {
		if (i <= 7 && i >= 0) {
			return true;
		} else {
			return false;
		}
	}
}
