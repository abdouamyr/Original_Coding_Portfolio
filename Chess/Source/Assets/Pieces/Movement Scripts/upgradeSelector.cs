using UnityEngine;
using System.Collections;

public class upgradeSelector : MonoBehaviour {
	alphaUpgradeSelector upgradeControl;

	// Use this for initialization
	void Start () {
		upgradeControl = GameObject.Find("Alpha Upgrader").GetComponent<alphaUpgradeSelector>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown () {
		upgradeControl.selectType(gameObject);
	}
}
