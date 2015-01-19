using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {
	GameRules controller; 
	public GameObject gameRules;
	bool timeToSwing = false;
	public bool swinging = false;
	int swingCounter = 0;

	// Use this for initialization
	void Start () {
		controller = gameRules.GetComponent<GameRules>();
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.position = new Vector3 (controller.player.transform.position.x + 1, controller.player.transform.position.y-1, controller.player.transform.position.z + 1);

		if (Input.GetMouseButtonDown(0)) {
			timeToSwing = true;
		} 

		if (timeToSwing && swingCounter < 20) {
			swingCounter++;
			swingWeapon();
			swinging = true; 
		} else {
			gameObject.transform.rotation = Quaternion.identity;
			timeToSwing = false;
			swinging = false;
			swingCounter = 0;
		}
	}

	void swingWeapon() {
		gameObject.transform.Rotate(0, -Time.deltaTime*300, 0, Space.World);
	}
}
