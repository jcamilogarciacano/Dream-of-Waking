using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
	public AudioSource [] footStepSounds;
	public AudioSource footStepSound;
	public AudioClip step;
	public AudioClip[] steps;
	public GameObject player;
	public Transform pT;
	private Animator m_Anim;            // Reference to the player's animator component.
	float vel;
	int timerStep;
    // Start is called before the first frame update
    void Start()
    {
		timerStep = 0;
		 vel = player.GetComponent<Rigidbody2D> ().velocity.magnitude;
		m_Anim = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update ()
	{
		print (vel);
		if (m_Anim.GetFloat ("Speed") > 0.2f || m_Anim.GetFloat ("Speed2") > 0.2f) {
			if (timerStep > 6) {
				footStepSound.PlayOneShot (steps[(int)Random.Range(0f,3f)], 0.8f);
				timerStep = 0;
			}
		}
		timerStep++;
	}
}
		
