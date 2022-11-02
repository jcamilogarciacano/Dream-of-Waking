using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
	public GameObject wakeUp;
	public PlayerTeleporterBetweenMatrix reseter;
	public KeyItemsProgress keyItems;
	public static float playerLife = 100 ;
	public Text comentaryT;
	private float m_MaxSpeed = 3f;            
	private Animator m_Anim;            // Reference to the player's animator component.
	private Rigidbody2D m_Rigidbody2D;
	private bool timer=true;
	public EnemyController enemyC;
	public EnemyControllerSplit enemyCS;
	private CapsuleCollider2D enemyCollider;
	private int lives=3;
	int tiempo;
	AudioSource disparo;
	AudioSource voice;
	public GameObject splash;
	private Light destello;
	ScreenplayController sPC;
	string[] dialogos;
	bool hasWeapon;
	SpriteRenderer m_SpriteRenderer;
	public GameObject mapSprite;

	public float pGL420=0;

	bool sound= false;
	int si =0;
	private void Awake(){
	//	enemyC = GameObject.FindWithTag ("Enemy").GetComponent<EnemyController> ();
		m_Anim = GetComponent<Animator>();
		disparo = GetComponent<AudioSource> ();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		if(mapSprite != null){
		m_SpriteRenderer = mapSprite.GetComponent<SpriteRenderer>();
		}StartCoroutine(WaitForDamage (1f));
		destello = GameObject.Find ("/Player/Spotlight").GetComponent<Light>();
		if (voice != null)
		{
			voice = GameObject.Find("Ahhh").GetComponent<AudioSource>();
		}
		sPC = GetComponent<ScreenplayController> ();
		if (sPC==null) {
			sPC = new ScreenplayController();
			hasWeapon = true;
		}
		timer =true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		//lifeT.text = playerLife.ToString();
		float h = CrossPlatformInputManager.GetAxis("Vertical");
		float j = CrossPlatformInputManager.GetAxis("Horizontal");
		float k = CrossPlatformInputManager.GetAxis("Mouse ScrollWheel");
		bool aiming = CrossPlatformInputManager.GetButton("Fire1");
		bool a = CrossPlatformInputManager.GetButton("Fire2");
		bool mapa = CrossPlatformInputManager.GetButton("Fire3");
	
		m_Anim.SetFloat ("DirectionX",h);
		m_Anim.SetFloat ("DirectionY", j);
		if (sPC.gunActive == true || hasWeapon==true) {
		m_Anim.SetBool ("Aiming", aiming);
		}
		Vector2 m_direction;
		m_direction = new Vector2 (j, h);

		if (playerLife<=0) {
			SceneManager.LoadScene ("Boss", LoadSceneMode.Single);
			Destroy (this.gameObject);	
		}
		if (sPC.gunActive == true || hasWeapon == true) {
			Destroy(GameObject.Find ("FirstAct"));
			if (aiming == true) {
				Aiming (h, j, a, m_direction);
			}
		}
		if (m_Anim.GetBool ("Aiming") == false) {
			Move (h, j);
		}
		if(mapSprite != null){
		if (mapa == true && GameObject.Find("Map")!=null){
			Time.timeScale = 0.1f;
			m_SpriteRenderer.enabled =true;
		}
		else{
			Time.timeScale = 1.0f;
			m_SpriteRenderer.enabled =false;
			}
		}
		pGL420 += Time.fixedDeltaTime;
		if(pGL420 > 9){
			m_Anim.SetInteger ("PGL", (int)	pGL420);
			//print ("pegueloo");
			pGL420 = 0;
		}
		//print (pGL420);
	}
	public string Comentary(){
		string text1 = "necesito buscar un refugio";
		string text2 = "¡qué frío hace!";
		string text3 = "¿estoy soñando?";
		string text4 = "una llave, ¿de donde será?";
		string text5 = "no creo poder aguantar más, muero de frio";
		string text6 = "¿donde estoy?";
		string text7 = "¿que ha pasado?, ¿y la llave?";
		string text8 = "¡¿hola?!, ¿alguien?";

		return text1;
	}
	public void Move(float move,float move2)
	{
		
		if(move !=0f  || move2 !=0f){
			pGL420 = 0;
			m_Anim.SetInteger ("PGL", (int)	pGL420);

		}
		m_Anim.SetFloat("Speed", Mathf.Abs(move));
		m_Anim.SetFloat("Speed2", Mathf.Abs(move2));
		m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);
		m_Rigidbody2D.velocity = new Vector2(move2*m_MaxSpeed, m_Rigidbody2D.velocity.x);
		m_Anim.SetFloat ("Velocity", m_Rigidbody2D.velocity.x);
		m_Anim.SetFloat ("Velocity2", m_Rigidbody2D.velocity.y);
		//print (m_Rigidbody2D.velocity);
	}
	public void Aiming(float move,float move2,bool a,Vector2 m_direction)
	{
		pGL420 = 0;
		m_Anim.SetInteger ("PGL", (int)	pGL420);
		m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed*0, m_Rigidbody2D.velocity.y);
		m_Rigidbody2D.velocity = new Vector2(move2*m_MaxSpeed*0, m_Rigidbody2D.velocity.x);
		if (a == true && timer==true) {
			timer = false;
			m_Anim.SetTrigger ("Shoot");
			Shoot (m_direction);
		}
	}
	public void Shoot(Vector2 direction){
		int layerMask = 1 << 9;
		layerMask= layerMask;
		RaycastHit2D hit = Physics2D.Raycast (transform.position, direction,Mathf.Infinity,layerMask);
		destello.spotAngle += 30;
		disparo.Play ();
		destello.color = Color.yellow;
		Debug.DrawRay (transform.position, direction, Color.green);

		if (hit.collider != null) {
			//enemyC =hit.collider.gameObject.GetComponent<EnemyController> ();
			print (hit.transform.tag);
			if (hit.transform.tag== "enemyBody") {
				enemyCS = hit.transform.gameObject.GetComponent<EnemyControllerSplit> (); 
				enemyC = hit.transform.gameObject.GetComponent<EnemyController> (); 
				if (enemyC != null) {
					enemyC.RecibirDanio ();
					StartCoroutine(WaitForKill(0.3f,enemyC));
					enemyC.EnemyStaggered(1);
					//Instantiate (splash, enemyC.transform);
				}
				if (enemyCS != null) {
					enemyCS.enemyLife =enemyCS.enemyLife -50 ;
					//StartCoroutine(WaitForKill(0.5f,enemyC));
					//enemyCS.EnemyStaggered(1);
					//Instantiate (splash, enemyC.transform);
				}
				print ("le di");
				print (timer);
			}
		}
		StartCoroutine(WaitForRecoil (0.5f));
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Enemy")==true) {
		print ("auch");
			voice.Play ();
			RestartDream ();
		}
		else{
			
		}
		
		//m_Rigidbody2D.AddForce( new Vector2 (-col.transform.position.x, -col.transform.position.y));
	}
	void OnTriggerStay2D(Collider2D col)
	{
		if (col.CompareTag("Enemy")==true) {
			print ("violadoo");
			RestartDream ();
		}
	}

//a falta de vida, mecanica innovadora incoming  TODO: player repetly re-spawns in the game as part of the dream Experience.
public void RestartDream(){
		if(SceneManager.GetActiveScene().name =="DarkPlace"){
			SceneManager.LoadScene ("Testing", LoadSceneMode.Single);

		}
		else if(SceneManager.GetActiveScene().name =="Boss"){
			ReceiveDamage();
		}
		else{
			
	this.gameObject.transform.position = new Vector2 (0f,0f);
	reseter.TeleportPlayerRandom(0);
	reseter.TeleportKeyRandom(2);	
			timer =true;
		wakeUp.SetActive(true);
	this.gameObject.SetActive(false);

		}
	// WAKE UP ANIMATION RESTARTS
	// THE SCENE IS NOT RESTARTED IT CONTINUES ACTIVES
}
	//este no lo necesitaremos para la version mobile. demasiado problematico.
	public void ReceiveDamage( ){
		//recibioDanio = true;
		StartCoroutine (WaitForDamage (1.5f));
		//m_Rigidbody2D.AddForce (new Vector2(-2,-2));
		if (enemyCS != null) {
			playerLife = playerLife - 20;
		}		//enemyCollider = enemyC.GetComponent<CapsuleCollider2D> ();
		
		if (true) {
		//	enemyCollider.isTrigger = true;
		}//m_Rigidbody2D.velocity = new Vector2(-move*m_MaxSpeed, -m_Rigidbody2D.velocity.y);
		//m_Rigidbody2D.velocity = new Vector2(-move2*m_MaxSpeed, -m_Rigidbody2D.velocity.x);
	}
	public IEnumerator WaitForDamage(float waitTime){
	Physics2D.IgnoreLayerCollision(8, 9,true);
		yield return new WaitForSeconds (waitTime);
		Physics2D.IgnoreLayerCollision(8, 9,false);
	//	timer = true;

	}
	private IEnumerator WaitForKill(float waitTime, EnemyController enemyC_){
		enemyC.attackCollider.enabled = false;
		yield return new WaitForSeconds (waitTime);
		enemyC_.enemyLife = enemyC_.enemyLife - 1f;
	//	timer = true;

	}

	private IEnumerator WaitForRecoil(float waitTime){
		
		yield return new WaitForSeconds (waitTime);
		destello.color = Color.white;
		while (destello.spotAngle>30) {
			destello.spotAngle--;
		}
		timer = true;

	}
}
