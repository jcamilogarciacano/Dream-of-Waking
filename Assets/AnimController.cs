using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class AnimController : MonoBehaviour
{
    private Animator m_Anim;
    private Rigidbody2D m_Rigidbody2D;
    private Patrol patrol;
    private SpriteRenderer mySriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        patrol = GetComponent<Patrol>();
        mySriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (patrol != null)
        {
            if (patrol.isPatrolling == true)
            {
                if (patrol.IsMoving() == true)
                {
                    UpdateOrientation(patrol.GetNextPoint());
                    UpdateAnimator("Moving", true);

                }
                else
                {
                    UpdateAnimator("Moving", false);
                }

            }
        }
    }

    public void UpdateOrientation(Vector2 objective)
    {
        if (objective.x > this.gameObject.transform.position.x)
        {
            mySriteRenderer.flipX = false;
        }
        else
        {
            mySriteRenderer.flipX = true;
        }
    }
    public  void UpdateAnimator(string Parameter, bool state)
    {
        m_Anim.SetBool(Parameter, state);
    }
    public void UpdateTrigger(string Parameter)
    {
        m_Anim.SetTrigger(Parameter);
    }

    public void Move(float move, float move2)
    {

        if (move != 0f || move2 != 0f)
        {
         

        }
        m_Anim.SetFloat("Speed", Mathf.Abs(move));
        m_Anim.SetFloat("Speed2", Mathf.Abs(move2));

        m_Anim.SetFloat("Velocity", m_Rigidbody2D.velocity.x);
        m_Anim.SetFloat("Velocity2", m_Rigidbody2D.velocity.y);
    }

}
