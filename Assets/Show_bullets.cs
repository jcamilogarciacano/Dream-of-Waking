using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Show_bullets : MonoBehaviour
{
    //public RawImage bullets;
    public TMP_Text ammoText;
    private Vector3 positionChange, positionChange2;
    public bool show;
    public bool hide;
    public RectTransform ammoInfo;
    public int ammo = 15;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (GameObject.FindGameObjectsWithTag("BulletsInfo").Length > 1)
        {
            Destroy(this.gameObject);
        }
    }
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name.Equals("HighWay") || scene.name.Equals("Testing") || scene.name.Equals("Boss") || scene.name.Equals("DarkPlace") || scene.name.Equals("Underworld")) { 
            ammoInfo = GameObject.Find("/Canvas/AmmoInfo").GetComponent<RectTransform>();
            ammoInfo.anchoredPosition = new Vector2(171, -92);
            ammoText = GameObject.Find("/Canvas/AmmoInfo/Ammo").GetComponent<TMP_Text>();
        }
        if (scene.name.Equals("Testing") || scene.name.Equals("DarkPlace") )
        {
            ammo += 15;  
        }
    }
        // Start is called before the first frame update
        void Start()
    {
        show = false;
        hide = false;
        // to show the freeze counter like crash bandicoot boxes/lives
        positionChange = new Vector3(0.0f, 0.01f, 0.0f);
        positionChange2 = new Vector3(0.0f, -0.01f, 0.0f);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if (show == true)
        {

            // transform.localScale += scaleChange;
            ammoInfo.transform.position += positionChange2;
            StartCoroutine(WaitAndPrint(1.7f));
        }
        else if (hide == true)
        {
            //  transform.localScale += scaleChange;
            ammoInfo.transform.position += positionChange;   
            StartCoroutine(WaitAndPrint2(1.7f));
        }
    }
    private IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        show = false;
        hide = true;

    }
    private IEnumerator WaitAndPrint2(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        show = false;
        hide = false;
    }
    public void AddAmmo()
    {

    }
}
