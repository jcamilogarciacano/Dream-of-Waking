using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossIntro : StateMachineBehaviour {

	public GameObject girl;
	public GameObject splitHead;
	public GameObject oldSplitHead;
	public GameObject oldGirl;
	public GameObject camera;
	public GameObject oldCamera;


	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		girl = animator.gameObject.GetComponent<wakeUp>().girl;
		camera = animator.gameObject.GetComponent<wakeUp>().kamera;
		oldCamera = animator.gameObject.GetComponent<wakeUp>().oldCamera;
		oldGirl = animator.gameObject.GetComponent<wakeUp>().oldGirl;
		splitHead = animator.gameObject.GetComponent<wakeUp>().splitHead;
		oldSplitHead = animator.gameObject.GetComponent<wakeUp>().oldSplitHead;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		girl.SetActive (true);	
		
		if(oldSplitHead !=null)
		oldSplitHead.SetActive(false);

		if (splitHead != null){
		splitHead.SetActive (true);
}
		camera.SetActive (true);

		oldCamera.SetActive (false);

		Debug.Log ("hola");
		oldGirl.SetActive (false);
		//splitHead.GetComponent<EnemyController>().enabled = true;
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
