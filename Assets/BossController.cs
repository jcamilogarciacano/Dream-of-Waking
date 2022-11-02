using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController {
	// Use this for initialization
	private Animator m_Anim;        
	public bool followPlayer;
	private Vector2 attackPosition;
	private int i;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public override void Attack2(float move, float move2){
		if(followPlayer == true){
			m_Anim.SetTrigger ("Attaking");
			transform.position = Vector2.MoveTowards (new Vector2 (transform.position.x,
				transform.position.y), attackPosition, 3f * Time.deltaTime);
			if (transform.position.x==attackPosition.x || i>150) {
				atacar = false;
				i = 0;
			}
		}
	}
}
