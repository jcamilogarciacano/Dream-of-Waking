using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnLeave : MonoBehaviour {

	public GameObject target;   
	public AudioSource Sound;
	public GameObject soundObject;
	int Unico = 0;
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (Unico==1) {
			soundObject.transform.Translate(Vector2.left * Time.deltaTime*10);
			Destroy (soundObject, 3);
			Destroy (this.gameObject.GetComponent<PlaySoundOnLeave>(), 3);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		
		if (Unico==0) {
			soundObject.GetComponent<AudioSource>().Play ();

			Unico = 1;
		}

	}
}
