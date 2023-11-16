using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEffect : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;
    public Color aA;
    float i;
    float counter;
    // Start is called before the first frame update
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.color = new Color(0, 0, 0, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        counter++;
        if (counter < 70)
        {
            i = 0.2f;
            m_SpriteRenderer.color = new Color(0, 0, 0, i);
            
        }
        else if (counter < 150)
        {
            i = 0.4f;
            m_SpriteRenderer.color = new Color(0, 0, 0, i);

        }
        else if (counter < 230)
        {
            i = 0.6f;
            m_SpriteRenderer.color = new Color(0, 0, 0, i);
        }
        else if (counter < 250)
        {
            i = 1f;
            m_SpriteRenderer.color = new Color(0, 0, 0, i);
        }
    }
}
