using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController1 : MonoBehaviour {
    public Transform player;
    public float speed = 5.0f;
    private bool touchStart = false;
    private Vector2 pointA;
    private Vector2 pointB;
	private Rigidbody2D m_Rigidbody2D;
    public Transform circle;
    public Transform outerCircle;
	private Animator m_Anim;     	
private void Awake(){
	//	enemyC = GameObject.FindWithTag ("Enemy").GetComponent<EnemyController> ();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
				m_Anim = GetComponent<Animator>();
	}
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButtonDown(0)){
            pointA = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));

            circle.transform.position = pointA * -1;
            outerCircle.transform.position = pointA * -1;
            circle.GetComponent<SpriteRenderer>().enabled = true;
            outerCircle.GetComponent<SpriteRenderer>().enabled = true;
        }
        if(Input.GetMouseButton(0)){
            touchStart = true;
            pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
        }else{
            touchStart = false;
        }
        
	}
	private void FixedUpdate(){
        if(touchStart){
            Vector2 offset = pointB - pointA;
            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);
            Move(direction * -1);

            circle.transform.position = new Vector2(pointA.x + direction.x, pointA.y + direction.y) * -1;
        }else{
            circle.GetComponent<SpriteRenderer>().enabled = false;
            outerCircle.GetComponent<SpriteRenderer>().enabled = false;
        }

	}
	void moveCharacter(Vector2 direction){
        player.Translate(direction * speed * Time.deltaTime);
    }
    public void Move(Vector2 direction)
	{
		m_Rigidbody2D.velocity = direction;
		m_Anim.SetFloat ("Velocity", m_Rigidbody2D.velocity.x);
		m_Anim.SetFloat ("Velocity2", m_Rigidbody2D.velocity.y);
		//print (m_Rigidbody2D.velocity);
	}
}