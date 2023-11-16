using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ItemPickedUp : MonoBehaviour {

	public	bool playerHasItem;
	public bool interactuable = false;

	public TMP_Text tutorialText;
	public RawImage btnImage;
	public RectTransform textContainer;
	public RectTransform placeHolder;

	[SerializeField]
	PlayerController pC;
	// Use this for initialization
	void Start () {
		playerHasItem = false;
		pC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		textContainer = GameObject.FindGameObjectWithTag("Container").GetComponent<RectTransform>();
		btnImage = GameObject.FindGameObjectWithTag("Info").GetComponent<RawImage>();
		tutorialText = GameObject.FindGameObjectWithTag("Tutorial").GetComponent<TMP_Text>();
        try
        {
			placeHolder = GameObject.FindGameObjectWithTag("Placeholder").GetComponent<RectTransform>();
		}
        catch (System.Exception)
        {

            print("no placeholder found ");
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		bool x = CrossPlatformInputManager.GetButton("Fire3");
		if (interactuable==true) {
			if (Input.GetKeyDown ("x") || x==true) {
				PlayerPickedItem ();
			}
		}
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag ("Player") == true) {
			interactuable = true;

			if(placeHolder != null)
            {
				Scene scene = SceneManager.GetActiveScene();
                if (scene.name.Equals("HighWay"))
                {
					placeHolder.localPosition = new Vector2(this.gameObject.transform.localPosition.x*2*10 , this.gameObject.transform.localPosition.y+130);
					//to change the position on top of the key
					textContainer.anchoredPosition = placeHolder.localPosition;
				}
                else
                {
					placeHolder.localPosition = new Vector2(this.gameObject.transform.localPosition.x * 100, this.gameObject.transform.localPosition.y * 100);
					//to change the position on top of the key
					textContainer.anchoredPosition = placeHolder.localPosition;
				}
			}
            else
            {
				textContainer = GameObject.FindGameObjectWithTag("Container").GetComponent<RectTransform>();
				//to change the position on top of the key
				textContainer.anchoredPosition = new Vector2(218, 100);
			}
			
			// 1 for X, 0 for Y. we use X to pick up items
			tutorialText.text = pC.texts[1];
			tutorialText.color = pC.colors[1];
			btnImage.enabled = true;
			tutorialText.enabled = true;
		}
	}
	void OnTriggerStay2D(Collider2D col)
	{
		if (col.CompareTag("Player") == true)
		{
			interactuable = true;
			btnImage.enabled = true;
			tutorialText.enabled = true;
		}
	}
		void OnTriggerExit2D(Collider2D col)
	{
		interactuable =false;
		btnImage.enabled = false;
		tutorialText.enabled = false;
	}
	public void PlayerPickedItem (){
		this.gameObject.GetComponent<SpriteRenderer>().enabled = false;	
		this.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;	
		playerHasItem = true;
		print ("agarre item");
	}
	public bool CheckItemIsPicked(){
		return playerHasItem;
	}
	public void PlayerLostItem(){
		this.gameObject.GetComponent<SpriteRenderer>().enabled = true;	
		this.gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
		playerHasItem = false;
	
	}
}
