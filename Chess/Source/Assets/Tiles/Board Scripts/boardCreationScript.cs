using UnityEngine;
using System.Collections;

public class boardCreationScript : MonoBehaviour {

	//board creation
	public int boardWidth = 8;
	public int boardHeight = 8;
	public GameObject [,] tiles;
	public GameObject [,] tileHalos;
	public GameObject tileCube;
	public GameObject tileParent;
	public GameObject borderParent;
	public GameObject horBorder;
	public GameObject vertBorder;
	public Vector3 [,] tileLocations;
	public Color lightGrey;
	public Color [,] tileColors;
	positionScript pieceLocation;

	whitePieceTracker whitePieces;
	blackPieceTracker blackPieces;

	void Awake ()
	{
		//creates the array to reference tiles
		tiles = new GameObject[boardWidth, boardHeight];
		tileLocations = new Vector3[boardWidth,boardHeight];
		tileColors = new Color [boardWidth, boardHeight];
		GameObject border;
		//creates the board and stores each tile within the array
		for (int x = 0; x<boardWidth; x++)
		{

			border = (GameObject) Instantiate(vertBorder, new Vector3 (x * 3 + 1.5F, 10.5F, -1), Quaternion.identity);
			border.transform.parent = borderParent.transform;
			border = (GameObject) Instantiate(horBorder, new Vector3 (10.5F, x*3 + 1.5F, -1), Quaternion.identity);
			border.transform.parent = borderParent.transform;
			for (int y = 0; y < boardHeight; y++)
			{	
				tiles [x,y] = (GameObject) Instantiate(tileCube, new Vector3 (x *3, y * 3, 0), Quaternion.identity);
				tiles [x,y].transform.parent = tileParent.transform;
				(tiles [x,y].GetComponent("Halo") as Behaviour).enabled = false;
				tileLocations [x,y] = tiles[x,y].transform.position;		
   			    pieceLocation = tiles[x,y].GetComponent<positionScript>();
        		pieceLocation.xPos = x;
				pieceLocation.yPos = y;
				tileColors [x,y] = Color.white;
				tiles[x,y].tag = "No Piece";
    
			}
		}
		border = (GameObject) Instantiate(vertBorder, new Vector3 (-1.5F, 10.5F, -1), Quaternion.identity);
		border.transform.parent = borderParent.transform;
		border = (GameObject) Instantiate(horBorder, new Vector3 (10.5F, -1.5F, -1), Quaternion.identity);
		border.transform.parent = borderParent.transform;
		//turns the even squares black
		for (int x = 1; x<boardWidth; x+=2)
		{
			for (int y = 1; y < boardHeight; y+=2)
			{	
				tiles [x,y].transform.renderer.material.color = Color.black;
				tileColors [x,y] = Color.black;
			}
		}

		//turns the odd squares black
		for (int x = 0; x<boardWidth; x+=2)
		{
			for (int y = 0; y < boardHeight; y+=2)
			{	
				tiles [x,y].transform.renderer.material.color = Color.black;
				tileColors [x,y] = Color.black;
			}
		}
	}
	// Use this for initialization
	void Start () 
	{
		//accesses the location of white pieces
		whitePieces = GameObject.Find("White Pieces").GetComponent<whitePieceTracker>();

		//accesses the location of black pieces
		blackPieces = GameObject.Find("Black Pieces").GetComponent<blackPieceTracker>();

		lightGrey = new Color (0.7F, 0.7F, 0.7F);
	}

	// Update is called once per frame
	void Update () 
	{
//		for (int x = 0; x<boardWidth; x++)
//		{
//			for (int y = 0; y < boardHeight; y++)
//			{	
//				if(blackPieces.blackPieceOnSquare[x,y] == false && whitePieces.whitePieceOnSquare[x,y] == false)
//				{
//					tiles [x,y].transform.renderer.material.color = Color.white;
//				}
//			}
//		}
//
//		//turns the even squares black
//		for (int x = 1; x<boardWidth; x+=2)
//		{
//			for (int y = 1; y < boardHeight; y+=2)
//			{
//				if(blackPieces.blackPieceOnSquare[x,y] == false && whitePieces.whitePieceOnSquare[x,y] == false)
//				{
//					tiles [x,y].transform.renderer.material.color = Color.black;
//				}
//			}
//		}
//
//		//turns the odd squares black
//		for (int x = 0; x<boardWidth; x+=2)
//		{
//			for (int y = 0; y < boardHeight; y+=2)
//			{	
//				if(blackPieces.blackPieceOnSquare[x,y] == false && whitePieces.whitePieceOnSquare[x,y] == false)
//				{
//					tiles [x,y].transform.renderer.material.color = Color.black;
//				}
//			}
//		}

		for (int x = 0; x < boardWidth; x++) {
			for (int y = 0; y < boardHeight; y ++) { 
				if ((tiles [x,y].GetComponent("Halo") as Behaviour).enabled == true && blackPieces.blackPieceOnSquare[x,y] == false && whitePieces.whitePieceOnSquare[x,y] == false) {
					tiles[x,y].renderer.material.color = Color.green; 
				} else if ((tiles [x,y].GetComponent("Halo") as Behaviour).enabled == false && tiles[x,y].transform.renderer.material.color != Color.red) {
					tiles[x,y].transform.renderer.material.color = tileColors [x,y];
				}
			}
		}
		if (whitePieces.whitePieceHasMoved || blackPieces.blackPieceHasMoved) {
			for (int x = 0; x<boardWidth; x++) {
				for (int y = 0; y < boardHeight; y++) {	
					(tiles [x,y].GetComponent("Halo") as Behaviour).enabled = false;	
				}
			}
			whitePieces.whitePieceHasMoved = false;
			blackPieces.blackPieceHasMoved = false;
		}

//		for (int x = 0; x<boardWidth; x++) {
//			for (int y = 0; y < boardHeight; y++) {	
//				if (blackPieces.blackPieceOnSquare[x,y]) {
//					tiles[x,y].transform.tag = "Black Piece";
//				} else if (whitePieces.whitePieceOnSquare[x,y]) {
//					tiles[x,y].transform.tag = "White Piece";
//				} else {
//					tiles[x,y].transform.tag = "No Piece"; 
//				}
//				
//			}
//		}

	}
}
