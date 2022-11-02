using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIntroduction : MonoBehaviour {
	//bool timer;
	public Camera camara;
	// Use this for initialization
	void Start () {
		camara.orthographicSize = 1;
		ZoomOut ();
	}

	// Update is called once per frame
	void FixedUpdate () {

	}
	private IEnumerator WaitForRecoil(float waitTime){
		yield return new WaitForSeconds (waitTime);
		camara.orthographicSize = (float)camara.orthographicSize + 0.1f;
		//timer = true;
		ZoomOut ();

	}
	public void ZoomOut (){
		if (camara.orthographicSize<3.61) {

			StartCoroutine(WaitForRecoil(0.1f));
		}
	}
}
