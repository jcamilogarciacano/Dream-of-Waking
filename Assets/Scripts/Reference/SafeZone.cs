using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour {
	public GameObject ballena;
	public int i;
	// Use this for initialization
	void Start () {
		ballena.SetActive (false);
		i = 0;
	}
	
	// Update is called once per frame
	void Update () {
		i++;
		if (i>10) {
			ballena.SetActive (true);
		}
	}
	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag.Equals("mina") || other.tag.Equals("barco") || other.tag.Equals("tiburon")  ) {			
			Destroy (other.gameObject);
			print ("objeto destruido en zona segura");
		}
	}
	void OnTriggerStay2D (Collider2D other) {
		if (other.tag.Equals("mina") || other.tag.Equals("barco") || other.tag.Equals("tiburon")  ) {			
			Destroy (other.gameObject);
			print ("objeto destruido en zona segura");
		}
	}

}
