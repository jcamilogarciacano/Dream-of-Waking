using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
public class InventoryMenu : MonoBehaviour {

	//Scene scene = SceneManager.GetActiveScene();
	// Use this for initialization
	bool Pause = true;
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown("Inv") ){
			Pause = !Pause;
			Time.timeScale = (Pause) ? 1.00f : 0.00f;
			if (Pause==false) {
				SceneManager.LoadScene ("Inventory", LoadSceneMode.Additive);
			} else {
				SceneManager.UnloadSceneAsync ("Inventory");
			}

		}
	}

}
