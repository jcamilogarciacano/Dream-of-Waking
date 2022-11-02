﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafezoneUntilDawn : MonoBehaviour
{
	public GameObject safeZone;
	private Collider2D sf2D;
	public int i;
    // Start is called before the first frame update
    void Start()
    {
		sf2D= safeZone.GetComponent<Collider2D>();
    }
	void EnableSafeZone(){
		print("estoy segura!");
		sf2D.enabled= true;
	}
	void DisableSafeZone(){
		sf2D.enabled= false;
	}
    // Update is called once per frame
    void FixedUpdate()
    {
		if(i > 0){
			EnableSafeZone();
			i++;
			if(i>100){
				DisableSafeZone();
				i=0;
			}
		}
    }
	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag=="Enemy" || other.tag=="enemyBody") {			
			Destroy (other.gameObject);
			print ("objeto destruido en zona segura");
		}
	}
	void OnTriggerStay2D (Collider2D other) {
		if (other.tag=="Enemy" || other.tag=="enemyBody" ) {			
			Destroy (other.gameObject);
			print ("objeto destruido en zona segura");
		}
	}
}