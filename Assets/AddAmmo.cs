using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAmmo : MonoBehaviour
{
    public Show_bullets bullets;
    public bool ammoPicked;
    // Start is called before the first frame update
    void Start()
    {
        ammoPicked = false;
        bullets = GameObject.Find("BulletsInfo").GetComponent<Show_bullets>();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.GetComponent<ItemPickedUp>().playerHasItem == true && ammoPicked == false)
        {
            pickedAmmo();
        }
    }
    public void pickedAmmo()
    {
        bullets.ammo += 15;
        bullets.ammoText.text = bullets.ammo.ToString();
        ammoPicked = true;
    }
}
