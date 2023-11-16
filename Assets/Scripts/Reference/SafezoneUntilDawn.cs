using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SafezoneUntilDawn : MonoBehaviour
{
    public GameObject safeZone;
    private Collider2D sf2D;
    public int i;
    // Start is called before the first frame update
    void Start()
    {
        sf2D = safeZone.GetComponent<CircleCollider2D>();
        EnableSafeZone();
    }
    void EnableSafeZone()
    {
        print("estoy segura!");
        sf2D.enabled = true;
    }
    void DisableSafeZone()
    {
        sf2D.enabled = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        

        // if scene
        if (SceneManager.GetActiveScene().name == "DarkPlace") {
            i++;
            if (i > 1500)
            {
                DisableSafeZone();
                //i=0;
                if (i > 2000)
                {
                    SceneManager.LoadScene("Testing", LoadSceneMode.Single);
                }
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "enemyBody")
        {
            
            Destroy(other.gameObject);
            print("objeto destruido en zona segura");
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "enemyBody")
        {
            Destroy(other.gameObject);
            print("objeto destruido en zona segura");
        }
    }
}
