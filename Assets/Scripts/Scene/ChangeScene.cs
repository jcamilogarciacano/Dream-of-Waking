using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class ChangeScene : MonoBehaviour {
	bool Pause = true;
	// Use this for initialization
	GameObject player;
	GameObject mainCamera;
	public void CambioDeScena  (string NombreDeScena) {
       // SceneManager.LoadScene("menu");

			if (NombreDeScena == "BossFight") {
		//		player = GameObject.Find ("Player");
			//DontDestroyOnLoad (player);
			//	mainCamera = GameObject.Find ("Main Camera");
			//	DontDestroyOnLoad (mainCamera);
			//	DontDestroyOnLoad(GameObject.Find("Canvas"));
			//	Pause = !Pause;
			//	Time.timeScale = (Pause) ? 1.00f : 0.00f;
				SceneManager.LoadScene (NombreDeScena, LoadSceneMode.Single);
//				player.transform.position = new Vector2 (0, 0);

			}
			if (NombreDeScena == "BossFight2") {
				SceneManager.LoadScene (NombreDeScena, LoadSceneMode.Single);
			}
			if (NombreDeScena == "BossFight3") {
				SceneManager.LoadScene (NombreDeScena, LoadSceneMode.Single);
			} 
			if (NombreDeScena == "Generator") {
				SceneManager.LoadScene (NombreDeScena, LoadSceneMode.Additive);
			}
			if (NombreDeScena == "Puzzle2") {
				SceneManager.LoadScene (NombreDeScena, LoadSceneMode.Additive);
			}
		}
		//Pause = true;
    
	
	
}
