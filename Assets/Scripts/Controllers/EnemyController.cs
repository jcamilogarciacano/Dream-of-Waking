using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
public class EnemyController : MonoBehaviour
{

    public float enemyLife = 600;
    public float enemyDamage = 26;
    public GameObject reward;
    public float m_MaxSpeed = 3f;
    private Animator m_Anim;            // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    private GameObject player;
    private Vector2 pPosition;
    private Vector2 attackPosition;
    private SpriteRenderer mySriteRenderer;
    public CapsuleCollider2D attackCollider;
    private bool followPlayer;
    private int i;
    private float w;
    private float e;
    private int girar;
    public GameObject attackObject;
    int randomX;
    int randomY;
    private bool esquivar = false;
    private bool patrullar = false;
    private int esqVal = 0;
    public bool atacar = false;
    public bool recibirDanio = false;
    public bool moving;

    public float deathTimer = 0.3f;

    public DropLoot loot;
    // Use this for initialization
    private void Awake()
    {
        patrullar = true;
        player = GameObject.Find("Player");
        mySriteRenderer = GetComponent<SpriteRenderer>();
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        if (attackObject != null)
        {
            attackObject.SetActive(false);
        }
        Physics2D.IgnoreLayerCollision(9, 9, true);

        loot = GetComponent<DropLoot>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (SceneManager.GetActiveScene().name == "Testing")
        {
            if (this.gameObject.transform.position.x > 6f)
            {
                this.gameObject.transform.position = new Vector2(-5.9f, this.gameObject.transform.position.y);
            }
            if (this.gameObject.transform.position.x < -6f)
            {
                this.gameObject.transform.position = new Vector2(5.9f, this.gameObject.transform.position.y);
            }
            if (this.gameObject.transform.position.y > 3f)
            {
                this.gameObject.transform.position = new Vector2(-this.gameObject.transform.position.x, -2.9f);
            }
            if (this.gameObject.transform.position.y < -3f)
            {
                this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x, 2.9f);
            }
        }
       

           // enemyDamage = enemyDamage;
        if (enemyLife <= 0)
        {
            if (reward != null)
            {
                GameObject obj = Instantiate(reward, this.transform.position, this.transform.rotation);
                obj.name = reward.name;
            }
            //this.gameObject.SetActive(false);
            //El juego no mas destruye el objeto, solo lo desactiva mientras el jugador mantenga en esa scena.
            //al dejar la scena debe activar el enemigo.
            Vector3 spawnPos = gameObject.transform.localPosition;
            gameObject.transform.position = spawnPos;
            if(loot != null)
            Instantiate(loot.DropLootAfterDead(),gameObject.transform.position, gameObject.transform.rotation);
            Destroy(this.gameObject);
        }
        if (player != null)
        {
            pPosition = new Vector2(player.transform.position.x, player.transform.position.y);
        }
        if (esquivar == true)
        {
            //	pPosition = new Vector2 (player.transform.position.x + EsquivarCentroX(), player.transform.position.y+EsquivarCentroY());
        }
        else
        {

        }
        //	
        float h = pPosition.x - transform.position.x;
        float j = pPosition.y - transform.position.y;


        if (atacar == true)
        {
            if (attackObject != null)
            {
                Attack2(h, j, attackObject);
            }
            else
            {
                Attack2(h, j);
            }
        }
        else
        {
            if (patrullar == true)
            {
                Patrullar(h, j);
            }
            Move(h, j);

        }
        ActAnimator(h, j);
        i++;
    }
    public void Move(float move, float move2)
    {
        if (followPlayer == true)
        {
            m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed * 0, m_Rigidbody2D.velocity.y);
            m_Rigidbody2D.velocity = new Vector2(move2 * m_MaxSpeed * 0, m_Rigidbody2D.velocity.x);


            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x,
            transform.position.y), pPosition, 1f * Time.deltaTime);

            if (move > 0)
            {
                mySriteRenderer.flipX = true;
            }
            else
            {
                mySriteRenderer.flipX = false;
            }


            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x,
                    transform.position.y), pPosition, m_MaxSpeed * Time.deltaTime);
            attackPosition = pPosition;
            int random = Random.Range(1, 200);
            //print (random);
            if (random < 3)
            {
                atacar = true;
            }
        }
        //print (m_Rigidbody2D.velocity);
    }

    public virtual void Attack2(float move, float move2)
    {
        if (followPlayer == true)
        {
            if (m_Anim.runtimeAnimatorController != null)
            {
                m_Anim.SetTrigger("Attacking");
            }
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x,
                    transform.position.y), attackPosition, m_MaxSpeed * Time.fixedDeltaTime);
            if (transform.position.x == attackPosition.x || i > 150)
            {
                atacar = false;
                i = 0;
            }
        }
    }
    public virtual void Attack2(float move, float move2, GameObject attackRange)
    {
        if (followPlayer == true)
        {
            if (m_Anim.runtimeAnimatorController != null)
            {
                m_Anim.SetTrigger("Attacking");
            }
            attackRange.SetActive(true);
            attackRange.transform.position = Vector2.MoveTowards(new Vector2(attackRange.transform.position.x,
                attackRange.transform.position.y), attackPosition, 12f * Time.deltaTime);
            if (transform.position.x == attackPosition.x || i > 150)
            {
                atacar = false;
                attackRange.SetActive(false);
                attackRange.transform.position = transform.position;
                i = 0;
            }
        }
    }

    public void DejarDeSeguir()
    {

        StartCoroutine(WaitForSeguir(0.6f));
    }
    public void Patrullar(float move, float move2)
    {
        int random = Random.Range(1, 200);
        if (girar > 100)
        {
            randomX = Random.Range(-1, 2);
            randomY = Random.Range(-1, 2);
        }
        m_Rigidbody2D.velocity = new Vector2(randomX, randomY);
        if (m_Rigidbody2D.velocity.x > 0)
        {
            mySriteRenderer.flipX = false;
        }
        else
        {
            mySriteRenderer.flipX = true;
        }
        if (girar < 100)
        {
            m_Rigidbody2D.velocity = new Vector2(randomX, randomY);

        }
        else if (random > 50)
        {
            m_Rigidbody2D.velocity = new Vector2(randomX, randomY);
            if (girar > 101)
            {
                girar = 0;
            }
        }
        girar++;

    }
    public void RecibirDanio()
    {
        if (m_Anim.runtimeAnimatorController != null)
        {
            if (attackCollider != null)
            attackCollider.enabled = false;
            m_Anim.SetTrigger("Damage");
        }
    }
    private IEnumerator WaitForSeguir(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        followPlayer = false;
        patrullar = true;
        atacar = false;
    }
    public void ActAnimator(float move, float move2)
    {
        if (m_Anim.runtimeAnimatorController != null)
        {
            m_Anim.SetFloat("PlayerDifferenceX", move);
            m_Anim.SetFloat("PlayerDifferenceY", move2);
            m_Anim.SetFloat("DirectionX", m_Rigidbody2D.velocity.x);
            m_Anim.SetFloat("DirectionY", m_Rigidbody2D.velocity.y);
            if (Mathf.Abs(m_Rigidbody2D.velocity.x) > 0.1f || Mathf.Abs(m_Rigidbody2D.velocity.y) > 0.1f)
            {
                m_Anim.SetBool("Moving", true);
            }
            else
            {
                m_Anim.SetBool("Moving", false);
            }
        }
    }
    public void EnemyStaggered()
    {
      //  m_Rigidbody2D.velocity = new Vector2(0, 0);
        m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    public void EnemyRecovered()
    {
        //  m_Rigidbody2D.velocity = new Vector2(0, 0);
        m_Rigidbody2D.constraints = RigidbodyConstraints2D.None;
        m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
