using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {
	//sets the values for each character attribute 
	public float health = 200; 
	public float sprint = 200;
	public float mana = 200; 

	public GameObject gameRules;
	GameRules controller;
	PlayerAbilityScript abilities;


	//used to monitor sprinting 
	int sprintCounter = 0;
	int stoppedCounter = 0;
	public bool sprinting = false;
	public bool beginRegenCountdown = false;
	public int manaCounter = 0;
	public bool shieldActive = false;

	// Use this for initialization
	void Start () {
		controller = gameRules.GetComponent<GameRules>();
		abilities = gameObject.GetComponent<PlayerAbilityScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if ((gameObject.GetComponent("Halo") as Behaviour).enabled == true) {
			shieldActive = true;
		} else {
			shieldActive = false;
		}

		//decreases remaining sprint if running
		if (Input.GetButton("Sprint") && (Input.GetButton("Horizontal") || Input.GetButton("Vertical")) && sprint > 0) {
			sprint--;
			sprinting = true;
		} else {
			stoppedCounter++;
			sprinting = false;
		}

		//used to regenerate sprint bar if not moving or empty
		if ((sprint == 0 || stoppedCounter == 100) && sprintCounter < 100) {
			sprintCounter++; 
			stoppedCounter = 0;
		} else if (sprint < 200 && sprintCounter > 0 && sprinting == false) {
			sprint++;
		} else {
			sprintCounter = 0;	
		}

		//kills the player if health has been reduced
		if (health < 1) {
			Destroy(gameObject);
			controller.player = null;
		}

		if (shieldActive && mana > 0) {
			mana--;
		} else if (shieldActive && mana == 0) {
			(gameObject.GetComponent("Halo") as Behaviour).enabled = false;
		} else if (shieldActive == false && mana < 200) {
			beginRegenCountdown = true;
		}

		if (abilities.usedMagic) {
			mana -= 5;
			abilities.usedMagic = false;
			beginRegenCountdown = true;
		}

		if (beginRegenCountdown && manaCounter < 201) {
			manaCounter++;
		} else if (manaCounter > 200 && mana < 200) {
			beginRegenCountdown = false;
			mana++;
		} else if (manaCounter > 0) {
			manaCounter = 0;
		}
	}

	//decreases character health when it comes into contact with an enemy
	void OnCollisionEnter(Collision collision) { 
		if (collision.collider.transform.tag ==  "enemy" && shieldActive == false) {
			health-=10;
		}
	}
}
