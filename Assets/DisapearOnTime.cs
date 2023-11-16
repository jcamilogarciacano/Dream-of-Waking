using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisapearOnTime : MonoBehaviour
{
    public GameObject tutorial;
    public int waitTime = 120;
    int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        tutorial.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (counter > waitTime)
        {
            tutorial.SetActive(false);
        }
        else if (tutorial.activeSelf == true)
        {
            counter++;
        }
      
    }
}
