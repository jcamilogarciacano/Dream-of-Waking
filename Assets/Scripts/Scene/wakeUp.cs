using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wakeUp : MonoBehaviour {
	public GameObject girl;
	public GameObject oldGirl;
	public GameObject kamera;
	public GameObject oldCamera;
	public GameObject splitHead;
	public Animator m_Anim;  
	// Use this for initialization
	void Start () {
		m_Anim.SetTrigger ("PlayerDied");
		oldGirl.SetActive(true);
		girl.SetActive(false);
		oldCamera.SetActive(true);
		kamera.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
