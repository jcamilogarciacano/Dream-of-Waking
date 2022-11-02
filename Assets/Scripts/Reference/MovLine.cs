using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovLine : MonoBehaviour {
	public bool timeUp,Orientacion;
	float time,r;

	public int tiempo_linea, velocidad_linea;
	// Use this for initialization
	void Start () {
		timeUp = false;
		time = 0;

	
	}
	
	// Update is called once per frame
	void Update () {

		if (time >= tiempo_linea) {
			time = 0;
			//rotar la imagen
			r = transform.rotation.eulerAngles.z + 180f;
			transform.rotation = Quaternion.Euler(0, 0, r);

		} else {
			time++;

				transform.Translate (Vector2.up * Time.deltaTime * velocidad_linea);


		}




		/*
		
		time++;
		if (time >tiempo_linea) {
			transform.Translate (Vector2.up * Time.deltaTime * velocidad_linea);
			loquesea ();
		}
		else {
			transform.Translate (Vector2.down * Time.deltaTime * velocidad_linea);
			//timeUp= !timeUp;
		}


		if (time > (tiempo_linea*2)) {
			time = 0;
		}
		*/

	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag.Equals("mina") || other.tag.Equals("barco") || other.tag.Equals("tiburon")  ) {			
			Destroy (other.gameObject);
			print ("objeto destruido en mov linea");
		}
	}

	private void loquesea(){
		if (Orientacion == true) {
			r = transform.rotation.eulerAngles.z + 180f;
			transform.rotation = Quaternion.Euler(0, 0, r);
			Orientacion = false;
		}

	}

}
