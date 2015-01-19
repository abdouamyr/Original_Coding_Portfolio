using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OverwatchControl : MonoBehaviour
{
	public Transform WhiteCube;
	public static int numCubesHor = 16;
	public static int numCubesVert = 9;
	public int z;
	public static Transform[,] cubes;
	public Airplane myAirplane;
	public static bool ActiveAirplane = false;
	public static float CargoLoad;
	public float MoveTimeLoop;
	public int MoveSpace; 
	public bool TimeToMove; 
	public int DesiredPosX;
	public int DesiredPosY; 
	public int goalX;
	public int goalY;
	public int totalScore;
	
	
	// Use this for initialization
	void Start ()
	{		
		myAirplane = new Airplane (0, 90);
		myAirplane.locationX = 0;
		myAirplane.locationY = 16;
		
		cubes = new Transform[numCubesHor, numCubesVert];
		
		// create a grid of cubes
		for (int x = 0; x < numCubesHor; x++)
		{
			for (int y = 0; y < numCubesVert; y++)
			{
				cubes [x, y] = (Transform)Instantiate (WhiteCube, new Vector3 (x * 2, y * 2, z), Quaternion.identity);
				cubes[x, y].GetComponent<CubeControl>().x = x;
				cubes[x, y].GetComponent<CubeControl>().y = y;
				cubes [x, y].renderer.material.color = Color.white;
			}						
		}
		
		// Set the lower left to red
		
		cubes [0, 8].renderer.material.color = Color.red;
		cubes [15, 0].renderer.material.color = Color.black;	
	}

	void Update ()
	{
		MoveTimeLoop += Time.deltaTime;
		
		if (MoveTimeLoop > 1.5)
			{
				MoveTimeLoop = 0;
				if (TimeToMove)
					{
						cubes [myAirplane.locationX/2, myAirplane.locationY/2].renderer.material.color = Color.white;
						myAirplane.locationX = GetMovePos (myAirplane.locationX, goalX);
						myAirplane.locationY = GetMovePos (myAirplane.locationY, goalY);
						cubes [myAirplane.locationX/2, myAirplane.locationY/2].renderer.material.color = Color.yellow;
					}
				
				if (myAirplane.locationX != 30 || myAirplane.locationY != 0)
					{
						cubes[15,0].renderer.material.color = Color.black;
					}
			
				if (myAirplane.locationX == 30 && myAirplane.locationY == 0)
					{
						totalScore += myAirplane.cargo;
						myAirplane.cargo = 0;
					}
			
				if ((myAirplane.locationX == 0 && myAirplane.locationY == 16) && myAirplane.cargo < 90)
					{
						myAirplane.cargo +=10;
					}
			}
	}	
	void OnGUI()
	{
		GUI.Box(new Rect(10,10,150,50), "Score:" + totalScore.ToString()+ "  |  " + "Cargo:" + myAirplane.cargo.ToString());
							
	}
	public void processClickedCube (GameObject ClickedCube, int x, int y)
	{
		//when airplane is active and clicked on clear sky
		if (myAirplane.airplaneActive && (ClickedCube.renderer.material.color == Color.white || ClickedCube.renderer.material.color == Color.black)) 
		{	
			
			goalX = (int) ClickedCube.transform.position.x;
			goalY = (int) ClickedCube.transform.position.y;
			DesiredPosX = GetMovePos (myAirplane.locationX, goalX);
			DesiredPosY = GetMovePos (myAirplane.locationY, goalY);
			print (myAirplane.locationX + ", " + myAirplane.locationY);
			TimeToMove = true;
		}
					
		else if (ClickedCube.renderer.material.color == Color.red) 
		{
			ClickedCube.renderer.material.color = Color.yellow; 
			myAirplane.airplaneActive = true;	
		}
		
		else if (ClickedCube.renderer.material.color == Color.yellow) 
		{
			ClickedCube.renderer.material.color = Color.red;
			myAirplane.airplaneActive = false;	
			TimeToMove = false;
		}	
		//move vehicle down 1, right 1 every 1.5 seconds
		//score += cargo on vehicle when it touches black cube
		//when vehicle returns to starting location, add 10 to cargo
		//active vehicle should move towards clicked location, in line with turns 
		//only vehicle active at a time
		//can be redirected with click
		
		
	}
	int GetMovePos (int StartPos, int EndPos) 
	{
		if (StartPos < EndPos)
			{
				MoveSpace = StartPos+2;
				return MoveSpace;
			}
		else if (StartPos > EndPos)
			{
				MoveSpace = StartPos-2;
			return MoveSpace;
			}
		else 
			{
				MoveSpace = StartPos;
			return MoveSpace;
			}
	}
}	



