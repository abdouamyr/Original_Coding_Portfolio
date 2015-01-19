using UnityEngine;
using System.Collections;

public class turnTracker : MonoBehaviour 
{
	bool gameOver = false;
	bool checkForCheck = false;
	public string playerTurn;
	string currentTurn;
	string winner = "none";

	alphaMovementScript tileMover; 
	public Texture whiteTurn;
	public Texture whiteMate;
	public Texture blackTurn;
	public Texture blackMate;
	public Texture staleMate;
	public Texture playAgain;
	// Use this for initialization
	void Start () 
	{
		tileMover = GameObject.Find("BoardCreator").GetComponent<alphaMovementScript>();
		playerTurn = "White";	
		currentTurn = playerTurn;
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void LateUpdate () {
		if (checkForCheck) {
			checkForCheck = false;
			if (tileMover.whiteVictory()) {
				gameOver = true;
				if (tileMover.blackKingInCheck) {
					playerTurn = "WhiteMate";
				} else {
					playerTurn = "Stalemate";
				}
			}
			if (tileMover.blackVictory()) {
				gameOver = true;
				if (tileMover.whiteKingInCheck) {
					playerTurn = "BlackMate";
				} else {
					playerTurn = "Stalemate";
				}
			}
		}
		if (currentTurn != playerTurn) { 
			currentTurn = playerTurn;
			tileMover.moveCommitted = true;
			checkForCheck = true;
		}
	}

	void OnGUI ()
	{
		if (gameOver) {
			if (playerTurn == "BlackMate") {
				GUI.Box(new Rect(400, 550, 200, 40), blackMate);
			} else if (playerTurn == "WhiteMate") {
				GUI.Box(new Rect(400, 550, 200, 40), whiteMate);
			} else {
				GUI.Box(new Rect(400, 550, 200, 40), staleMate);
			}

			if (GUI.Button(new Rect(400, 500, 200, 40), playAgain)) {
				tileMover.newGame();
				gameOver = false;
				playerTurn = "White";
				currentTurn = playerTurn;
			}
		} else {
			if (playerTurn == "White") {
				GUI.Box(new Rect(400, 550, 200, 40), whiteTurn);
			} else if (playerTurn == "Black") {
				GUI.Box(new Rect(400, 550, 200, 40), blackTurn);
			}
		}
	}
}
