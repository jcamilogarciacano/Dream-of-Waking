using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemySpawner : MonoBehaviour
{
    public GameObject[] enemies;
    public int what;
    public GameObject[] spawns;
    public int timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer >= 150)
        {
            what = Random.Range(0, 1);
            ReSpawnEnemies(what, Random.Range(0, 3));
            timer=0;
        }
        else {
            timer++;
                }
    }

    public void ReSpawnEnemies(int i, int pos)
    {
        //sF.i = 1;
        //PlayerController pc = player.GetComponent<PlayerController>();
        Instantiate(enemies[i], spawns[pos].transform.position, transform.rotation);

        //StartCoroutine(pc.WaitForDamage(1f));
    }
}
