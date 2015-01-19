using UnityEngine;
using System.Collections;

public class clickedWallSegmentScript : MonoBehaviour 
{
	gameController clickScript;

	// Use this for initialization
	void Start () 
	{
		//calls a reference to the main class
		clickScript = GameObject.Find("GameController").GetComponent<gameController>();
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void OnMouseDown ()
	{
		//passes wal segment in as wallSegment1 if there are no active wall segments
		if(clickScript.oneWallSegmentClicked == false)
		{
			clickScript.selectWallSegment1(gameObject);
		}

		//if there is already a gameObject occupying wallSegment1
		else if (clickScript.oneWallSegmentClicked == true) 
		{
			//if the clicked wall is grey
			if (gameObject.transform.renderer.material.color == Color.grey)
			{
				//pass it in as segment 2
				clickScript.selectWallSegment2(gameObject);
			}

			//otherwise, give feedback about invalid placement
			else if (gameObject.transform.renderer.material.color == clickScript.wallColor)
			{
				clickScript.playerFeedback = "Invalid Placement :O";
				StartCoroutine(changeBackPlayerFeedback());
			}
	
		}
	
		//Furthers player feedback, when 1 of the two wall segments is slected
		if (clickScript.wallSegment1 != null && gameObject.transform.renderer.material.color == clickScript.wallColor) 
		{
			clickScript.playerFeedback = "Select A Valid 2nd Segement :D";
		}

		//Furthers player feedback of invalid placement: if gameObject is already brown
		else if (clickScript.wallSegment1 != null 
			&& gameObject.transform.renderer.material.color == clickScript.wallColor
			&& clickScript.playerFeedback == "Select A Valid 2nd Segement :D")
		{
			clickScript.playerFeedback = "Invalid Placement :O";
			StartCoroutine(changeBackPlayerFeedback());
		}
	}

	//Coroutine to change playerFeedback back to default
	IEnumerator changeBackPlayerFeedback ()
	{
		yield return new WaitForSeconds (3.0f);
		clickScript.playerFeedback = "Select A Move :)"; 
		clickScript.coRoutineFeedbackActive = true;
		
	}
}
