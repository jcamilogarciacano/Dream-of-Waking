using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject parent;
    public CapsuleCollider2D visionRange;
    public Patrol _patrol;
    public float speed;
    public bool attacking;
    public Transform objective;

    public int reloadAttack = 0;
    public int reloadingTime = 60;
    public float attackRange = 1;

    public AnimController m_anim;
    // Start is called before the first frame update
    void Start()
    {
        _patrol = GetComponentInParent<Patrol>();
        parent =  this.transform.parent.gameObject;
        m_anim = GetComponentInParent<AnimController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (attacking == true)
        {
            float diffX = Mathf.Abs(parent.transform.position.x - objective.transform.position.x);
            float diffY = Mathf.Abs(parent.transform.position.y - objective.transform.position.y);

            m_anim.UpdateOrientation(objective.transform.position);

            if (diffX > attackRange || diffY > attackRange)
            {

                float step = speed * Time.deltaTime;
                m_anim.UpdateAnimator("Moving", true);
               
                if(reloadAttack > reloadingTime)
                parent.transform.position = Vector2.MoveTowards(parent.transform.position, objective.transform.position, step);

                reloadAttack++;
            }
            else if (diffX <= attackRange && diffY <= attackRange)
            {
                m_anim.UpdateAnimator("Moving", false);
                m_anim.UpdateTrigger("Attack");
                reloadAttack = 0;
            }
          
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") == true)
        {
            objective = other.transform;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true)
        {
            //objective = collision.transform;
            _patrol.isPatrolling = false;
            attacking = true;
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        attacking = false;
        _patrol.isPatrolling = true;
    }
}
