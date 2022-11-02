using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class ScreenplayController : MonoBehaviour {

	//SP is a screenplay
	//AS is a sound
	public GameObject NoteText;
	public GameObject DeadText;
	public GameObject Gun;
	PlayerController pC;
	bool noteRead;
	public bool gunActive;

	bool x;

	// Use this for initialization
	void Start () {
		pC = GetComponent<PlayerController> ();
		NoteText.SetActive(false);
		noteRead = false;
		gunActive = false;
	}
	
	// Update is called once per frame
	void Update () {
		 x = CrossPlatformInputManager.GetButton("Fire3");
		if (noteRead){
			NoteText.SetActive(true);
		}
	}

	void OnCollisionExit2D ( Collision2D other)
    {
        if (other.gameObject.CompareTag("Note"))
        {
            noteRead = true;
        }
    }

     void OnCollisionStay2D (Collision2D other)
    {
    	if ((other.gameObject.CompareTag("Deceased"))&x==true)
        {
            print("Tenemos arma pez");
            gunActive = true;
            Destroy(DeadText);
            Destroy(Gun);
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("TextOnce"))
        {
            Destroy (other);
        }
    }
}
