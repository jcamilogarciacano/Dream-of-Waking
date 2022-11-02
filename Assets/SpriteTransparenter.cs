using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTransparenter : MonoBehaviour
{
    // Start is called before the first frame update
	SpriteRenderer m_SpriteRenderer;
    void Start()
    {
		m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
		m_SpriteRenderer.color = new Color(1f,1f,1f,5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
