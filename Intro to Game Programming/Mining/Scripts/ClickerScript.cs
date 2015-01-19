using UnityEngine;
using System.Collections;

public class ClickerScript : MonoBehaviour {
		public GameObject spawnObj;
		RaycastHit hit;
		float Destroyed = 1;
	    public float count, cubeCount;
	// Use this for initialization
	void Start () {
		
	
	
	}
	
	// Update is called once per frame
	void Update () {
		
		
        if (Input.GetMouseButtonDown(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)){
           			 GameObject.Destroy(hit.transform.gameObject);
					 cubeCount -= Destroyed;
						print ("Clicked!");
			}
			}
		}
		}

	



	


