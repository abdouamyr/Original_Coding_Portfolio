using UnityEngine;
using System.Collections;

public class AdvancedSpawn : MonoBehaviour{

	
	public float cubeCount;
	public Transform spawnBronze;
	public Transform spawnSilver;
	public Transform spawnGold;
	public Transform changeColor;
	float timeElapsed;
	RaycastHit hit;
	public int SilverCount, BronzeCount, GoldCount;
	public float gameScore;
	public float GameClock = 0F;
	public float GameMin = 0F;
	public float OnScreenTime = 0F;
	public bool SecondMax;
	void OnGUI(){
		GUI.Label(new Rect (0,0,1000,500), "Score:" + gameScore.ToString());
		GUI.Label(new Rect (0,25,100,50), GameMin + ":" + OnScreenTime.ToString()); 
			
		}
	
		
	

	
	
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	
	void Update ()
	{
		
		if (GameClock > OnScreenTime){
			OnScreenTime += 1;
			SecondMax= false;}
		
		if (OnScreenTime > 60){
			GameMin += 1;
			OnScreenTime = 0;
			GameClock = 0;
			SecondMax = true;}
		
		if(SecondMax = true){
			OnScreenTime=0;
			GameClock=0;}
		
		timeElapsed = timeElapsed + Time.deltaTime;
		GameClock = Time.time;
		
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			
			if (Physics.Raycast (ray, out hit)) {
				if (hit.transform.tag == "Bronze") {
					GameObject.Destroy (hit.transform.gameObject);
					cubeCount --;//-- means minus one, ++ means plus one
					BronzeCount --;
					gameScore += 1;
					
					 
				} else if (hit.transform.tag == "Silver") {
					GameObject.Destroy (hit.transform.gameObject);
					cubeCount --;//-- means minus one, ++ means plus one
					SilverCount --;
					gameScore += 10;
					 
				} else if (hit.transform.tag == "Gold") {
					GameObject.Destroy (hit.transform.gameObject);
					cubeCount --;//-- means minus one, ++ means plus one
					GoldCount --;
					gameScore += 100;
					 
				
					
				}
				

			}
				
		}
			
			
		if (BronzeCount == 2 && SilverCount == 2) {
			if (GoldCount < 1) {
				print ("spawngold");
				Instantiate (spawnGold, new Vector3 (Random.Range (-30, 30), Random.Range (-30, 30), Random.Range (-30, 30)), Quaternion.identity);
				cubeCount += 1;
				GoldCount += 1;
			}
		}	
		
				
		if (timeElapsed > 3) {
			if (BronzeCount < 4) {
				timeElapsed = 0; 
				Instantiate (spawnBronze, new Vector3 (Random.Range (-30, 30), Random.Range (-30, 30), Random.Range (-30, 30)), Quaternion.identity);
				cubeCount += 1;	 
				BronzeCount += 1;
			}
		}
		
	
		if (timeElapsed > 3) { 
			if (cubeCount > 3) {
				Instantiate (spawnSilver, new Vector3 (Random.Range (-30, 30), Random.Range (-30, 30), Random.Range (-30, 30)), Quaternion.identity);
				timeElapsed = 0; 
				cubeCount += 1;
				SilverCount += 1;
			}
		}
			
			
				
		//points 
	
		
			
	} 
	
}
	
