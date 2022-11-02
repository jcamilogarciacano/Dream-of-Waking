using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenScript : MonoBehaviour {


    public Transform positionCorrect;
    public bool checkCorrectPosition;

    // Use this for initialization
    void Start() {
        checkCorrectPosition = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (transform.position == positionCorrect.transform.position) {
            print("correctPosition");
            checkCorrectPosition = true;
        }
		
	}
}
