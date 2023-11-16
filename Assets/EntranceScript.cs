using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEditor.Animations;

public class EntranceScript : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody2D;
    public CapsuleCollider2D collider;
    public Animator m_Anim;
    public GameObject player;
    private Vector2 pPosition;
    public float m_MaxSpeed = 3f;
    public EnemyController eC;
    public SpriteRenderer mySpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        m_Anim = gameObject.GetComponent<Animator>();
        eC = gameObject.GetComponent<EnemyController>();
        mySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //for the enemies to cross the invisible wall only when entering.
        collider.enabled = false;
        if (SceneManager.GetActiveScene().name == "Boss")
        {

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player != null)
        {
            pPosition = new Vector2(player.transform.position.x, player.transform.position.y);
            Attack2();
        }
        StartCoroutine(WaitForEntrance(1f));

        if (this.gameObject.transform.position.x > pPosition.x)
        {
            mySpriteRenderer.flipX = true;
        }
        else
        {
            mySpriteRenderer.flipX = false;
        }
    }
    public virtual void Attack2()
    {
        
            if (m_Anim.runtimeAnimatorController != null)
            {
                m_Anim.SetBool("Attacking",true);
            }
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x,
                    transform.position.y), pPosition, m_MaxSpeed * Time.fixedDeltaTime);
        
    }
    private IEnumerator WaitForEntrance(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        collider.enabled = true;
        eC.enabled = true;
        m_Anim.SetBool("Attacking", false);
        gameObject.GetComponent<EntranceScript>().enabled = false;
    }
}
