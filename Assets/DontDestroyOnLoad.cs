using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
	int key = 0;
    // Start is called before the first frame update
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
		if(key ==1){
			DontDestroyOnLoad(this.gameObject);
		}
		else if (this.gameObject.name == "DontDestroyOnLoad"){
			DontDestroyOnLoad(this.gameObject);
		}
	//	if(this.gameObject.GetComponent<SpriteRenderer>()

    }
	public void KeyPicked(int value){
		key = value;
	}
}
