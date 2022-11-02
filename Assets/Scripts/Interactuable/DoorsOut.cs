using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DoorsOut : MonoBehaviour {

    public GameObject target;
    bool Pause = true;
    public GameObject TextThatAppearsWhenInteracted;
    public GameObject TextThatAppearsWhenInteracted2;
    int turnoDelDialogo = 0;

    void Start()
    {
        TextThatAppearsWhenInteracted.SetActive(false);
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
        if (other.name == target.name)
        {
            TextThatAppearsWhenInteracted.SetActive(false);
            if (TextThatAppearsWhenInteracted2 != null)
            {
                TextThatAppearsWhenInteracted2.SetActive(false);
            }
        }
    }
    private void EnableText(Collider2D other)
    {
        if (other.name == target.name)
        {

            if (Input.GetKeyDown("k"))
            {

             
            }
        }
    }
}
