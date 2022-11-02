using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItself : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, 0.2f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
