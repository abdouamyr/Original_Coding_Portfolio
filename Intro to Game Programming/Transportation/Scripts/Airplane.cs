using UnityEngine;
using System.Collections;

public class Airplane 
{	
	public int cargoCapacity = 90;
	public int cargo = 0; 
	public int locationX,locationY;
	public bool airplaneActive;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	
	}
	
	public Airplane(int cargoOnPlane, int Capacity)
	{ 
		cargo = cargoOnPlane;
		cargoCapacity = Capacity; 
		
	}

}
