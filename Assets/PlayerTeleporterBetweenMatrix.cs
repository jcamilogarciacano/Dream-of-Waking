using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTeleporterBetweenMatrix : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemies;
    public Transform[] transforms;
    public GameObject player;
    public string sceneName;
    public GameObject[] backGrounds;
    public GameObject[] specialGrounds;
    private GameObject presentBG;
    private IEnumerator coroutine;
    private GameObject activeScene;
    public ItemPickedUp key;
    public ItemPickedUp dinamyte;
	public SafezoneUntilDawn sF;

    private int lives = 3;
    int kk=0;
    int k=-1;
    int i=0;
    int j=1;

bool specialLine;
    int z=0;
    void Start()
    {
    	specialLine = false;
    	presentBG = backGrounds[i];
        backGrounds[i].SetActive(true);

//key = GameObject.Find("DE_PW/key_gold").GetComponent<ItemPickedUp>();

    }

    // Update is called once per frame
    void Update()
    {
      
    	Adaptation();

    	  if(player.transform.position.x > -0.5f && player.transform.position.x < 0.5f){
    	  	if(player.transform.position.y > 1f ){ 
            activeScene= GameObject.FindWithTag("KeyDoor");
            if(activeScene != null){
            	if(key.CheckItemIsPicked()== true){
    	  	sceneName = "InsideHouse";
        	 coroutine = WaitAndPrint(0.2f);
             StartCoroutine(coroutine);
        	}
        }
        }
        }


//RECORDATORIO: LOS ENABLER VAN CON i y j , DISABLERS VAN CON i y k. LOS NORMALES, 
    	//SPECIALS VAN CON z y v, v y z LOS DISABLERS SPECIALS.
    	//REMINDER: UTILIZAR LA LISTA ADECUADA PARA EL METODO INDICADO
        if(player.transform.position.y > 3 && player.transform.position.x > 3){
        		ReSpawnEnemies();
        	 EnablerBG(backGrounds[i],backGrounds[j]);
        	 print(j+" future red "+i);
        }
        else if(player.transform.position.y >3 && backGrounds[i].name == "BG_PW"){
        		ReSpawnEnemies();
        	 EnablerBG(backGrounds[i],backGrounds[j]);
        	 print(j+" future red "+i);
        }
         else if(player.transform.position.y > 3 && player.transform.position.x < -3){
             SpecialEnablerBG(specialGrounds[z],specialGrounds[j]);
        	 print(j+" future blue "+i);
        }
        else if(player.transform.position.x > 6.37f){
        		
        	 SpecialEnablerBG(specialGrounds[z],specialGrounds[j]);
        	 print(j+" future lado casa blue "+i);
        }
         if(player.transform.position.y < -3 && specialLine ==false){
         		ReSpawnEnemies();
        	 print(kk);
        	 DisablerBG(backGrounds[i],backGrounds[k]);	
        	 print(k+" backfure red "+i);
        }
         else if(player.transform.position.x < -6.37f){
         		
        	 SpecialDisablerBG(specialGrounds[z],specialGrounds[k]);
        	 print(j+" backfure blue "+i);
        }       
        
        else if(player.transform.position.y < -3 && specialLine ==true){
       
	         SpecialDisablerBG(specialGrounds[z],specialGrounds[k]);
        	 print(j+" backfure blue "+i);
        	}	
print(specialLine);
    }

void LateUpdate()
    {

    }


    public void Adaptation(){     
 if( k<0){
 	int _k=0;
 	_k=backGrounds.Length-1;
 	k=_k;
 }
 if(i<0){
 	int _i=0;
 	_i=backGrounds.Length-1;
 	i=_i;
 }
 if( j<0){
 	int _j=0;
 	_j=backGrounds.Length-1;
 	j=_j;
 }

 if( k>backGrounds.Length-1){
 	int _k=0; 	
 	k=_k;
 }
 if(i>backGrounds.Length-1){
 	int _i=0;
 	i=_i;
 }
 if( j>backGrounds.Length-1){
 	int _j=0;
 	j=_j;
 }
    	z=i;
    		
    	
    }



public void TeleportPlayerRandom(int new_i){
//PLAYERTELEPORT AT RANDOM POSITION OF THE BACKGROUNDS ARRAY, FISRTLY DISABLING ALL OF THE SCENES. to make sure 2 of them are active at the same time.
new_i = Random.Range(0,12);
foreach(GameObject scene in backGrounds){
	scene.SetActive(false);
}
foreach(GameObject scene in specialGrounds){
	scene.SetActive(false);
}
i = new_i;
j= new_i+1;
k= new_i-1;
z= new_i;
specialLine =false;
lives--;
if(lives ==0){
	SceneManager.LoadScene("FreezeToDead",LoadSceneMode.Single);
}
backGrounds[new_i].SetActive(true);
}
public void TeleportKeyRandom(int new_i){
//PLAYERTELEPORT AT RANDOM POSITION OF THE BACKGROUNDS ARRAY, FISRTLY DISABLING ALL OF THE SCENES. to make sure 2 of them are active at the same time.
new_i = Random.Range(0,12);
foreach(GameObject scene in backGrounds){
	
}
if(key.CheckItemIsPicked() ==true ){
key.transform.parent = backGrounds[new_i].transform;
key.PlayerLostItem();
}
}
    //PARA LIMPIAR LOS METODOS ENABLERBG Y DISABLERBG, SE CAMBIA LA UBICACION DEL PLAYER EN UN METODO APARTE
    // SE COMPARA EL NOMBRE DE LA SCENA SIGUIENTE PARA SABER A DONDE TELETRANPORTAR AL PLAYER.
public void TeleportPlayerE(string nameT){

if (nameT=="BG_PW" || nameT == "BG_V"){
player.transform.position =new Vector2(0,-3);
}
}

public void TeleportPlayerD(string nameT){	 	

 if(nameT =="BG_V"){
player.transform.position =new Vector2(3.7f,2.9f);
}	
else if(nameT=="BG_PW"){
player.transform.position =new Vector2(0,3);
}
}
//TELEPORTERS BASICOS, PERMITEN NAVEGAR ENTRE EL PRIMER CIRCULO (IZQUIERDO)
public void EnablerBG(GameObject pBG,GameObject nBG){
  pBG.SetActive(false);
TeleportPlayerE(nBG.name);
 nBG.SetActive(true);
 i++;
 j++;
 k++;
    }

  public void DisablerBG(GameObject pBG,GameObject bBG){
  pBG.SetActive(false);
TeleportPlayerD(bBG.name);
 bBG.SetActive(true);
 i--;
 j--;
 k--;
 print(kk);
    }
//TELEPORTERS INTERNOS, PERMITEN ENTRAR A PARTES MAS PROFUNDAS DE LOS CIRCULOS (IZQUIERDA), REQUIEREN PARAMETROS ESPECIALES
//NO MEZCLAR CON LOS METODOS TELEPORTPLAYERD Y TELEPORTPLAYERE. POSIBLEMENTE NECESITE MAS.
    public void SpecialEnablerBG(GameObject pBG,GameObject eBG){
  pBG.SetActive(false);
  if(eBG.name == "SD_H"){
  	print("entro");
  	player.transform.position =new Vector2(-6.2f,-1f);
  }
   else if(eBG.name == "DE_PW"){
  	print("entro");
  	player.transform.position =new Vector2(-6.2f,-1f);
  }
   else if(eBG.name == "SG_SH" || eBG.name == "DO_H"){
  	print("casa");
  	player.transform.position =new Vector2(0f,-3f);
  }

  else if(eBG.tag == "PlayerIsDead"){
 	print("player Died");
	player.SetActive(false);
  	 coroutine = WaitAndPrint(3.0f);
     StartCoroutine(coroutine);
}
 eBG.SetActive(true);
 specialLine=true;
i++;
j++;
k++;
    }
  public void SpecialDisablerBG(GameObject pBG,GameObject bBG){
  pBG.SetActive(false);
  if(bBG.name == "SD_H"){
player.transform.position =new Vector2(6.3f,-1f);
}
 else if(bBG.name == "BG_V"){
player.transform.position =new Vector2(-3.7f,2.9f);
specialLine=false;
}
 bBG.SetActive(true);
 i--;
 j--;
 k--;
    }
     private IEnumerator WaitAndPrint(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
             SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            print("WaitAndPrint " + Time.time);
        }
    }
    public void ReSpawnEnemies(){
		sF.i =1;
		PlayerController pc = player.GetComponent<PlayerController>();
    	Instantiate(enemies,new Vector3 (Random.Range(-6,6),Random.Range(-3,3),0f),transform.rotation);
		StartCoroutine(pc.WaitForDamage(1f));
    }
}
