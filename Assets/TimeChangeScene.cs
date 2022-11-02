using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
public class TimeChangeScene : MonoBehaviour {
	public string nombreDeScena;
	// Use this for initialization
	void Start () {
		StartCoroutine(WaitForTransition(15f));

	}
	
	// Update is called once per frame
	void Update () {
		}
	private IEnumerator WaitForTransition(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		SceneManager.LoadScene (nombreDeScena, LoadSceneMode.Single);
	}
}
