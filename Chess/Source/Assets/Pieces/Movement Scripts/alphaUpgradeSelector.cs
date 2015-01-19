using UnityEngine;
using System.Collections;

public class alphaUpgradeSelector : MonoBehaviour {
	public GameObject upgradePieceType;
	public GameObject upgradePiece; 

	public GameObject whitePieceUpgrader;
	public GameObject blackPieceUpgrader;

	public bool beginBlackUpgrade;
	public bool beginWhiteUpgrade;

	public bool chooseWhitePiece;
	public bool chooseBlackPiece;

	public bool endBlackUpgrade;
	public bool endWhiteUpgrade;

	public GameObject whiteRook;
	public GameObject whiteKnight;
	public GameObject whiteBishop;
	public GameObject whiteQueen;

	public GameObject blackRook;
	public GameObject blackKnight;
	public GameObject blackBishop;
	public GameObject blackQueen;

	public GameObject whiteParent;
	public GameObject blackParent;

	public GameObject pawn;
	public GameObject newPiece;
	// Use this for initialization
	void Start () {
		//hides upgrade panels from view
		whitePieceUpgrader.transform.position += Vector3.back*30;
		blackPieceUpgrader.transform.position += Vector3.back*30;
	}
	
	// Update is called once per frame
	void Update () {
		//moves panel locations if necessary
		updatePanelLocations();


	}

	public void selectType (GameObject pieceType) {
		upgradePieceType = pieceType;
		returnUpgradePiece();
		newPiece = (GameObject) Instantiate(upgradePiece, pawn.transform.position, pawn.transform.rotation);
		if (pawn.transform.tag == "Black") {
			newPiece.transform.parent = blackParent.transform;
			newPiece.transform.tag = "Black";
		} else if (pawn.transform.tag == "White") {
			newPiece.transform.parent = whiteParent.transform;
			newPiece.transform.tag = "White";
		}
		newPiece = null;
		Destroy(pawn);
	}

	public void updatePanelLocations () {
		if (beginWhiteUpgrade){
			whitePieceUpgrader.transform.position += Vector3.forward*30;
			chooseWhitePiece = true;
			beginWhiteUpgrade = false;
		} else if (beginBlackUpgrade) {
			blackPieceUpgrader.transform.position += Vector3.forward*30;
			chooseBlackPiece = true;
			beginBlackUpgrade = false;
		}
		
		if (endWhiteUpgrade){
			whitePieceUpgrader.transform.position += Vector3.back*30;
			endWhiteUpgrade = false;
		} else if (endBlackUpgrade) {
			blackPieceUpgrader.transform.position += Vector3.back*30;
			endBlackUpgrade = false;
		}
	}

	public void returnUpgradePiece () {
		if (chooseWhitePiece) {
			if (upgradePieceType.transform.tag == "rook") {
				upgradePiece = whiteRook;
			} else if (upgradePieceType.transform.tag == "bishop") {
				upgradePiece = whiteBishop;
			} else if (upgradePieceType.transform.tag == "queen") {
				upgradePiece = whiteQueen;
			} else if (upgradePieceType.transform.tag == "knight") {
				upgradePiece = whiteKnight;
			}
			chooseWhitePiece = false;
			endWhiteUpgrade = true;
		} else if (chooseBlackPiece) {
			if (upgradePieceType.transform.tag == "rook") {
				upgradePiece = blackRook;
			} else if (upgradePieceType.transform.tag == "bishop") {
				upgradePiece = blackBishop;
			} else if (upgradePieceType.transform.tag == "queen") {
				upgradePiece = blackQueen;
			} else if (upgradePieceType.transform.tag == "knight") {
				upgradePiece = blackKnight;
			}
			chooseBlackPiece = false;
			endBlackUpgrade = true;
		}
	}
}
