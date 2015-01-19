using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour
{
	//game parameters 
	public int gridWidth = 9;
	public int gridHeight = 9;
	public int wallsPerPlayer = 10;
	public int enemyTargetX;
	public string playerTurn = "Player 1's Turn";
	public string playerFeedback = "Select A Move :)";
	public string playerDesignator = "Player 1";
	public Color wallColor;
	public bool coRoutineFeedbackActive = false;
	public bool [,] connectedSquares;

	//pawn movement
	public int numInvalidHighlights = 0;
	public int pawn1GridXLocation = 0;
	public int pawn1GridYLocation = 4;
	public int pawn2GridXLocation = 8;
	public int pawn2GridYLocation = 4;
	public int enemyGridXLocation;
	public int enemyGridYLocation;
	public GameObject tileLeft;
	public GameObject tileRight;
	public GameObject tileUp;
	public GameObject tileDown;
	public bool modLeftActive = false;
	public bool modRightActive = false;
	public bool modUpActive = false;
	public bool modDownActive = false;
	public GameObject player1Pawn;
	public GameObject player2Pawn;
	public GameObject[] pawns;
	public GameObject activePawn = null;
	public GameObject[] invalidMoveSpaces = null;
	public GameObject tileCube;
	public GameObject[,] tiles;
	public GameObject tileParent;
	public bool pawnIsActive = false;
	public bool moveCommitted = false;
	public bool turnEnded;
	public float activePawnPosX = -1f;
	public float activePawnPosY = -1f;
	public float moveSpaceTarget;
	
	//wall building 
	public GameObject vertEmptyWallCube;
	public GameObject[,] vertEmptyWalls;
	public GameObject horEmptyWallCube;
	public GameObject[,] horEmptyWalls;
	public GameObject midWallSeg;
	public GameObject[,] midWallSegs;
	public GameObject player1Wall;
	public GameObject[] player1Walls;
	public GameObject player2Wall;
	public GameObject[] player2Walls;
	public GameObject activeStockWall;
	public GameObject vanishedWall = null;
	public GameObject wallSegment1 = null;
	public GameObject wallSegment2 = null;	
	public GameObject wallGrandParent;
	public GameObject horWallParent;
	public GameObject vertWallParent;
	public GameObject midWallParent;
	public GameObject p1WallParent;
	public GameObject p2WallParent;
	public GameObject currentCheckedWall;
	public bool stockWallIsActive = false;
	public bool oneWallSegmentClicked = false;
	public bool invalidCubesHighlighted = false;
	public bool wallOverlap = false;
	public bool coRoutineWallActive = false;
	public string wallRotation = null;
	public Color activePlayerColor;
	
	
	// Use this for initialization
	void Start () 
	{
		enemyTargetX = 0;
		activePlayerColor = Color.blue;
		connectedSquares = new bool[gridWidth,gridHeight];
		for (int x = 0; x < gridWidth; x++)
		{
			for (int y = 0; y < gridHeight; y++)
			{
				connectedSquares[x,y] = false;
			}
		}

		playerFeedback = "Select A Move :)";

		//sets the range of possible array of invalid spaces (red highlights) based on the size of the grid
		invalidMoveSpaces = new GameObject[gridWidth * gridHeight];

		//sets the dimensions of the grid of tiles 
		tiles = new GameObject[gridWidth,gridHeight];
		//creates the tiles 
		for (int x = 0; x < gridWidth; x++)
		{
			for (int y = 0; y < gridHeight; y++)
			{
				//places cubes into the array
				tiles [x, y] = (GameObject) Instantiate (tileCube, new Vector3 (x *2, y * 2, 0), Quaternion.identity);
				//sets the cubes to white
				tiles [x, y].renderer.material.color = Color.white;
				//turns off their halos 
				(tiles[x, y].GetComponent("Halo") as Behaviour).enabled = false;
				tiles [x, y].transform.parent = tileParent.transform;
			}						
		}

		//sets the dimensions of the horizontal wall segments
		horEmptyWalls = new GameObject[gridWidth,gridHeight-1];
		//creates the horizontal wall segments 
		for (int x = 0; x < gridWidth; x++)
		{
			for (int y = 0; y < (gridHeight-1); y++)
			{
				//places segments into the array
				horEmptyWalls [x, y] = (GameObject) Instantiate (horEmptyWallCube, new Vector3 (x * 2, (y + 0.5f) *2, 1), Quaternion.identity);
				//sets the segments to grey
				horEmptyWalls [x, y].renderer.material.color = Color.grey;
				//sets them to an empty game object parent
				horEmptyWalls [x, y].transform.parent = horWallParent.transform;
			}						
		}

		//sets the dimensions of the vertical wall segments
		vertEmptyWalls = new GameObject[gridWidth-1,gridHeight];
		
		//instantiates the vertical wall segemtns 
		for (int x = 0; x < (gridWidth-1); x++)
		{
			for (int y = 0; y < gridHeight; y++)
			{
				//places segments into the array
				vertEmptyWalls [x, y] = (GameObject) Instantiate (vertEmptyWallCube, new Vector3 ((x+0.5f) * 2, (y) *2, 1), Quaternion.identity);
				//sets the segements to grey
				vertEmptyWalls [x, y].renderer.material.color = Color.grey;
				//sets them to an empty game object parent
				vertEmptyWalls [x, y].transform.parent = vertWallParent.transform;
			}					
		}
	
		//sets the dimensions of the mid wall segments 
		midWallSegs = new GameObject[gridWidth-1,gridHeight];

		//instantiates the mid wall segments
		for (int x = 0; x < (gridWidth-1); x++)
		{
			for (int y = 0; y < gridHeight-1; y++)
			{
				//places segments into the array
				midWallSegs [x, y] = (GameObject) Instantiate (midWallSeg, new Vector3 ((x+0.5f)*2, (y+0.5f)*2, 1), Quaternion.identity);
				//sets the segements to grey
				midWallSegs [x, y].renderer.material.color = Color.grey;
				//sets them to an empty game object parent
				midWallSegs [x, y].transform.parent = midWallParent.transform;
			}					
		}

	
		//sets the range of the array of player 1's walls 
		player1Walls = new GameObject[wallsPerPlayer];

		//instaniates player 1's walls
		for (int x = 0; x < wallsPerPlayer; x++)
		{
			//places walls into the array
			player1Walls [x] = (GameObject) Instantiate (player1Wall, new Vector3 ((x-10) * 1.1f, (-3) *2, 1), Quaternion.identity);
			//sets the walls to bornw
			player1Walls [x].transform.parent = p1WallParent.transform;
			//turns "halo" off
			(player1Walls[x].GetComponent("Halo") as Behaviour).enabled = false;
		}				

		//sets the range of the array of player 2's walls 
		player2Walls = new GameObject[wallsPerPlayer];
		for (int x = 0; x < wallsPerPlayer; x++)
		{
			//places walls into the array
			player2Walls [x] = (GameObject) Instantiate (player2Wall, new Vector3 ((x+16) * 1.1f, (-3) *2, 1), Quaternion.identity);
			//sets the walls to brown
			player2Walls [x].transform.parent = p2WallParent.transform;
			//turns "halo" off
			(player2Walls[x].GetComponent("Halo") as Behaviour).enabled = false;
		}
		
		//places the wall subparent's into a "grand" parent				
		midWallParent.transform.parent = wallGrandParent.transform;
		vertWallParent.transform.parent = wallGrandParent.transform;
		horWallParent.transform.parent = wallGrandParent.transform;
		p1WallParent.transform.parent = wallGrandParent.transform;
		p2WallParent.transform.parent = wallGrandParent.transform;

		//sets the array of pawns
		pawns = new GameObject[2];
		//instaniates player 1's pawn
		pawns [0] = (GameObject) Instantiate (player1Pawn, new Vector3 (0, 8, -1), Quaternion.identity);
		//parents it to the "tileParent" GameObject
		pawns [0].transform.parent = tileParent.transform;
		//insantiates player 2's pawn
		pawns [1] = (GameObject) Instantiate (player2Pawn, new Vector3 (16, 8, -1), Quaternion.identity);
		//parents it to the "tileParent" GameObject
		pawns [1].transform.parent = tileParent.transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//sets the enemy location variables: for the invalid wall placement check
		setEnemyPlayerPosition ();

		//sets the active player color for wall highlights
		setActivePlayerColor ();

		//stops the CoRoutine from looping after it changes playerFeedback to default
		if (coRoutineFeedbackActive == true)
		{
			 StopCoroutine("changeBackPlayerFeedback");
		}

		//code testing cheat: switch turn when spacebar is pressed 
		if (Input.GetKeyDown(KeyCode.Space))
		{
			switch(playerTurn)
			{
				case "Player 1's Turn":
				playerTurn = "Player 2's Turn";
				activePawn = player2Pawn;
				pawnIsActive = false;
				break;
				
				case "Player 2's Turn":
				playerTurn = "Player 1's Turn";
				break;
				
				default:
				activePawn = player1Pawn;
				break;	
			}
		}

		//turns any mid segments brown if they have a wall segment on either side of them
		checkMidSegments();

		//switches the "playerTurn" variable, if a move has been committed 
		returnActivePlayer();

		//switches the "playerDesignator" variable, used in the feedback GUI box
		switchPlayerDesignator();
	}

	//displays player feedback at the bottom of the screen
	void OnGUI ()
	{
		//designates player and the currently given feedback
		GUI.Box(new Rect(310, 470, 300, 30), playerDesignator + " : " + playerFeedback);
	}

	//switches player turn, if a move has been comitted 
	public string returnActivePlayer () 
	{	
		//if player 2 has entered a move, turn changes to player 1's turn
		if (moveCommitted && playerTurn == "Player 2's Turn" && wallOverlap == false)
		{
			playerTurn = "Player 1's Turn";
			
			//resets the move variable
			moveCommitted = false;
			
			//triggers the end turn actions
			turnEnded = true;

			//resets the wall placement variables 
			wallOverlap = false;
			activeStockWall = null;
			wallSegment1 = null;
			wallSegment2 = null;
			enemyTargetX = 0;
		
			//resets the movement variables	
			pawnIsActive = false;
			deactivateMods();
			moveSpaceTarget = 16f;
		
			//resets player feedback 
			playerFeedback = "Select A Move :)";
			
			//completes the end turn phase
			turnEnded = false;
			return playerTurn;
		}

		//if player 1 has entered a move, turn changes to player 2's turn
		if (moveCommitted && playerTurn == "Player 1's Turn" && wallOverlap == false)
		{
			playerTurn = "Player 2's Turn";
			
			//resets the move variable
			moveCommitted = false;
			
			//triggers the end turn actions
			turnEnded = true;

			//resets the wall placement variables 
			wallOverlap = false;
			activeStockWall = null;
			wallSegment1 = null;
			wallSegment2 = null;
			enemyTargetX = 8;
		
			//resets the movement variables	
			pawnIsActive = false;
			deactivateMods();
			moveSpaceTarget = 0f;
		
			//resets player feedback 
			playerFeedback = "Select A Move :)";
			
			//completes the end turn phase
			turnEnded = false;
			return playerTurn;
		}
		
		//if no move has been committed, playerTurn remains unchanged 
		else
		{
			return playerTurn;
		}
	}

	//activates a wall from the playes stock, upon click
	public void activateClickedStockWall (GameObject clickedStockWall)
	{
		//if a stock wall is not active, activates stock wall
		if (stockWallIsActive == false)
		{
				
			//deactivates the pawn, if active 
			if (pawnIsActive == true)
			{
				pawnIsActive = false;
				activePawn.transform.localScale = new Vector3(1, 1, 1);
				hideValidMoves ();
				playerFeedback = "Select A Move :)";
			}

			//sets the clicked wall to the "activeStockWall" gameObject 
			activeStockWall = clickedStockWall;
			
			//turns on the wall's "halo"
			(activeStockWall.GetComponent("Halo") as Behaviour).enabled = true;

			//triggers the bool
			stockWallIsActive = true;
			
			//updates player feedback
			playerFeedback = "Click A Grey Wall to Place :D";
		}

		//deactivates the currently highlighted stock wall, on click
		else if (stockWallIsActive && clickedStockWall == activeStockWall)
		{
			activeStockWall.transform.localScale = new Vector3(0.5f, 3.0f, 1);
		 	(activeStockWall.GetComponent("Halo") as Behaviour).enabled = false;
			stockWallIsActive = false;
			
			//if the player has already selected on of the two wall segments 
			if (wallSegment1 != null)
			{
				//deactivates the wall segment and resets its color 
				oneWallSegmentClicked = false;
				wallSegment1.renderer.material.color = Color.grey;
			}
			
			//updates player feedback to neutral
			playerFeedback = "Select A Move :)";
		
		}
		
		//if a different stock wall is clicked, deactivates first and activates the other 
		else if (stockWallIsActive && clickedStockWall != activeStockWall)
		{
			//deactivates prior stock wall
			activeStockWall.transform.localScale = new Vector3(0.5F, 3.0f, 1);
			(activeStockWall.GetComponent("Halo") as Behaviour).enabled = false;

			//deactivates segment 1, if already designated 
			if (wallSegment1 != null)
			{
				oneWallSegmentClicked = false;
				wallSegment1.renderer.material.color = Color.grey;
			}
			//changes "activeStockWall" to the currently selected one
			activeStockWall = clickedStockWall;
			
			//activates the new stock wall's "halo"
			(activeStockWall.GetComponent("Halo") as Behaviour).enabled = true;
		}
	}

	//checks wall placement after both segments have been selected
	public void checkWallPlacement ()
	{
		
		//places the wall, if the method was able to find a valid midsegment 
		if (checkConnections(enemyGridXLocation,enemyGridYLocation))
		{
			if (changeActiveMidSegment (wallSegment1, wallSegment2) != null)
			{
				wallSegment1.transform.renderer.material.color = wallColor;
			    activeStockWall.transform.renderer.enabled = false;
				(activeStockWall.GetComponent("Halo") as Behaviour).enabled = false;
				vanishedWall = activeStockWall;
				stockWallIsActive = false;
				oneWallSegmentClicked = false;
				moveCommitted = true;
			}

			else 
			{
				deactivateCurrentWalls();
			}
		}

		else 
		{
			deactivateCurrentWalls();
		}
	}

	public void deactivateCurrentWalls ()
	{
		oneWallSegmentClicked = false;
		activeStockWall.transform.localScale = new Vector3(0.5F, 3.0f, 1);
		wallSegment1.transform.renderer.material.color = Color.grey;
		wallSegment2.transform.renderer.material.color = Color.grey;
		wallSegment1 = null;
		wallSegment2 = null;
		playerFeedback = "Invalid Move :O";
		StartCoroutine(changeBackPlayerFeedback());
	}
	//places the first wall segment
	public void selectWallSegment1 (GameObject clickedWallSegment)
	{
		//if the clicked segment does not already contain a wall 
		if (clickedWallSegment.transform.renderer.material.color == Color.grey && oneWallSegmentClicked == false && stockWallIsActive)
		{
			//sets the clicked object to an assignable variable 
			wallSegment1 = clickedWallSegment;

			//designates the rotation of the wall, to be passed onto the "checkMidSegments()" method
			if (wallSegment1.tag == "Horizontal Wall")
			{
				wallRotation = "Horizontal";
			}

			else if (wallSegment1.tag == "Vertical Wall")
			{
				wallRotation = "Vertical";
			}
	
			//changes the color of the first wall
			wallSegment1.transform.renderer.material.color = wallColor;
			
			//the current selected Player Wall shrinks to show that it has been half placed
			activeStockWall.transform.localScale = new Vector3 (0.5f, 1.5f, 1.5f);

			//triggers the bool to activate the selectWallSegment2 script
			oneWallSegmentClicked = true;
		}
	}

	//places the second wall segment
	public void selectWallSegment2 (GameObject clickedWallSegment) 
	{
		checkMidSegments();
		//places the second wall segment, if the clicked segment is grey
		if (clickedWallSegment.transform.renderer.material.color == Color.grey 
			&& segmentIsAdjacent(clickedWallSegment.transform.position.x, clickedWallSegment.transform.position.y)
			&& wallOverlap == false) 
		{
			wallSegment2 = clickedWallSegment;
			wallSegment2.transform.renderer.material.color = wallColor;
			checkWallPlacement();
		}
	}

	//detects whether a second wall segment 
	public bool segmentIsAdjacent(float x, float y) 
	{
		//detects am adjacent segment to the left or right
		if ((wallSegment1.transform.position.x == x - 2f && wallSegment1.transform.position.y == y) || 
		     (wallSegment1.transform.position.x == x + 2f && wallSegment1.transform.position.y == y))
		{
			return true;
		}
		//detect an adjacent segment above or below 
		else if ((wallSegment1.transform.position.y == y - 2f && wallSegment1.transform.position.x == x) || 
		    (wallSegment1.transform.position.y == y +2f && wallSegment1.transform.position.x == x))
		{
			return true;
		}
		//returns false if no adajcent segment was found
		else
		{
			return false;
		}
	}

	//activates the pawn upon click 
	public void activatePawn (GameObject clickedPawn)
	{
		//sets the activePawnGameObject and position 
		activePawn = clickedPawn;
		activePawnPosX = clickedPawn.transform.position.x;
		activePawnPosY = clickedPawn.transform.position.y;
		
		//if the pawn is not already active, it activates 
		if (pawnIsActive == false)
		{
			//if a stock wall was activates, it deactivates 
			if (stockWallIsActive)
			{
				activeStockWall.transform.localScale = new Vector3(0.5F, 3.0f, 1);
				(activeStockWall.GetComponent("Halo") as Behaviour).enabled = false;
				stockWallIsActive = false;
				oneWallSegmentClicked = false;
				if (wallSegment1 != null)
				{
					wallSegment1.renderer.material.color = Color.grey;
				}
			}
			
			//pawn is active bool 
			pawnIsActive = true;

			//enlarges the pawn to show that it is active
			clickedPawn.transform.localScale = new Vector3(1.5F, 1.5f, 1.5f);
			
			//shows the valid moves with "halos" on the tile cubes
			switch (playerTurn)
			{
			case "Player 1's Turn":
				showValidMoves (pawn1GridXLocation, pawn1GridYLocation);
				wallBlocked (pawn1GridXLocation, pawn1GridYLocation);
				break;
		
			case "Player 2's Turn":
				showValidMoves (pawn2GridXLocation, pawn2GridYLocation);
				wallBlocked (pawn2GridXLocation, pawn2GridYLocation);
				break;

			default:
				break;
			}

			//changes player feedback 
			playerFeedback = "Click A Highlighted Square to Move :D";
		}
		
		
		//deactivates the pawn on click, if it is already on active 
		else if (pawnIsActive == true)
		{
			//resets the necessary settings
			pawnIsActive = false;
			clickedPawn.transform.localScale = new Vector3(1, 1, 1);
			hideValidMoves ();
			playerFeedback = "Select A Move :)";
		}
	}

	//highlights the moves to the right, left, above, and below of the pawn
	public void showValidMoves (int x, int y)
	{
		//modifiers to increase the move distance, if the other pawn is directly adjacent
		int modUp = 0;
		int modDown = 0;
		int modLeft = 0;
		int modRight = 0;
	
		//detects an adajcent pawn
		if (playerTurn == "Player 1's Turn")
		{
			switch (adjacentPawn (pawn1GridXLocation, pawn1GridYLocation, pawn2GridXLocation, pawn2GridYLocation)) 
			{
			case "Pawn Left":
				modLeft = -1;
				modLeftActive = true;
				break;
		
			case "Pawn Right":
				modRight = 1;
				modRightActive = true;
				break;
		
			case "Pawn Above":
				modUp = 1;
				modUpActive = true;
				break;
		
			case "Pawn Below":
				modDown = -1;
				modDownActive = true;
				break;
		
			default:
				break;
			}
		}

		if (playerTurn == "Player 2's Turn")
		{
			switch (adjacentPawn (pawn2GridXLocation, pawn2GridYLocation, pawn1GridXLocation, pawn1GridYLocation)) 
			{
			case "Pawn Left":
				modLeft = -1;
				modLeftActive = true;
				break;

			case "Pawn Right":
				modRight = 1;
				modRightActive = true;
				break;

			case "Pawn Above":
				modUp = 1;
				modUpActive = true;
				break;

			case "Pawn Below":
				modDown = -1;
				modDownActive = true;
				break;

			default:
				break;
			}
		}


		//highlights the cube to the right of the pawn
		if (x+1 + modRight < gridWidth)
		{
			(tiles[x+ 1 + modRight, y].GetComponent("Halo") as Behaviour).enabled = true;
			tileRight = tiles[x+ 1 + modRight, y];
		}

		//highlights the cube above the pawn
		if (y+1+modUp < gridHeight)
		{
			(tiles[x, y+ 1 + modUp].GetComponent("Halo") as Behaviour).enabled = true;
			tileUp = tiles [x, y + 1+modUp];
		}

		//highlights the cube below the pawn
		if (y-1+modDown	>= 0)
		{
			(tiles[x, y-1+modDown].GetComponent("Halo") as Behaviour).enabled = true;
			tileDown = tiles [x, y - 1+modDown];
		}

		//highlights the cube to the left of the pawn
		if (x-1+modLeft >= 0)
		{
			(tiles[x-1+modLeft, y].GetComponent("Halo") as Behaviour).enabled = true;
			tileLeft = tiles [x - 1+modLeft,y]; 
		}
	}
	
	//deactivates the modifier bools in between turns, so that they can be checked again
	public void deactivateMods ()
	{
		modUpActive = false;
		modLeftActive = false;
		modRightActive = false; 
		modDownActive = false;
	}

	//deactivates "halos" once the turn is complete 
	public void hideValidMoves ()
	{
		
		if (tileUp != null)
		{
			(tileUp.GetComponent("Halo") as Behaviour).enabled = false;
			tileUp = null;
		}
		if (tileDown != null)
		{
			(tileDown.GetComponent("Halo") as Behaviour).enabled = false;
			tileDown = null;
		}
		if (tileRight != null)
		{
			(tileRight.GetComponent("Halo") as Behaviour).enabled = false;
			tileRight = null;
		}
		if (tileLeft != null)
		{
			(tileLeft.GetComponent("Halo") as Behaviour).enabled = false;
			tileLeft = null;
		}
	}

	//deactivates the "halos" if the movetiles are blocked by active walls
	public void wallBlocked (int x ,int y) 
	{
		//blocks the movement if there is a wall above, adds a modifier to block the higher up move square
		if (modUpActive)
		{ 
			if ((y)+2 < gridHeight-1 && horEmptyWalls[x,y+1].transform.renderer.material.color == wallColor)
			{
				(tileUp.GetComponent("Halo") as Behaviour).enabled = false;
				print ("blocked up");
			}
		}

		//if the pawns are not adjacent, blocks regular up movement 
		else 
		{	
				if (y+1 < gridHeight-1 && horEmptyWalls[x,y].transform.renderer.material.color == wallColor)
				{
					(tileUp.GetComponent("Halo") as Behaviour).enabled = false;
					print ("blocked up");
				}
		}
		
		//blocks the movement if there is a wall below, adds a modifier to block the low down move square
		if (modDownActive) 
		{
			if (y-2 >= 0 && horEmptyWalls[x,y-2].transform.renderer.material.color == wallColor)
			{	
				(tileDown.GetComponent("Halo") as Behaviour).enabled = false;
				print ("blocked down");
			}
		}
		
		//if the pawns are not adjacent, blocks regular down movement 
		else 
		{
			if (y-1 >= 0 && horEmptyWalls[x,y-1].transform.renderer.material.color == wallColor)
			{
				(tileDown.GetComponent("Halo") as Behaviour).enabled = false;
				print ("blocked down");
			}
		}
		
		//blocks the movement if there is a wall to the left, adds a modifier to block the left move square one space farther away
		if (modLeftActive)
		{
			if (x-2 >= 0 && vertEmptyWalls[x-2,y].transform.renderer.material.color == wallColor )
			{
				(tileLeft.GetComponent("Halo") as Behaviour).enabled = false;
				print ("blocked left");
			}
		}
		
		//if the pawns are not adjacent, blocks regular left movement 
		else 
		{
			if (x-1 >= 0 && vertEmptyWalls[x-1,y].transform.renderer.material.color == wallColor )
			{
				(tileLeft.GetComponent("Halo") as Behaviour).enabled = false;
				print ("blocked left");
			}
		}

		//blocks the movement if there is a wall to the right, adds a modifier to block the right move square one space farther away
		if (modRightActive)
		{
			if (x+1 < gridWidth-1 && vertEmptyWalls[x+1,y].transform.renderer.material.color == wallColor )
			{
				(tileRight.GetComponent("Halo") as Behaviour).enabled = false;
				print ("blocked right");
			}
		}

		//if the pawns are not adjacent, blocks regular right movement 
		else 
		{
			if (x < gridWidth-1 && vertEmptyWalls[x,y].transform.renderer.material.color == wallColor )
			{
				(tileRight.GetComponent("Halo") as Behaviour).enabled = false;
				print ("blocked right");
			}
		}

		

	}

	//turns midsegemtns brown and checks for invalid wall placement
	public void checkMidSegments ()
	{
		//checks all the midSegments in the grid
		for (int x = 0; x < (gridWidth-1); x++)
		{
			for (int y = 0; y < gridHeight-1; y++)
			{
				
					//edge case for two vertical walls on either side of a horizontal wall
					if ((x+1 > gridWidth - 1 && x-1 > 0)
						&& (midWallSegs[x+1,y].transform.renderer.material.color == wallColor
						&& midWallSegs[x+1,y].tag == "Vertical Wall"
						&& midWallSegs[x-1, y].transform.renderer.material.color == wallColor
						&& midWallSegs[x-1, y].tag == "Vertical Wall"))
					{
						midWallSegs[x,y].transform.renderer.material.color = wallColor;	
						midWallSegs[x,y].tag = "Horizontal Wall";
					} 

					//turns any midSegments with a "... Wall" brown
					if (midWallSegs[x,y].tag == "Horizontal Wall" || midWallSegs[x,y].tag == "Vertical Wall")
					{
						midWallSegs[x,y].transform.renderer.material.color = wallColor;
					}

					//turns all other midSegments grey
					else if (midWallSegs[x,y].tag == "Unclaimed Mid-Segment")
					{
						midWallSegs[x,y].transform.renderer.material.color = Color.grey;
					}
				}
			}
	}

	public void turnInvalidCubesWhiteAgain ()
	{
		//resets any invalid move tiles to white (from red), after the pawn moves 
		if (invalidCubesHighlighted && moveCommitted)
		{
			int i = 0;
			while (i < numInvalidHighlights) 
			{
				invalidMoveSpaces[i].transform.renderer.material.color = Color.white;
				//clears the spaces in the array 
				invalidMoveSpaces[i] = null;
				i++;
			}
			//resets the turn booleans and array counter 
			invalidCubesHighlighted = false;
			turnEnded = false;
			numInvalidHighlights = 0;
			i = 0;
		}
	}
	//moves the pawn when it is active
	public void movePawn (GameObject moveSpace)
	{
		//if the pawn is active and the movespace is valid (is one of the designated movespaces and has its "halo" enabled
		if (pawnIsActive
		&& (moveSpace.GetComponent("Halo") as Behaviour).enabled == true
		&& (moveSpace == tileUp 
		|| moveSpace == tileDown
		|| moveSpace == tileRight
		|| moveSpace == tileLeft))
		{
			//deactivate the pawn 
			activePawn.transform.position = new Vector3 (moveSpace.transform.position.x, moveSpace.transform.position.y, -1f);
			activePawn.transform.localScale = new Vector3 (1, 1, 1);
			
			//move the pawn 
			hideValidMoves ();
			
			//update activePawnPos
			activePawnPosX = moveSpace.transform.position.x;
			activePawnPosY = moveSpace.transform.position.y;
			
			//update pawn position int variables 
			updatePawnPosition();
			
			//switches playerTurn and the function to reset all turn variables
			moveCommitted = true;
			turnInvalidCubesWhiteAgain();
		}	

		//highlights all clicked cubes that are not valid moves in red
		else if (pawnIsActive && (moveSpace.GetComponent("Halo") as Behaviour).enabled == false) 
		{
			moveSpace.transform.renderer.material.color = Color.red;
			invalidMoveSpaces[numInvalidHighlights++] = moveSpace;
			invalidCubesHighlighted = true;
			playerFeedback = "Invalid Move :0";
			StartCoroutine(changeBackPlayerFeedback());
		}
		
		//if player 1's pawn has reached the other side, load screen win
		if (pawn1GridXLocation == 8) 
		{
			Application.LoadLevel ("Player1Win");
		}

		//if player 1's pawn has reached the other side, load screen win
		if (pawn2GridXLocation == 0)
		{
			Application.LoadLevel ("Player2Win");
		}
	}

	//sets pawn position to the array coordinates of its corresponding tile
	public bool updatePawnPosition ()
	{
		bool pawnPositionLocated = false;
		
		//locates the pawn, based on whether is has the same position as any of the tiles	
		for (int x = 0; x < gridWidth; x++)
		{
			for (int y = 0; y < gridHeight; y++)
			{
				if (activePawn != null
					&& activePawn.transform.position.x == tiles [x, y].transform.position.x
					&& activePawn.transform.position.y == tiles [x, y].transform.position.y) 
				{
					pawnPositionLocated = true;
					//assigns the new pawn position, based on which player's turn it is
					switch (playerTurn)
					{
					case "Player 1's Turn":
						pawn1GridXLocation = x;
						pawn1GridYLocation = y;
						break;

					case "Player 2's Turn":
						pawn2GridXLocation = x;
						pawn2GridYLocation = y;
						break;

					default:
						break;
					}
				}
			}						
		}
		return pawnPositionLocated;
	}

	//detects an adjacent pawn: allowing for the jumping condition
	public string adjacentPawn (int x1, int y1, int x2, int y2)
	{
		//if the second pawn is on the right
		if (x1 + 1 == x2 && y1 == y2) 
		{
			modUpActive = true;
			return "Pawn Right";
			
		}

		//if the second pawn is on the left
		if (x1 - 1 == x2 && y1 == y2) 
		{
			modLeftActive = true;
			return "Pawn Left";
			
		}

		//if the second pawn is above
		if (y1 + 1 == y2 && x1 == x2) 
		{
			modUpActive = true;
			return "Pawn Above";	
		}

		//if the second pawn is below
		if (y1 - 1 == y2 && x1 == x2) {
			modDownActive = true;
			return "Pawn Below";	
		} 

		//otherwise, no pawn is adjacent 
		else 
		{
			print ("No Pawn");
			return "No Pawn";	
		}
	}	 

	//CoRoutine delay to change "playerFeedback" back to delay
	IEnumerator changeBackPlayerFeedback ()
	{
		yield return new WaitForSeconds (3.0f);
		playerFeedback = "Select A Move :)"; 
		coRoutineFeedbackActive = true;
		
	}

	//changes the string used within the player feedback box to address active player
	public void switchPlayerDesignator ()
	{
		if (playerTurn == "Player 1's Turn")
		{
			playerDesignator = "Player 1";
		}

		else if (playerTurn == "Player 2's Turn")
		{
			playerDesignator = "Player 2";
		}
	}

	//casts a ray out right
	public GameObject castHorizontalRight(GameObject receivedCaster)
	{
		RaycastHit hit;
		GameObject currentCaster = receivedCaster;

		Physics.Raycast(currentCaster.transform.position, Vector3.right, out hit);
		//returns the hit gameObject
		return hit.transform.gameObject;
	}

	//casts a ray out left
	public GameObject castHorizontalLeft(GameObject receivedCaster)
	{
		RaycastHit hit;
		GameObject currentCaster = receivedCaster;

		Physics.Raycast(currentCaster.transform.position, Vector3.left, out hit);
		//returns the hit gameObject
		return hit.transform.gameObject;
	}

	//casts a ray upwards
	public GameObject castVerticalUp(GameObject receivedCaster)
	{
		RaycastHit hit;
		GameObject currentCaster = receivedCaster;

		Physics.Raycast(currentCaster.transform.position, Vector3.up, out hit);
		//returns the hit gameObject
		return hit.transform.gameObject;
	}
	
	//casts a ray downwards
	public GameObject castVerticalDown(GameObject receivedCaster)
	{
		RaycastHit hit;
		GameObject currentCaster = receivedCaster;

		Physics.Raycast(currentCaster.transform.position, Vector3.down, out hit);
		//returns the hit gameObject
		return hit.transform.gameObject;
	}

	public void setActivePlayerColor ()
	//changes wall color to match the color of the player's pawn
	{
		switch (playerTurn)
		{
			case "Player 1's Turn":
				activePlayerColor = Color.blue;
				break;
	
			case "Player 2's Turn":
				activePlayerColor = Color.green;
				break;
				
			default:
				break;
		}
	}

	//returns the opposite pawns position, in order to check wall blocking 
	public void setEnemyPlayerPosition ()
	{
	if (playerTurn == "Player 1's Turn")
		{
			enemyGridXLocation = pawn2GridXLocation;
			enemyGridYLocation = pawn2GridYLocation;
		}
	
		else if (playerTurn == "Player 2's Turn")
		{
			enemyGridXLocation = pawn1GridXLocation;
			enemyGridYLocation = pawn1GridYLocation;	
		}		
	}	

	//turns the currently active midSegment brown
	public GameObject changeActiveMidSegment (GameObject placedWall1, GameObject placedWall2)
	{

		GameObject activeMidSegment = null;

		if (placedWall1.tag == "Horizontal Wall")
		{
			//case for wallSegment1 being on the right and wallSegment2 on the left of their midsegment
			if (castHorizontalLeft (placedWall1) == castHorizontalRight (placedWall2))
			{
			if (castHorizontalLeft (placedWall1).tag == "Unclaimed Mid-Segment")
				{
				castHorizontalLeft (placedWall1).transform.renderer.material.color = activePlayerColor;
				castHorizontalLeft (placedWall1).tag = "Horizontal Wall";
				activeMidSegment = castHorizontalLeft (placedWall1);
				placedWall1.tag = "Horizontal Wall Right";
				placedWall2.tag = "Horizontal Wall Left"; 
				}

			else 
				{
					//if there is available midSegment (overlapping walls), deactivate the plaed segments
					deactivateCurrentWalls();
				}
			}

			//case for wallSegment1 being on the left and wallSegment2 on the right of their midsegment
			else if (castHorizontalRight (placedWall1) == castHorizontalLeft(placedWall2))
			{
				if (castHorizontalRight (placedWall1).tag == "Unclaimed Mid-Segment")
				{
					castHorizontalRight (placedWall1).transform.renderer.material.color = activePlayerColor;
					castHorizontalRight (placedWall1).tag = "Horizontal Wall";
					activeMidSegment = castHorizontalRight (placedWall1);
					placedWall1.tag = "Horizontal Wall Left";
					placedWall2.tag = "Horizontal Wall Right";
				} 
			
				else 
				{
					//if there is available midSegment (overlapping walls), deactivate the plaed segments
					deactivateCurrentWalls();
				}
			}
		}	

		else if (placedWall1.tag == "Vertical Wall") 
		{	
			//case for wallSegment1 being above and wallSegment2 below their midsegment
			if (castVerticalDown (placedWall1) == castVerticalUp (placedWall2))
			{
				if (castVerticalDown (placedWall1).tag == "Unclaimed Mid-Segment")
				{
				castVerticalDown (placedWall1).transform.renderer.material.color = activePlayerColor;
				castVerticalDown (placedWall1).tag = "Vertical Wall";
				activeMidSegment = castVerticalDown (placedWall1);
				placedWall1.tag = "Vertical Wall Above";
				placedWall2.tag = "Vertical Wall Below"; 
				}
			
				else 
				{
					//if there is available midSegment (overlapping walls), deactivate the plaed segments
					deactivateCurrentWalls();
				}
			}

			//case for wallSegment1 being below and wallSegment2 above their midsegment
			else if (castVerticalUp (placedWall1) == castVerticalDown (placedWall2))
			{
				if (castVerticalUp (placedWall1).tag == "Unclaimed Mid-Segment")
				{
				castVerticalUp (placedWall1).transform.renderer.material.color = activePlayerColor;
				castVerticalUp (placedWall1).tag = "Vertical Wall";
				activeMidSegment = castVerticalUp (placedWall1);
				placedWall1.tag = "Vertical Wall Below";
				placedWall2.tag = "Vertical Wall Above"; 
				}
				
				else 
				{
					//if there is available midSegment (overlapping walls), deactivate the plaed segments
					deactivateCurrentWalls();
				}
			}
		}

		//returns the midSegment gameObject
		return activeMidSegment;
	}

	
	//checks the available moves for a pawn 
	public bool checkConnections (int x, int y)
	{	
		//sets all connections back to false
		for (int x1 = 0; x1 < gridWidth; x1++)
		{
			for (int y1 = 0; y1 < gridHeight; y1++)
			{
				connectedSquares[x1,y1] = false;
			}
		}
		
		// sets the square that the pawn is currently inhabiting to "true"
		connectedSquares[x,y] = true;	

		//extends the search to all available moves
		generateConnections(x,y);
		
	 //creates a bool to test whether the pawn can reach the other side 
	 bool connected = false;
     for (int y1 = 0; y1 < gridWidth-1; y1++)
        {
           //if there's still a way to reach the opposite side, returns true
			if (connectedSquares[enemyTargetX, y1])
			{
				connected = true;
			}
        }
		//returns true, if the pawn still has a a valid move path to the other side 
      return connected;
	}

	public void generateConnections (int x, int y)
	{
		//checks the square to the right of the pawn
		if ((x+1 < gridWidth && x+1 > 0)
			 && vertEmptyWalls[x,y].transform.renderer.material.color == Color.grey 
			 && connectedSquares[x+1,y] == false)
		{
			connectedSquares[x+1,y] = true;
			generateConnections(x+1, y);
			
		}
		
		//checks the square to the left of the pawn
		if ((x-1 < gridWidth && x > 0)
			 && vertEmptyWalls[x-1,y].transform.renderer.material.color == Color.grey
			 && connectedSquares[x-1,y] == false)
		{
			connectedSquares[x-1,y] = true;
			generateConnections(x-1, y);
			print ("good to go left");		
		}

		//checks the square below the pawn
		if ((y-1 < gridHeight && y > 0)
			 && horEmptyWalls[x,y-1].transform.renderer.material.color == Color.grey
			 && connectedSquares[x, y-1] == false)
		{
			connectedSquares[x, y-1] = true;
			generateConnections(x, y-1);
			
		}

		//checks the square above the pawn
		if ((y+1 < gridHeight && y+1 > 0)
			 && horEmptyWalls[x,y].transform.renderer.material.color == Color.grey
	 		 && connectedSquares[x,y+1] == false)
		{
			connectedSquares[x,y+1] = true;
			generateConnections(x, y+1);
			
		}
	}
}