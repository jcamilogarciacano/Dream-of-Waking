using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XlayerFliping : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        //transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.y);
        try
        {
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int)Camera.main.WorldToScreenPoint(gameObject.GetComponent<SpriteRenderer>().bounds.min).y * -1;

        }
        catch (System.Exception)
        {

            return;
        }
		}
}
