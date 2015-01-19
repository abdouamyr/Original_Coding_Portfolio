using UnityEngine;
using System.Collections;

public class keepAlive : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	//keeps the background scene elements from being destroyed
	void Awake ()
	{
		DontDestroyOnLoad(this.gameObject);
	}
}
