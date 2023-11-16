using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class increaseSize : MonoBehaviour
{
    private Vector3 scaleChange, positionChange;
    // Start is called before the first frame update
    void Start()
    {
        scaleChange = new Vector3(0.005f, 0.005f, 0.005f);
        positionChange = new Vector3(0.0f, 0.0025f, 0.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
      
        if (transform.localScale.x > 5f || transform.localScale.y > 3.0f)
        {

        }
        else
        {
            transform.localScale += scaleChange;
            transform.position += positionChange;
        }
        }
}
