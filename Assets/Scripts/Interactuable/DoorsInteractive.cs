using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class DoorsInteractive : MonoBehaviour {
    public Transform pos;
    public GameObject target;
    //GameObject enemy;
    //bool Pause = true;
    public GameObject fog;
    //int turnoDelDialogo = 0;
	bool interactuable=false;
	//bool waitDoor =false;
	public SpriteRenderer playerR;
	public InvertLayer invL;

	int time;
    void Start()
    {
		playerR = target.GetComponent<SpriteRenderer>();

    }

	void FixedUpdate()
	{
		bool x = CrossPlatformInputManager.GetButton("Fire3");
		if (interactuable==true&&target!=null) {
			EnableTeleport (x);
		}

		if(time<200){
			time++;
		}
		else{
			time=0;
		}
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
		if (other.name==target.name) {
		interactuable = true;
        //EnableTeleport(other);
		}
    }
    private void OnTriggerStay2D(Collider2D other)
    {
     //   EnableText(other);
    }
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.name == target.name) {
			
			interactuable = false;
		}
	}

	private void EnableTeleport(bool x)
    {
   
		if (x ==true) {
			StartCoroutine (WaitForTransition (0.5f));
			StartCoroutine (Wait (0.2f));
			target.transform.position = pos.position;
		}
        
    }
   // private void EnableTeleport(Collider2D enemyC){
    //	if(interactuable==true && time<30){
      // enemyC.gameObject.transform.position = pos.position;
       //interactuable=false;
   //}
   //else{
   	//enemyC.gameObject.transform.position = new Vector2(target.transform.position.x-2,target.transform.position.y+1);
   //}				 // StartCoroutine (WaitForEnemy (1f));
    private IEnumerator WaitForTransition(float waitTime)
    {

        fog.SetActive(true);
		interactuable = false;
		yield return new WaitForSeconds(waitTime);
        fog.SetActive(false);

    }
	private IEnumerator Wait(float waitTime)
	{
		target.GetComponent<Collider2D> ().enabled = false;
		yield return new WaitForSeconds(waitTime);
		target.GetComponent<Collider2D> ().enabled = true;
	}
}
