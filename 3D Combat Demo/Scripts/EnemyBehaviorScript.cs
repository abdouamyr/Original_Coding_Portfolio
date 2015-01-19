using UnityEngine;
using System.Collections;

public class EnemyBehaviorScript : MonoBehaviour {
	Deactivation proximity; 
	GameRules controller; 
	WeaponScript playerWeapon;

	public GameObject gameRules;
	public bool swinging = false;

	// Use this for initialization
	void Start () {
		proximity = gameObject.GetComponent<Deactivation>();
		controller = gameRules.GetComponent<GameRules>();
		playerWeapon = controller.playerWeapon.GetComponent<WeaponScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if (proximity.nearPlayer) {
			attackPlayer();
		}
	}

	//tasks the enemy to follow the player
	void attackPlayer() {
		gameObject.transform.LookAt(controller.player.transform.position);
		gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, controller.player.transform.position, Time.deltaTime);
	}

	void OnCollisionEnter(Collision collision) { 
		if ((collision.collider.transform.tag ==  "weapon" && playerWeapon.swinging) || collision.collider.transform.tag == "playerMagic") {
			Destroy(gameObject);
		}
	}
}