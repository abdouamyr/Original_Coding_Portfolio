using UnityEngine;
using System.Collections;

public class Deactivation : MonoBehaviour {
	//establishes line to the main controller
	public GameObject gameRules;
	public GameRules controller;

	//indicates whether GameObject is in range of player
	public bool nearPlayer;

	//the view dimensions of the camera
	public float screenHeight = 8;
	public float screenWidth = 16;

	//the material of the object
	public Color naturalColor;

	// Use this for initialization
	void Start () {
		//link to the main controller
		controller = gameRules.GetComponent<GameRules>();

		//sets the starting material 
		naturalColor = gameObject.transform.renderer.material.color;
	}
	
	// Update is called once per frame
	void Update () {
		//sets the object to red if the player is far enough away from it
		if (Mathf.Abs(gameObject.transform.position.x - controller.player.transform.position.x) > screenWidth || Mathf.Abs(gameObject.transform.position.z - controller.player.transform.position.z) > screenHeight) {
			gameObject.transform.renderer.material.color = Color.red;
			nearPlayer = false;
		} else {
			gameObject.transform.renderer.material.color = naturalColor;
			nearPlayer = true;
		}
	}
}
