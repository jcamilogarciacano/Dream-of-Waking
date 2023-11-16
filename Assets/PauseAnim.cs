using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAnim : MonoBehaviour
{
    Animator anim;
    string m_ClipName;
    AnimatorClipInfo[] m_CurrentClipInfo;

    float m_CurrentClipLength;
    float time = 0;
    public float waitTime = 30;

    public GameObject[] hideItems;
    public GameObject[] showItems;
    public float holdTime = 1;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            anim = GetComponent<Animator>();
            //Fetch the current Animation clip information for the base layer
            m_CurrentClipInfo = this.anim.GetCurrentAnimatorClipInfo(0);
            //Access the current length of the clip
            m_CurrentClipLength = m_CurrentClipInfo[0].clip.length;
        }
        catch (System.Exception)
        {

            print("miau");
        }
     
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        time++;
        if(time >= waitTime)
        {
            if (anim != null)
            {
                anim.speed = 0;
            }
            StartCoroutine(WaitForSeguir(holdTime));
           
        }
    }
    private IEnumerator WaitForSeguir(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        foreach (var item in hideItems)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var item in showItems)
        {
            item.gameObject.SetActive(true);
        }
    }
}
