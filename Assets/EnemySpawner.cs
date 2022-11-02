using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public GameObject enemies;
	private int i;
    // Start is called before the first frame update
    void Start()
    {
     i=0;   
    }

    // Update is called once per frame
    void Update()
    {
        if(i>500){
ReSpawnEnemies();
i=0;
        }
        else{
        	i++;
        }
    }
     private void ReSpawnEnemies(){
   
    	Instantiate(enemies,new Vector3 (Random.Range(-20,20),Random.Range(-10,10),0f),transform.rotation);
    }
}
