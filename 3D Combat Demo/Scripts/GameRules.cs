using UnityEngine;
using System.Collections;

public class GameRules : MonoBehaviour {

	//determines the bounds of the map
	public float mapWidth = 50; 
	public float mapHeight = 50;

	//identifies the "player" object for other scripts' references 
	public GameObject player;
	public GameObject playerWeapon;

	//establishes the ground 
	public GameObject ground; 

	public bool monkey = true;

	// Use this for initialization
	void Start () {
		//sets the size of the game map
		ground.transform.localScale = new Vector3(mapWidth/5, 10, mapHeight/5);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//returns true while the character is within the map boundaries
	public float inHorizontalBounds (GameObject character, string xDir, float x) {
		if (character.transform.position.x > mapWidth - 3 && xDir == "Right") {
			return 0;
		} else if (character.transform.position.x < mapWidth * -1 + 3 && xDir == "Left") {
			return 0;
		} else {
			return x;
		}
	}

	public float inVerticalBounds (GameObject character, string yDir, float y) {
		if (character.transform.position.z > mapHeight - 3 && yDir == "Up") {
			return 0;
		} else if (character.transform.position.z < mapHeight * -1 + 3 && yDir == "Down") {
			return 0;
		} else {
			return y;
		}
	}
}
