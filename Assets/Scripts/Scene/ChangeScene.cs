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
	public bool unStuck = false;
	public string sceneName;
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
		if (NombreDeScena == "IntroScene" )
		{
			StartCoroutine(WaitIfBugged(10f, NombreDeScena));
		}
	}

    private void FixedUpdate()
    {
		  
    }
    void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Player") == true)
		{
			if (SceneManager.GetActiveScene().name == "HighWay")
			{
				this.gameObject.GetComponent<PauseAnim>().enabled = true;
			}
            else
            {
				SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
			}

		}
		else
		{

		}

		//m_Rigidbody2D.AddForce( new Vector2 (-col.transform.position.x, -col.transform.position.y));
	}
	//Pause = true;

	public IEnumerator WaitIfBugged(float waitTime, string name)
	{
		yield return new WaitForSeconds(waitTime);
		SceneManager.LoadScene(name, LoadSceneMode.Single);
	}

}
