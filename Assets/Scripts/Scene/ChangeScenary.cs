using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScenary : MonoBehaviour {
	public GameObject[] scenesArray;
	public GameObject scene;
	public int arrayPos;
	public Transform pos;
	public Transform posSpawn;
	public int yPosSpawn;
	public SceneBase sceneC;
	int i;
	// Use this for initialization
	void Start () {
		scene = GameObject.FindGameObjectWithTag ("Scenery");
		sceneC = scene.GetComponent<SceneBase> ();
		Actualizar ();	
	}

	// Update is called once per frame
	void Update () {
		//arrayPos = i;
		scene = GameObject.FindGameObjectWithTag ("Scenery");
		sceneC = scene.GetComponent<SceneBase> ();
		Actualizar ();

	}
	void OnCollisionEnter2D(Collision2D col){
		
		if (col.gameObject.tag == "Atras") {
			Destroy (scene);
			arrayPos -= 1;
			if (sceneC.atrasPosSpawnY ==0) {
				this.transform.position = new Vector2 (sceneC.atrasPosSpawn, this.transform.position.y);
			} else {
				this.transform.position = new Vector2 (sceneC.atrasPosSpawn, sceneC.atrasPosSpawnY);

			}
			GameObject scena =(GameObject)Instantiate (scenesArray [arrayPos]);
			scena.name = scenesArray [arrayPos].name;
			print ("cree"+Time.time);
			Actualizar ();
		}
		if (col.gameObject.tag == "Adelante") {
			Destroy (scene);
			arrayPos += 1;
			if (sceneC.adelantePosSpawnY == 0) {
				this.transform.position = new Vector2 (sceneC.adelantePosSpawn, this.transform.position.y);
			}
			else {
				this.transform.position = new Vector2 (sceneC.atrasPosSpawn, sceneC.atrasPosSpawnY);

			}
			GameObject scena =(GameObject)Instantiate (scenesArray [arrayPos]);
			scena.name = scenesArray [arrayPos].name;
			print ("cree"+Time.time);
			Actualizar ();
		}
	}
	void Actualizar(){
		foreach(GameObject go in scenesArray)
		{
			if(go.name == scene.name)
			{
				
			print ("wenas "+scenesArray.Length);
			arrayPos = i;
				//go.GetComponent<Text>().enabled = true;
				break;
			}
			i++;
		}
		i = 0;
		 
	}

}
