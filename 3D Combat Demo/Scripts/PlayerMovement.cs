using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	//links script to main controller
	public GameObject gameController;
	GameRules controller;
	PlayerStats stats;

	//adjusts the normal speed of the player
	public float speedMod = 5;
	public int baseSpeed = 5;
	public int originalSpeed = 5;

	//adjusts the sprint speed of the player
	public float sprintMod = 4;
	public float sprintSpeed;

	//determines the direction in which the player is moving
	public string yDir;
	public string xDir; 

	// Use this for initialization
	void Start () {
		controller = gameController.GetComponent<GameRules>();
		stats = gameObject.GetComponent<PlayerStats>(); 
		sprintSpeed = (float) originalSpeed * (float) sprintMod;
	}

	// Update is called once per frame
	void Update () {
		//sets sprint speed
		sprintSpeed = originalSpeed * sprintMod;

		//increases the player's speed while "shift" is held down
		if (Input.GetButton("Sprint") && stats.sprint > 0) {
			speedMod = sprintSpeed;
			//sprint speed increases the longer it's held down
			if (sprintMod < 6) {
				sprintMod += 0.01F;
			}
		} else { //resets sprint 
			speedMod = sprintSpeed/sprintMod;
			sprintMod = 4;
		}

		//syncs the movement to the framerate 
		float speed = Time.deltaTime * baseSpeed;

		//modifies the movement based on key input
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");

		//sets the direction values of the player
		updateDirection();

		//moves the player, if the player is within the bounds of the map
		gameObject.transform.position += new Vector3 (controller.inHorizontalBounds(gameObject, xDir, x), 0, controller.inVerticalBounds(gameObject, yDir, y)) * speed;


		//resets the cube's rotation
		gameObject.transform.rotation = Quaternion.identity;
	}

	//determines the direction of the player, based on which movement keys are held down
	void updateDirection () {
		if (Input.GetAxis("Horizontal") > 0) {
			xDir = "Right";
		} else if (Input.GetAxis("Horizontal") < 0) { 
			xDir = "Left";
		} else {
			xDir = "None";
		}

		if (Input.GetAxis("Vertical") > 0) {
			yDir = "Up";
		} else if (Input.GetAxis("Vertical") < 0) {
			yDir = "Down";
		} else {
			yDir = "None";
		}
	}
}
