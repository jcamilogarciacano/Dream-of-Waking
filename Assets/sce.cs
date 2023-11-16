using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sce : StateMachineBehaviour

{
    public string sceneName;
    public SpriteRenderer sprite;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (SceneManager.GetActiveScene().name == "DogIntroduction")
        {
            sprite = GameObject.Find("Cinematic1").GetComponent<SpriteRenderer>();
            sprite.enabled = false;
        }else if(SceneManager.GetActiveScene().name == "Introduction")
        {
            sprite = GameObject.Find("Cinematic2").GetComponent<SpriteRenderer>();
            sprite.enabled = false;
        }
        else if (SceneManager.GetActiveScene().name == "HighWay")
        {
            sprite = GameObject.Find("Cinematic3").GetComponent<SpriteRenderer>();
            sprite.enabled = false;
        }
        if (SceneManager.GetActiveScene().name == "IntroScene")
        {
            if (GameObject.Find("DontDestroyOnLoad") != null)
            {
                SceneManager.LoadScene("Introduction", LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
        }
        else if (SceneManager.GetActiveScene().name == "OutSide")
        {
            if (GameObject.Find("House_Key") != null)
            {
                SceneManager.LoadScene("Puente_02", LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
        }
        else
        {
            //load next scene typed on the field sceneName, when exit anim
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);


        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
