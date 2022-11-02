using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc : StateMachineBehaviour {
	
	public GameObject girl;
	public GameObject splitHead;
	public GameObject oldGirl;
	public GameObject kamera;
	public GameObject oldCamera;


	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		girl = animator.gameObject.GetComponent<wakeUp>().girl;
		kamera = animator.gameObject.GetComponent<wakeUp>().kamera;
		oldCamera = animator.gameObject.GetComponent<wakeUp>().oldCamera;
		oldGirl = animator.gameObject.GetComponent<wakeUp>().oldGirl;
		splitHead = animator.gameObject.GetComponent<wakeUp>().splitHead;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		girl.SetActive (true);
		splitHead.SetActive (true);
		kamera.SetActive (true);

		oldCamera.SetActive (false);

		Debug.Log ("hola");
		oldGirl.SetActive (false);
	
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
