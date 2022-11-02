using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class KeyItemsProgress : MonoBehaviour {

	public ItemPickedUp sopleteichon;
	public ItemPickedUp secondRoomKey;
	public ItemPickedUp generatorKey;
	ArrowScript arrow;
	public GameObject secondRoomDoor;
	public GameObject generatorCol;
	public GameObject iceStatue;
    public GameObject EnemyJail;
    public GameObject EnemyJail2;
    public GameObject Jail;
    public GameObject SubBoss;
    public GameObject Eliminated;
    public GameObject Bridge;
    public GameObject BigDoor;
	public GameObject BossEnabler;
	//public GameObject puzzleDoor;
	Scene m_Scene;
	bool levelOneFinished;
	public GameObject [] puzzlePieces = new GameObject[8];
	public GameObject puzzleDoor;
	bool levelTwoFinished;
    bool levelTrheeFinished;
    int piecesPicked;
	// Use this for initialization
	void Start () {
		m_Scene = SceneManager.GetActiveScene ();
		print (m_Scene.name);

		if (m_Scene.name =="SecondLevel") {
			piecesPicked = 0;
			levelOneFinished = true;
			levelTwoFinished = false;
		}
        if (m_Scene.name == "ThirdLevel")
        {
            
            levelOneFinished = true;
            levelTwoFinished = true;
            levelTrheeFinished = false;
        }
    }
	
	// Update is called once per frames
	void Update () {

		if (m_Scene.name =="FirstLevel") {
				if (iceStatue != null && sopleteichon.CheckItemIsPicked () == true) {
					iceStatue.GetComponent<CloseEnoughtToInteract> ().destroyable = true;
				}
			if (iceStatue == null) {
				print ("se derritio");
				if (!secondRoomKey.CheckItemIsPicked () && secondRoomKey != null) {
					secondRoomKey.GetComponent<SpriteRenderer> ().enabled = true;
					secondRoomKey.GetComponent<CircleCollider2D> ().enabled = true;
				}
			}
			if (secondRoomKey.CheckItemIsPicked () && secondRoomKey != null) {
				secondRoomDoor.GetComponent<DoorsInteractive> ().enabled = true;
			}
			if (generatorKey.CheckItemIsPicked () && generatorKey != null) {
				generatorCol.GetComponent<DoorsInteractive> ().enabled = true;
			}
			if (GameObject.Find ("arrow") != null) {
				if (GameObject.Find ("arrow").GetComponent<ArrowScript> ().openDoor == true) {
					BigDoor.SetActive (false);
					BossEnabler.GetComponent<CloseEnoughtToInteract> ().enabled = true;
					levelOneFinished = true;
				}
			}
		}
		if (m_Scene.name == "SecondLevel") {
			m_Scene = SceneManager.GetActiveScene ();
			if (m_Scene.name == "SecondLevel") {
				if(piecesPicked==8){	
					puzzleDoor.GetComponent<CloseEnoughtToInteract>().enabled =true;
					if(GameObject.Find("Main Camera 2")!=null){
					puzzleDoor.GetComponent<DoorsInteractive>().enabled = true;
				 }
				}
				CountPieces (0);
				print (piecesPicked);
			}
		}
            if (m_Scene.name == "ThirdLevel")
            {
                if (secondRoomKey.CheckItemIsPicked() && secondRoomKey != null)
                {
                    secondRoomDoor.GetComponent<DoorsInteractive>().enabled = true;
                }
            if (EnemyJail == null && EnemyJail2 == null)
            {
                Destroy(Jail,2);
            }
            if (SubBoss == null)
            {
                Destroy(Eliminated);
                Bridge.SetActive(true);
            }
        }
       // }
    }
	void CountPieces(int qnt){
		foreach (GameObject item in puzzlePieces) {
			if (item.GetComponent<ItemPickedUp> ().CheckItemIsPicked() == true) {
				qnt++;
			}
		}
		piecesPicked = qnt;
	}
}
