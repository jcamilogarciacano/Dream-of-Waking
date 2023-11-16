using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
public class SceneGO : MonoBehaviour {
	public Scene sceneName;
	//bool Pause = false;
	public GameObject arrow;
	public Hits puntos;
	// Use this for initialization
	void Start () {
		if (SceneManager.sceneCount != 1) {
			sceneName = SceneManager.GetSceneAt (1);
		}
		if (sceneName.name == "Puzzle2") {
			puntos = GameObject.Find ("Main Camera 2").GetComponent<Hits> ();
		}
		print(sceneName.name);
	}
	// Update is called once per frame
	void Update () {
		bool b = CrossPlatformInputManager.GetButton("Cancel");
		if (puntos != null) {
			if (puntos.points == 16) {
				SceneManager.UnloadSceneAsync (sceneName);
				DontDestroyOnLoad (puntos);
				puntos.GetComponent<Camera> ().enabled = false;
			}
		}
			if (b==true) {
				if (arrow != null) {
					if (arrow.GetComponent<ArrowScript> ().openDoor == true) {
						DontDestroyOnLoad (arrow);
					}
				}
				SceneManager.UnloadSceneAsync (sceneName);
				//Time.timeScale = (Pause) ? 1.00f : 0.00f;
			}
		
	}
}
