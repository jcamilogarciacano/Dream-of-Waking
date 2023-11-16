using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class OnMouseClick : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;
    public GameObject mapSprite;

    public TMP_Text tutorialText;
    public RawImage btnImage;
    public RectTransform textContainer;
    int one = 0;
    [SerializeField]
    PlayerController pC;

    // tienen quee star en el update 
    //public bool mapa, escape;

    // Start is called before the first frame update
    void Start()
    {

        m_SpriteRenderer = mapSprite.GetComponent<SpriteRenderer>();
        pC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        textContainer = GameObject.FindGameObjectWithTag("Container").GetComponent<RectTransform>();
        btnImage = GameObject.FindGameObjectWithTag("Info").GetComponent<RawImage>();
        tutorialText = GameObject.FindGameObjectWithTag("Tutorial").GetComponent<TMP_Text>();

        if (textContainer != null)
        {
            textContainer.gameObject.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        bool mapa = CrossPlatformInputManager.GetButton("Fire3"); //X on xbox
        bool escape = CrossPlatformInputManager.GetButton("Back"); //B on xbox
        // if map already was picked up
        if (pC.mapPicked == true)
        {
            m_SpriteRenderer.enabled = false;

            btnImage.enabled = false;
            tutorialText.enabled = false;
        }
        else
        {

            textContainer.anchoredPosition = new Vector2(18, 82);
            //table map sprite
            m_SpriteRenderer.enabled = true;
            tutorialText.text = pC.texts[1];
            tutorialText.color = pC.colors[1];
            btnImage.enabled = true;
            tutorialText.enabled = true;
        }

        // input control // input teclado 
        if (mapa == true || Input.GetKeyDown("x"))
        {
            if(pC.mapPicked == false)
            pickUpMap();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (pC.mapPicked == false)
                    pickUpMap();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) || escape == true)
        {
            // load a new scene\
            Time.timeScale = 1.0f;
            SceneManager.UnloadSceneAsync("insideCamp");
        }
    }
    void OnMouseDown()
    {
      // pickUpMap();

    }
    void pickUpMap()
    {
        m_SpriteRenderer.enabled = false;
        btnImage.enabled = false;
        tutorialText.enabled = false;
       // DontDestroyOnLoad(this.gameObject);
        // load a new scene\
        Time.timeScale = 1.0f;
        SceneManager.UnloadSceneAsync("insideCamp");
        pC.mapPicked = true;
        //change tutorial text to Y for the player to learn how to open the map.
        tutorialText.text = pC.texts[0];
        tutorialText.color = pC.colors[0];
        btnImage.enabled = true;
        tutorialText.enabled = true;
        //textContainer.SetActive(true);
    }
}
