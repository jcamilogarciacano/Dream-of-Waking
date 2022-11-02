using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper : MonoBehaviour {

    public GameObject target;
    // The game object to affect. If none, the trigger work on this game object
    public GameObject TextThatAppearsWhenInteracted;
    // Use this for initialization
    void Start()
    {
        TextThatAppearsWhenInteracted.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == target.name)
        {


            TextThatAppearsWhenInteracted.SetActive(true);

        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.name == target.name)
        {

            if (Input.GetKeyDown("k"))
            {
                TextThatAppearsWhenInteracted.SetActive(true);
            }
        }
        print("wenas");
        //DoActivateTrigger();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == target.name)
        {


            TextThatAppearsWhenInteracted.SetActive(false);

        }
        print("wenas");
        //DoActivateTrigger();
    }
}
