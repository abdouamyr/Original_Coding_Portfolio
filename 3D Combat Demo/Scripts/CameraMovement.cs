using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	public GameObject player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//keeps the camera centered on the player
		gameObject.transform.position = new Vector3 (player.transform.position.x, 20, player.transform.position.z-8);
	}
}
