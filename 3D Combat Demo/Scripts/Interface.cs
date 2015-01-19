using UnityEngine;
using System.Collections;

public class Interface : MonoBehaviour {
	//estbalishes a speed variable to be controlled 
	public float speed = 0.0F;

	//links to the player scripts 
	PlayerMovement movement;
	PlayerStats stats;

	//link to the game controller object
	public GameObject gameRules;

	//the stat bar textures
	public Texture2D healthBar;
	public Texture2D sprintBar;
	public Texture2D manaBar;

	//link to the main game controller script
	GameRules controller;

	// Use this for initialization
	void Start () {
		//establishes link to game controller script
		controller = gameRules.GetComponent<GameRules>();

		//establishes link to player scripts
		stats = controller.player.GetComponent<PlayerStats>();
		movement = controller.player.GetComponent<PlayerMovement>();
	}

	void OnGUI() {
		//creates a mouse control to modify the character's base speed (for debugging)
		GUI.BeginGroup(new Rect(100, 500, 200, 100));
			GUI.Box(new Rect(0, 0, 200, 50), "Speed:");
			speed = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), speed, 0.0F, 10.0F);
		GUI.EndGroup();

		//creates the health bar
		GUI.BeginGroup (new Rect(100, 100, stats.health, 100));
			GUI.Box(new Rect(0, 0, 200, 20), healthBar);
		GUI.EndGroup();


		//creates the sprint bar
		GUI.BeginGroup (new Rect(100, 100, stats.sprint, 100));
			GUI.Box(new Rect(0, 30, 200, 20), sprintBar);
		GUI.EndGroup();

		//creates the mana bar
		GUI.BeginGroup (new Rect(100, 100, stats.mana, 100));
			GUI.Box(new Rect(0, 60, 200, 20), manaBar);
		GUI.EndGroup();

	}
	
	// Update is called once per frame
	void Update () {
		//modifies character's speed based on the slider's position
		movement.baseSpeed = Mathf.RoundToInt(speed * movement.speedMod + movement.speedMod);
	}


}
