using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnMouseClick : MonoBehaviour
{
	SpriteRenderer m_SpriteRenderer;
	public GameObject mapSprite;
    // Start is called before the first frame update
    void Start()
    {
		m_SpriteRenderer = mapSprite.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	void OnMouseDown()
	{
		m_SpriteRenderer.enabled =false;
		DontDestroyOnLoad(this.gameObject);
		// load a new scene\
		Time.timeScale = 1.0f;
		SceneManager.UnloadSceneAsync("insideCamp");
	}
}
