using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class EnemyControllerSplit : MonoBehaviour {
	public int enemyLife=600;

	private float m_MaxSpeed = 3f;            
	private Animator m_Anim;            // Reference to the player's animator component.
	private Rigidbody2D m_Rigidbody2D;
	private GameObject player;
	private Vector2 pPosition;
	private Vector2 attackPosition;
	private int i;
	private float w;
	private float e;
	private bool esquivar = false;
	public int esqVal =0;
	public bool atacar =false;
    public GameObject exitBoss;
	public GameObject exitEffect;
	public HealthBar lifeBar;
	public GameObject healthBar;
	public string sceneNameIfBugged;

	public TMP_Text youWinText;

	// Use this for initialization
	private void Awake(){
		player = GameObject.Find ("Player");
        exitBoss.SetActive(true);
        m_Anim = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		lifeBar.enabled = true;
		//lifeBar = GameObject.FindGameObjectWithTag("BossLife").GetComponent<Image>();
	}

	// Update is called once per frame
	void FixedUpdate () {
		
		if (esquivar == true) {
			pPosition = new Vector2 (player.transform.position.x + EsquivarCentroX(), player.transform.position.y+EsquivarCentroY());
		} else {
			pPosition = new Vector2 (player.transform.position.x , player.transform.position.y);
		}
		//	
		float h = pPosition.x - transform.position.x;
		float j = pPosition.y - transform.position.y;
		if (atacar == true) {
			Attack2 (h, j);
		} else {
			if (esquivar ==true) {

			}
			Move (h, j);
		}

		i++;
	}
    private void LateUpdate()
    {
		if (enemyLife <= 0)
		{
			healthBar.SetActive(false);
			exitBoss.SetActive(false);
			exitEffect.SetActive(true);
			youWinText.enabled = true;
			youWinText.GetComponent<DestroyThisObject>().DestroyThisGameobject();
			Destroy(GameObject.Find("Music"));
			GameObject.Find("ExitTrigger").GetComponent<ChangeScene>().CambioDeScena(sceneNameIfBugged);
			Destroy(this.gameObject);

		
		}
	}
	public IEnumerator WaitIfBugged(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		SceneManager.LoadScene(name, LoadSceneMode.Single);
	}
	public void Move(float move,float move2)
	{
		m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed*0, m_Rigidbody2D.velocity.y);
		m_Rigidbody2D.velocity = new Vector2(move2*m_MaxSpeed*0, m_Rigidbody2D.velocity.x);
		m_Anim.SetFloat("Speed", move);
		m_Anim.SetFloat("Speed2", move2);
	
			transform.position = Vector2.MoveTowards (new Vector2 (transform.position.x,
				transform.position.y), pPosition, 1f * Time.deltaTime);
		
		
		
			transform.position = Vector2.MoveTowards (new Vector2 (transform.position.x,
				transform.position.y), pPosition, 1f * Time.deltaTime);
		
		m_Anim.SetFloat ("Velocity", m_Rigidbody2D.velocity.x);
		m_Anim.SetFloat ("Velocity2", m_Rigidbody2D.velocity.y);
		attackPosition = pPosition;
		int random = Random.Range (1, 200);
		//print (random);
		if (random<3) {
			atacar = true;
		}
		//print (m_Rigidbody2D.velocity);
	}
	public void Attack2(float move, float move2){
		transform.position = Vector2.MoveTowards (new Vector2 (transform.position.x,
			transform.position.y), attackPosition, 3f * Time.deltaTime);
		if (transform.position.x==attackPosition.x || i>150) {
			atacar = false;
			i = 0;
		}
	}
	void OnCollisionEnter2D(Collision2D col){
	//	print ("esquivando");
		if (col.gameObject.tag == "Centro") {
			
			//print ("esquivando");
			esquivar = true;
		}
	}
	void OnCollisionExit2D(Collision2D col){
	//	print ("esquivando");

		if (col.gameObject.tag == "Centro") {
			print ("esquivando");
		esqVal = 0;
		esquivar = false;
			}
	}
	public int EsquivarCentroX(){
		//print ("esquivando");
		if (pPosition.x - transform.position.x < 0)
			return esqVal-=2;
		if (pPosition.x - transform.position.x > 0)
			return esqVal+=2;
		else {
			return 0;
		}
		//
	}
	public int EsquivarCentroY(){
		//print ("esquivando");
		if (pPosition.y - transform.position.y < 0)
			return esqVal-=2;
		if (pPosition.y - transform.position.y > 0)
			return esqVal+=2;
		else {
			return 0;
		}
		//
	}


}	
