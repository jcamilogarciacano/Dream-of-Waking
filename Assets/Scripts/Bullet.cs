using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float Speed;
    public float spwanTime;
    public GameObject spwaner;
    public GameObject bulletPrefab;
    GameObject player;
    Vector3 target, dir;
    private float counter;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform.position;
            dir = (target - transform.position).normalized;
        }
    }
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;

        if (counter > spwanTime)
        {
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, spwaner.transform, true);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Speed, bullet.GetComponent<Rigidbody2D>().velocity.y);
            counter = 0;
        }
	}
    void OnTriggerEnter2D(Collider2D col)
    {
        // Si chocamos contra el jugador o un ataque la borramos
        if (col.transform.tag == "Player" )
        {
            Destroy(bulletPrefab);
        }
    }
    void OnBecameInvisible()
    {
        // Si se sale de la pantalla borramos la roca
        Destroy(bulletPrefab);
    }
}
