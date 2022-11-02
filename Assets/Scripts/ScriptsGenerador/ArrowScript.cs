using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
public class ArrowScript : MonoBehaviour {

    //estas son las fichas del puzzle
    public GameObject tokenSun;
    public GameObject tokenMoon;
    public GameObject tokenFlake;
    public GameObject tokenLeaf;
    public GameObject tokenFlower;
	int w;
    //en este vector se guardan las fichas 
    GameObject[] tokenPosition;

    //Variables auxiliares para manipular las fichas dentro del vector y su posicion
    GameObject tokenAux;
    GameObject tokenAux2;

    //variables de la flecha de seleccion de la ficha que cambiara de lugar
    int arrowPosition;
    int arrowSelect;
    bool tokenSelect;

    //camaras para cambiar entre el puzzle y el mapa principal
    public Camera camPuzzle;

    //variable para abrir la puerta
    public bool openDoor;


    void Start () 
    {
       
        arrowPosition = 2;
        arrowSelect = 0;
        tokenSelect = false;
        tokenPosition = new GameObject [5];
        tokenPosition[0] = tokenSun;
        tokenPosition[1] = tokenMoon;
        tokenPosition[2] = tokenFlake;
        tokenPosition[3] = tokenLeaf;
        tokenPosition[4] = tokenFlower;
        tokenAux = null;
        
        camPuzzle.GetComponent<Camera>().enabled = true;
        
        openDoor = false;

       
    }
	
	void FixedUpdate ()
	{
		float j = CrossPlatformInputManager.GetAxis("Horizontal");
		bool a = CrossPlatformInputManager.GetButton("Fire2");
		if (openDoor == false) {
		//	if (w>2) {
		//		w = 0;
		//	}
		
			if (a ==true) {
				tokenSelect = true;
				arrowSelect = arrowPosition;
			}
		//	if (w  ==2) {
				if (j > 0 & (transform.position.x < 2.2)) {

					transform.position = new Vector3 (transform.position.x + 1.1f, transform.position.y, transform.position.z);
					arrowPosition++;

					if (tokenSelect) {

						//creo un clon de la ficha a amover
						tokenAux = Instantiate (tokenPosition [arrowSelect]);
						tokenAux.transform.position = tokenPosition [arrowSelect].transform.position;
		
						//intercambio las pocisiones de las fichas
						tokenPosition [arrowSelect].transform.position = tokenPosition [arrowPosition].transform.position;
						tokenPosition [arrowPosition].transform.position = tokenAux.transform.position;
						tokenAux2 = tokenPosition [arrowSelect];

						//intercambio las pocisiones en el vector
						tokenPosition [arrowSelect] = tokenPosition [arrowPosition];
						tokenPosition [arrowPosition] = tokenAux2;
						tokenSelect = false;
						Destroy (tokenAux);
					}

				}
				if (j < 0 & (transform.position.x > -2.2)) {

					transform.position = new Vector3 (transform.position.x - 1.1f, transform.position.y, transform.position.z);
					arrowPosition--;

					if (tokenSelect) {
						//creo un clon de la ficha a amover
						tokenAux = Instantiate (tokenPosition [arrowSelect]);
						tokenAux.transform.position = tokenPosition [arrowSelect].transform.position;

						//intercambio las pocisiones de las fichas
						tokenPosition [arrowSelect].transform.position = tokenPosition [arrowPosition].transform.position;
						tokenPosition [arrowPosition].transform.position = tokenAux.transform.position;
						tokenAux2 = tokenPosition [arrowSelect];

						//intercambio las pocisiones en el vector
						tokenPosition [arrowSelect] = tokenPosition [arrowPosition];
						tokenPosition [arrowPosition] = tokenAux2;
						tokenSelect = false;
						Destroy (tokenAux);

					}
				}

				for (int i = 0; i <= 4; i++) {	


					TokenScript script = tokenPosition [i].GetComponent<TokenScript> ();

					if (!script.checkCorrectPosition) {	
						//print("break");
						break;
					}

					if (i == 4) {
						camPuzzle.GetComponent<Camera> ().enabled = false;
						DontDestroyOnLoad (this.gameObject);
						openDoor = true;
						Time.timeScale = 1.00f;
						SceneManager.UnloadSceneAsync ("Generator");
                
               

					}
				}
			//}
		}//w++;
	}
}
