using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveDmg : MonoBehaviour
{
    public int life = 12;
    //public Sprite gotHitSprite;
   // public Sprite normalSprite;
   // public SpriteRenderer thisSpriteRenderer;
    public AnimController m_anim;
    public Patrol _patrol;
    public AudioSource hitSound;
    public Attack _attack;
    // Start is called before the first frame update
    void Start()
    {
       // thisSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        m_anim = GetComponent<AnimController>();
        _attack = GetComponentInChildren<Attack>();
        hitSound = GetComponent<AudioSource>();
        _patrol = GetComponentInParent<Patrol>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(life <= 0)
        {
            _patrol.enabled = false;
            _attack.enabled = false;
            m_anim.UpdateAnimator("Death", true);
            Destroy(this.gameObject, 1.9f);
        }
    }

    public void gotHit(Transform target)
    {
        life--;
        m_anim.UpdateTrigger("Damaged");
        hitSound.pitch = Random.Range(0.2f, 0.3f);
        hitSound.PlayOneShot(hitSound.clip);
        if (_attack != null)
        {
            _attack.objective = target;
            StartCoroutine(WaitForDamage(3f));
        }
    }

    public IEnumerator WaitForDamage(float waitTime)
    {
       
            _attack.attacking = true;
            _attack._patrol.isPatrolling = false;
        yield return new WaitForSeconds(waitTime);
       // _attack.attacking = false;
    }

}
