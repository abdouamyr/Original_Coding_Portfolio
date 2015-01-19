using UnityEngine;
using System.Collections;

public class CubeControl : MonoBehaviour
{
	OverwatchControl cubesScript;
	public Airplane myAirplane;
	public static int DesiredPosX = 0;
	public static int DesiredPosY = 16;
	public int x;
	public int y;
	
	void Start ()
	{		
		cubesScript = GameObject.Find("Overwatch").GetComponent<OverwatchControl>();
	}
	
	// Use this for initialization
	void OnMouseDown ()
	{	
		cubesScript.processClickedCube(gameObject, x, y);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}	
}	
	