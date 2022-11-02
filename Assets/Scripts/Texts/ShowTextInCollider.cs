using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTextInCollider : MonoBehaviour {

    public GameObject target;
    public GameObject TextThatAppearsWhenInteracted;
    int turnoDelDialogo = 0;
   
    void Start()
    {


		if (TextThatAppearsWhenInteracted != null) {
			
		
			TextThatAppearsWhenInteracted.SetActive (false);
        
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (target == null) {
			Destroy (this.gameObject);
		}
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        EnableText(other);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        EnableText(other);

    }
    private void OnTriggerExit2D(Collider2D other)
    {
		if (target != null) {
			if (other.name == target.name) {
				hideText ();
			}
		}
    }
    private void EnableText(Collider2D other)
    {
        if (other.name == target.name)
        {

			showText ();
       
           
        }
    }
	private void showText(){
		if (TextThatAppearsWhenInteracted != null) {
			TextThatAppearsWhenInteracted.SetActive (true);
		}
	}
	private void hideText(){
		if (TextThatAppearsWhenInteracted != null) {
			TextThatAppearsWhenInteracted.SetActive (false);	
		}
	}
}
