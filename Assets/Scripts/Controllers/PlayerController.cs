using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public GameObject wakeUp;
    public PlayerTeleporterBetweenMatrix reseter;
    public KeyItemsProgress keyItems;
    public Show_bullets bulletsInfo;

    [SerializeField]
    public static float playerLife = 200;
    //to update the hearts based on player Life
    public PlayerLives hearts;
    int hearts_counter = 1;
    bool deleteH;
    public SpriteRenderer dmgReceived;

    public Text comentaryT;
    public TMP_Text tutorialText;
    public RawImage btnImage;
    public bool mapPicked;

    [SerializeField]
    private float m_MaxSpeed = 3f;
    private Animator m_Anim;            // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    private bool timer = true;
    public EnemyController enemyC;
    public EnemyControllerSplit enemyCS;
    private CapsuleCollider2D enemyCollider;
    private int lives = 3;
    int tiempo;
    AudioSource disparo;
    public AudioSource voice;
    public GameObject splash;
    private Light destello;
    ScreenplayController sPC;
    //string[] dialogos;
    bool hasWeapon;

    SpriteRenderer map_SpriteRenderer;
    SpriteRenderer key_map_SpriteRenderer;
    public GameObject mapSprite;
    public GameObject keyMapSprite;

    public bool hasShotgun;
    //public ItemPickedUp Key;

    public string[] texts = { "Y", "X"};
    public Color32[] colors = { new Color32(255, 236, 0, 255), new Color32(0, 170, 255, 255) };

    public float pGL420 = 0;

    //bool sound = false;
    //int si = 0;

    
    private void Awake()
    {
        try
        {
            if(dmgReceived!= null)
                dmgReceived.enabled = false;
            tutorialText = GameObject.FindGameObjectWithTag("Tutorial").GetComponent<TMP_Text>();
            btnImage = GameObject.FindGameObjectWithTag("Info").GetComponent<RawImage>();
            // new, i assign this via the inspector.
            bulletsInfo = GameObject.Find("BulletsInfo").GetComponent<Show_bullets>();
            // try catch to get voice new.
            voice = GameObject.Find("Ahhh").GetComponent<AudioSource>();

        }
        catch(Exception e)
        {
            print("no info or voice text " + e);
        }
        //	enemyC = GameObject.FindWithTag ("Enemy").GetComponent<EnemyController> ();
        disparo = GetComponent<AudioSource>();
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        if (mapSprite != null)
        {
            map_SpriteRenderer = mapSprite.GetComponent<SpriteRenderer>();
            key_map_SpriteRenderer = keyMapSprite.GetComponent<SpriteRenderer>();
        }
        StartCoroutine(WaitForDamage(1f));
        destello = GameObject.Find("/Player/Spotlight").GetComponent<Light>();

        // changing voice to try /catch
        if (voice != null)
        {
           
        }
        sPC = GetComponent<ScreenplayController>();
        if (sPC == null)
        {
            sPC = new ScreenplayController();
            hasWeapon = true;
        }
        timer = true;
        deleteH = false;
    }

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        try
        {
            // new, i assign this via the inspector.
            bulletsInfo = GameObject.Find("BulletsInfo").GetComponent<Show_bullets>();
            if(bulletsInfo.ammoText != null)
            bulletsInfo.ammoText.text = bulletsInfo.ammo.ToString();
        }
        catch (Exception)
        {
            print("no bullets data");
            throw;
        }
    }
    private void Start()
    {
        try
        {
            // new, i assign this via the inspector.
            bulletsInfo = GameObject.Find("BulletsInfo").GetComponent<Show_bullets>();
            if (bulletsInfo.ammoText != null)
                bulletsInfo.ammoText.text = bulletsInfo.ammo.ToString();
        }
        catch (Exception)
        {
            print("no bullets data");
            throw;
        }
        Physics2D.IgnoreLayerCollision(8, 10, true);
    }
    // Update is called once per frame
    void FixedUpdate()
    {

       // print(playerLife);
        //lifeT.text = playerLife.ToString();
        float h = CrossPlatformInputManager.GetAxis("Vertical");
        float j = CrossPlatformInputManager.GetAxis("Horizontal");
        float k = CrossPlatformInputManager.GetAxis("Mouse ScrollWheel");
        bool aiming = CrossPlatformInputManager.GetButton("Fire1"); // lb on xbox
        bool a = CrossPlatformInputManager.GetButton("Fire2"); // RB on xbox   A-B on xbox
        bool mapa = CrossPlatformInputManager.GetButton("Map"); //Y on xbox and PC
        m_Anim.SetFloat("DirectionX", h);
        m_Anim.SetFloat("DirectionY", j);
        if (sPC.gunActive == true || hasWeapon == true)
        {
            m_Anim.SetBool("Aiming", aiming);
        }
        Vector2 m_direction;
        m_direction = new Vector2(j, h);

        if (playerLife <= 0)
        {
            // WE HAVE TO SET LIFE AGAIN T0 100 TO NOT END UP IN A LOOP// DOES NOT WORK, PLAYER DOES NOT DIE
           

            SceneManager.LoadScene("Boss", LoadSceneMode.Single);
            playerLife = 200;
            Destroy(this.gameObject);
        }
        if (sPC.gunActive == true || hasWeapon == true)
        {
            Destroy(GameObject.Find("FirstAct"));
            if (aiming == true)
            {
                Aiming(h, j, a, m_direction);
            }
        }
        if (m_Anim.GetBool("Aiming") == false)
        {
            Move(h, j);
        }
        if (mapSprite != null)
        {
            
            if (mapa == true && mapPicked == true)
            {
                // para quitar el texto informativo de como abrir el mapa la primera vez q se usa.
                //////////////////// infoText.enabled = false;
                if (btnImage != null)
                    btnImage.enabled = false;
                if (tutorialText != null)
                    tutorialText.enabled = false;
                Time.timeScale = 0.1f;
                map_SpriteRenderer.enabled = true;
                if (reseter.key.CheckItemIsPicked()!= true)
                {
                    key_map_SpriteRenderer.enabled = true;
                }
            }
            else
            {
                Time.timeScale = 1.0f;
                map_SpriteRenderer.enabled = false;
                key_map_SpriteRenderer.enabled = false;
            }
        }
        pGL420 += Time.fixedDeltaTime;
        if (pGL420 > 15)
        {
            m_Anim.SetInteger("PGL", (int)pGL420);
            //print ("pegueloo");
            pGL420 = 0;
        }
        //print (pGL420);
    }
    public string Comentary()
    {
        string text1 = "necesito buscar un refugio";
        /*string text2 = "¡qué frío hace!";
        string text3 = "¿estoy soñando?";
        string text4 = "una llave, ¿de donde será?";
        string text5 = "no creo poder aguantar más, muero de frio";
        string text6 = "¿donde estoy?";
        string text7 = "¿que ha pasado?, ¿y la llave?";
        string text8 = "¡¿hola?!, ¿alguien?";*/

        return text1;
    }
    public void Move(float move, float move2)
    {

        if (move != 0f || move2 != 0f)
        {
            pGL420 = 0;
            m_Anim.SetInteger("PGL", (int)pGL420);

        }
        m_Anim.SetFloat("Speed", Mathf.Abs(move));
        m_Anim.SetFloat("Speed2", Mathf.Abs(move2));
        m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);
        m_Rigidbody2D.velocity = new Vector2(move2 * m_MaxSpeed, m_Rigidbody2D.velocity.x);
        m_Anim.SetFloat("Velocity", m_Rigidbody2D.velocity.x);
        m_Anim.SetFloat("Velocity2", m_Rigidbody2D.velocity.y);
        //print (m_Rigidbody2D.velocity);
    }
    public void Aiming(float move, float move2, bool a, Vector2 m_direction)
    {
        pGL420 = 0;
        m_Anim.SetInteger("PGL", (int)pGL420);
        m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed * 0, m_Rigidbody2D.velocity.y);
        m_Rigidbody2D.velocity = new Vector2(move2 * m_MaxSpeed * 0, m_Rigidbody2D.velocity.x);
        if (a == true && timer == true && bulletsInfo.ammo > 0)
        {
            timer = false;
            m_Anim.SetTrigger("Shoot");
            Shoot(m_direction);
        }
    }
    public void Shoot(Vector2 direction)
    {
        print(direction + "direccion");
        // 10 because it is the enemyBody layer
        int layerMask = 1 << 10;
        //layerMask = layerMask;
        
       
        if (hasShotgun == true)
        {
            Vector3 gunPosition = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
            if (direction.x == 1 && direction.y == 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(gunPosition, direction, Mathf.Infinity, layerMask);
                Debug.DrawRay(gunPosition, direction, Color.green);
                OnHit(hit);
                RaycastShotgunY(gunPosition, direction, layerMask, 0.25f);
                RaycastShotgunY(gunPosition, direction, layerMask, -0.25f);

            }
            else if (direction.x == 0 && direction.y == 1)
            {
                RaycastHit2D hit = Physics2D.Raycast(gunPosition, direction, Mathf.Infinity, layerMask);
                Debug.DrawRay(gunPosition, direction, Color.green);
                OnHit(hit);
                RaycastShotgunX(gunPosition, direction, layerMask, 0.25f);
                RaycastShotgunX(gunPosition, direction, layerMask, -0.25f);

            }
            else if (direction.x == 1 && direction.y == 1)
            {
                RaycastHit2D hit = Physics2D.Raycast(gunPosition, direction, Mathf.Infinity, layerMask);
                Debug.DrawRay(gunPosition, direction, Color.green);
                OnHit(hit);
                RaycastShotgunX(gunPosition, direction, layerMask, 0.25f);
                RaycastShotgunX(gunPosition, direction, layerMask, -0.25f);
            }
            else if (direction.x == -1 && direction.y == 1)
            {
                RaycastHit2D hit = Physics2D.Raycast(gunPosition, direction, Mathf.Infinity, layerMask);
                Debug.DrawRay(gunPosition, direction, Color.green);
                OnHit(hit);
                RaycastShotgunX(gunPosition, direction, layerMask, 0.25f);
                RaycastShotgunX(gunPosition, direction, layerMask, -0.25f);
            }
            else if (direction.x == -1 && direction.y == 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(gunPosition, direction, Mathf.Infinity, layerMask);
                Debug.DrawRay(gunPosition, direction, Color.green);
                OnHit(hit);
                RaycastShotgunY(gunPosition, direction, layerMask, 0.25f);
                RaycastShotgunY(gunPosition, direction, layerMask, -0.25f);
            }
            else if (direction.x == -1 && direction.y == -1)
            {
                RaycastHit2D hit = Physics2D.Raycast(gunPosition, direction, Mathf.Infinity, layerMask);
                Debug.DrawRay(gunPosition, direction, Color.green);
                OnHit(hit);
                RaycastShotgunX(gunPosition, direction, layerMask, 0.25f);
                RaycastShotgunX(gunPosition, direction, layerMask, -0.25f);

            }
            else if (direction.x == 0 && direction.y == -1)
            {
                RaycastHit2D hit = Physics2D.Raycast(gunPosition, direction, Mathf.Infinity, layerMask);
                Debug.DrawRay(gunPosition, direction, Color.green);
                OnHit(hit);
                RaycastShotgunX(gunPosition, direction, layerMask, 0.25f);
                RaycastShotgunX(gunPosition, direction, layerMask, -0.25f);

            }
            else if (direction.x == 1 && direction.y == -1)
            {
                RaycastHit2D hit = Physics2D.Raycast(gunPosition, direction, Mathf.Infinity, layerMask);
                Debug.DrawRay(gunPosition, direction, Color.green);
                OnHit(hit);
                RaycastShotgunX(gunPosition, direction, layerMask, 0.25f);
                RaycastShotgunX(gunPosition, direction, layerMask, -0.25f);

            }

        }
        else
        {
            Vector3 gunPosition = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
            RaycastHit2D hit = Physics2D.Raycast(gunPosition, direction, Mathf.Infinity, layerMask);
            Debug.DrawRay(gunPosition, direction, Color.green);
            OnHit(hit);
        }
        //print(m_Anim.);
        destello.spotAngle += 30;
        disparo.Play();
        destello.color = Color.yellow;
        //bullet information
        bulletsInfo.ammo--;
        bulletsInfo.ammoText.text = bulletsInfo.ammo.ToString();

        StartCoroutine(WaitForRecoil(0.5f));
    }
    void RaycastShotgunX(Vector3 position, Vector2 direction, int layerMask, float value)
    {
        Vector2 direction2 = new Vector2(direction.x + value, direction.y);
        RaycastHit2D hit2 = Physics2D.Raycast(position, direction2, Mathf.Infinity, layerMask);
        Debug.DrawRay(position, direction2, Color.red);
        OnHit(hit2);
    }
    void RaycastShotgunY(Vector3 position, Vector2 direction, int layerMask, float value)
    {
        Vector2 direction2 = new Vector2(direction.x, direction.y + value);
        RaycastHit2D hit2 = Physics2D.Raycast(position, direction2, Mathf.Infinity, layerMask);
        Debug.DrawRay(position, direction2, Color.red);
        OnHit(hit2);
    }
        void OnHit( RaycastHit2D hit)
    {
        if (hit.collider != null)
        {
            //enemyC =hit.collider.gameObject.GetComponent<EnemyController> ();
            print(hit.transform.tag);
            if (hit.transform.tag == "enemyBody")
            {
                enemyCS = hit.transform.gameObject.GetComponent<EnemyControllerSplit>();
                enemyC = hit.transform.gameObject.GetComponent<EnemyController>();
                ReceiveDmg enemyHitted = hit.transform.gameObject.GetComponent<ReceiveDmg>();
                if (enemyC != null)
                {
                    enemyC.RecibirDanio();
                    StartCoroutine(WaitForKill(enemyC.deathTimer, enemyC));
                    enemyC.EnemyStaggered();
                    //Instantiate (splash, enemyC.transform);
                }
                if (enemyCS != null)
                {
                    enemyCS.enemyLife = enemyCS.enemyLife - 50;
                    //StartCoroutine(WaitForKill(0.5f,enemyC));
                    //enemyCS.EnemyStaggered(1);
                    //Instantiate (splash, enemyC.transform);
                }
                if( enemyHitted != null)
                {
                    enemyHitted.gotHit(this.gameObject.transform);
                    //enemyHitted.
                }
                print("le di");
                print(timer);
            }
            if (hit.transform.tag == "ratBody")
            {
                enemyC = hit.transform.gameObject.GetComponent<EnemyController>();
                if (enemyC != null)
                {
                    enemyC.RecibirDanio();
                    StartCoroutine(WaitForKill(enemyC.deathTimer, enemyC));

                    //Instantiate (splash, enemyC.transform);
                }
            }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") == true)
        {
            print("auch");
            voice.Play();
            RestartDream();

            Time.timeScale = 1.0f;
            if (map_SpriteRenderer != null)
                map_SpriteRenderer.enabled = false;
        }
        else
        {

        }

        //m_Rigidbody2D.AddForce( new Vector2 (-col.transform.position.x, -col.transform.position.y));
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") == true)
        {
            //voice.Play();
            print("violadoo");
            RestartDream();
            Time.timeScale = 1.0f;
            if(map_SpriteRenderer != null)
            map_SpriteRenderer.enabled = false;
        }
    }

    //a falta de vida, mecanica innovadora incoming  TODO: player repetly re-spawns in the game as part of the dream Experience.
    public void RestartDream()
    {
        
        if (SceneManager.GetActiveScene().name == "DarkPlace")
        {
            SceneManager.LoadScene("Testing", LoadSceneMode.Single);

        }
        else if (SceneManager.GetActiveScene().name == "Boss")
        {
            ReceiveDamage();
        }
        else if (SceneManager.GetActiveScene().name == "Underworld")
        {
            ReceiveDamage(SceneManager.GetActiveScene().name);
        }
        else
        {
            // this is for testing scene
            this.gameObject.transform.position = new Vector2(0f, 0f);
            reseter.TeleportPlayerRandom(0);
            reseter.TeleportKeyRandom(2);
            timer = true;
            wakeUp.SetActive(true);
            this.gameObject.SetActive(false);

        }
        // WAKE UP ANIMATION RESTARTS
        // THE SCENE IS NOT RESTARTED IT CONTINUES ACTIVES
    }
    public void ReceiveDamage(string sceneName)
    {
        StartCoroutine(WaitForDamage(1.5f));
        m_Rigidbody2D.AddForce(new Vector2(UnityEngine.Random.Range(-2,2), UnityEngine.Random.Range(-2, 2)));
    
            playerLife = playerLife - 20;
            // hearts update
            if (deleteH == true)
            {
                hearts._hearts[hearts._hearts.Length - hearts_counter].enabled = false;
                hearts_counter++;

            }
            else
            {

                hearts._hearts[hearts._hearts.Length - hearts_counter].texture = hearts.half_heart;
            }
            deleteH = !deleteH;
        
    }

        //este no lo necesitaremos para la version mobile. demasiado problematico.
        public void ReceiveDamage()
    {
        //recibioDanio = true;
        StartCoroutine(WaitForDamage(1.5f));
        //m_Rigidbody2D.AddForce (new Vector2(-2,-2));
        if (enemyCS != null )
        {
            playerLife = playerLife - 20;
            // hearts update
            if (deleteH == true)
            {
                hearts._hearts[hearts._hearts.Length - hearts_counter].enabled = false;
                hearts_counter++;

            }
            else
            {
                
                hearts._hearts[hearts._hearts.Length - hearts_counter].texture = hearts.half_heart;
            }
            deleteH = !deleteH;
        }       //enemyCollider = enemyC.GetComponent<CapsuleCollider2D> ();

        if (true)
        {
            //	enemyCollider.isTrigger = true;
        }//m_Rigidbody2D.velocity = new Vector2(-move*m_MaxSpeed, -m_Rigidbody2D.velocity.y);
         //m_Rigidbody2D.velocity = new Vector2(-move2*m_MaxSpeed, -m_Rigidbody2D.velocity.x);
    }
    public IEnumerator WaitForDamage(float waitTime)
    {
        if(dmgReceived!= null)
        dmgReceived.enabled = true;

        Physics2D.IgnoreLayerCollision(8, 9, true);
        yield return new WaitForSeconds(waitTime);
        // this is only for the not enemy body collider, putting enemyBody on another Layer for it to not mess with the player collider and kick him out of the arena
        Physics2D.IgnoreLayerCollision(8, 9, false);
        if (dmgReceived != null)
            dmgReceived.enabled = false;
        //	timer = true;

    }
    private IEnumerator WaitForKill(float waitTime, EnemyController enemyC_)
    {
        if(enemyC.attackCollider != null)
        enemyC.attackCollider.enabled = false;

        enemyC.EnemyStaggered();
        yield return new WaitForSeconds(waitTime);
        enemyC_.enemyLife = enemyC_.enemyLife - 1f;

        if(enemyC != null)
        enemyC.EnemyRecovered();
        //	timer = true;

    }

    private IEnumerator WaitForRecoil(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        destello.color = Color.white;
        while (destello.spotAngle > 30)
        {
            destello.spotAngle--;
        }
        timer = true;

    }
}
