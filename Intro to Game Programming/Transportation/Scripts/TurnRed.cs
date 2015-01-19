using UnityEngine;
using System.Collections;

public class TurnRed : MonoBehaviour
{
	OverwatchControl cubesScript;
	
	void Start ()
	{	
		cubesScript = GameObject.Find("Overwatch").GetComponent<OverwatchControl>();
	}
	
	// Use this for initialization
	void OnMouseDown ()
	{	
		cubesScript.processClickedCube(gameObject);
	}
	
	void OnTriggerEnter (Collider info)
	{
		if(info.tag == "Airplane") renderer.material.color = Color.yellow;
	}
	
	
	// Update is called once per frame
	void Update ()
	{

	}		 
		
}	
