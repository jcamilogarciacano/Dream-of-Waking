using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertLayer : MonoBehaviour {

	public DoorsInteractive inD;
	public DoorsInteractive outD;

	SpriteRenderer thisSprite;
	bool isInverted;

	// Use this for initialization
	void Start () {
		isInverted=false;
		thisSprite = this.gameObject.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void Invert(SpriteRenderer playerS, GameObject detecter){
		if (detecter.name == "DetecterOut") {

			thisSprite.sortingOrder = 2;
			playerS.sortingOrder = 3;
			isInverted= !isInverted;
		} else {
			
			thisSprite.sortingOrder = 3;
			playerS.sortingOrder = 2;
			isInverted= !isInverted;
		}
	}
}
