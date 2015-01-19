using UnityEngine;
using System.Collections;

public class PlayerAbilityScript : MonoBehaviour {
	public GameObject magicAmmo;
	public bool usedMagic;
	PlayerStats stats; 
	// Use this for initialization
	void Start () {
		(gameObject.GetComponent("Halo") as Behaviour).enabled = false;
		stats = gameObject.GetComponent<PlayerStats>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(1)) {
			(gameObject.GetComponent("Halo") as Behaviour).enabled = true;
		}

		if ((Input.GetKeyDown("q") || Input.GetKeyDown("e") || Input.GetKeyDown("x") || Input.GetKeyDown("left shift")) && stats.mana > 4) {
			GameObject magicBall;
			magicBall = (GameObject) Instantiate(magicAmmo, gameObject.transform.position, Quaternion.identity);
			usedMagic = true;

			if (Input.GetKeyDown("q")) { 
				magicBall.transform.renderer.material.color = Color.red;
			} else if (Input.GetKeyDown("e")) {
				magicBall.transform.renderer.material.color = Color.blue;
			} else if (Input.GetKeyDown("x")) {
				magicBall.transform.renderer.material.color = Color.green;
			} else if (Input.GetKeyDown("left shift")) {
				magicBall.transform.renderer.material.color = Color.yellow;
			}
		}
	}
}
