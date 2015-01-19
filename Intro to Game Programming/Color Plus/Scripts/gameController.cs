using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour {
	public GameObject [,] cubes;
	public Color [] cubeColor;
	public int numCubesHor = 8;
	public int numCubesVert = 5;
	public GameObject WhiteCube; 
	public static int totalScore;
	int randomNumber;
	public float mainTimer;
	public float syncTimer;
	public float countdownTimer = 60;
	public bool plusFormation;
	public static bool gameWin;
	// Use this for initialization
	void Start () 
	{
		cubes = new GameObject[numCubesHor, numCubesVert];
		for (int x = 0; x < numCubesHor; x++)
		{
			for (int y = 0; y < numCubesVert; y++)
			{
				cubes [x, y] = (GameObject) Instantiate (WhiteCube, new Vector3 (x * 2, y * 2, 0), Quaternion.identity);
				cubes [x, y].renderer.material.color = Color.white;
			}						
		}
		
		Instantiate (WhiteCube, new Vector3 (7, -2, 0), Quaternion.identity);
		
		cubeColor = new Color[numCubesVert];
		cubeColor [0] = Color.black;
		cubeColor [1] = Color.blue;
		cubeColor [2] = Color.green;
		cubeColor [3] = Color.red;
		cubeColor [4] = Color.yellow;
		
		Destroy(cubes [Random.Range(0,7), Random.Range (0,4)]);
	}
	
	void OnGUI () 
	{
		// Make a background box
		GUI.Box(new Rect(10,10,100,30), "Score:" + totalScore.ToString());
		GUI.Box(new Rect(375, 315, 100, 30), "Next Cube" );
		
	}
	
	
	// Update is called once per frame
	void Update () 
	{
		mainTimer += Time.deltaTime;
		
		//check for the "+" arrangment of cubes
		//spawn "NextCube"
		//destroy a bock if no number input is pressed 
		//number key input
		//ends game if timer expires
		//ends game if there are no available cubes 
		//upon end game, load "endScreen" (different "Load Level") 
		if (totalScore > 0)
			{
				gameWin = true;
			}	
	}
	
	public bool detectPlus ()
	{	
		for (int x = 0; x < numCubesHor; x++)
		{
			for (int y = 0; y < numCubesVert; y++)
			{
				if (cubes [x, y].transform.renderer.material.color == cubes [x++, y++].transform.renderer.material.color&&
					cubes [x, y].transform.renderer.material.color == cubes [x--, y--].transform.renderer.material.color&&
					cubes [x, y].transform.renderer.material.color == cubes [x++, y--].transform.renderer.material.color&&
					cubes [x, y].transform.renderer.material.color == cubes [x--, y++].transform.renderer.material.color)
					{
						plusFormation = true;	
					}
				else 
					{
						plusFormation = false;
					}
				}
			}
		return plusFormation;
	}
	
	//detects when the cubes are aligned in a "+" formation (same color, or all colors)
	//returns true if this arrangment is found	
	
	public void processClickedCube ()
	{
		
	}
	
		public float randomColumn ()
	{
		randomNumber = Random.Range(1,5);
		return (float) randomNumber;
	}
		
	//chooses and random number (1-5) and returns it (to assign the y coordinate of where "NextCube" moves to	
	public Color changeColor () 
	{
	//What if you had an array of colors, picked a random int, and then used that element of the color array? It would take fewer lines of code, and be easier to maintain if the game designer wants to add or remove colors later (or change from colors to textures).
		randomNumber = Random.Range(1,5);
		renderer.material.color = cubeColor [randomNumber];
		return renderer.material.color;		
	}
	
}
