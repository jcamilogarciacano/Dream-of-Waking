using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public float X;
    public float Y;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
      
        if (col.CompareTag("Player") == true)
        {
            col.gameObject.transform.position = new Vector2(X, Y);
        }

        //m_Rigidbody2D.AddForce( new Vector2 (-col.transform.position.x, -col.transform.position.y));
    }
}
