using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPuzzle : MonoBehaviour {

    // Variables a usar para mover las fichas
    public bool move;       
    public Transform slot;
    float x;
    float y;
public GameObject other;

    //Verificación del tag
    public string tagObject;

    //Variables para modificar el Script Hits.
    public GameObject script;
    Hits modify;



    void Start()
    {
        move = true;

        tagObject = gameObject.tag;
        modify = script.GetComponent<Hits>();
    
	}


    void Update()
    {
if (modify.points==16){

}
         
    }


    void OnTriggerEnter(Collider other)
    {
        this.other = other.gameObject;
        print("ENT "+other.tag+"www "+tagObject);
        if (other.tag == tagObject)
        {
            modify.points += 1;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == tagObject)
        {
            modify.points -= 1;

        }
    }


    void OnMouseUp()
    {
    if (move == true)
        {
            if(Vector3.Distance(transform.position, slot.position) == 1)
            {
                x = transform.position.x;
                y = transform.position.y;
                transform.position = new Vector3(slot.position.x, slot.position.y, 0);
                slot.position = new Vector3(x, y, 0);
            }
        }    
    }
}
