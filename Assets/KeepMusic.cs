using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class KeepMusic : MonoBehaviour
{
    public string sceneName;
    public string secondSceneName;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
       
    }
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        //to clear an error
        try
        {
            if (this.gameObject != null)
            {
              
                 if(scene.name.Equals(sceneName))
                {
                    print("1");
                    Destroy(this.gameObject);
                }

                 
            }
        }
        catch (System.Exception)
        {

            print("music likes to cry");
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void LateUpdate()
    {
        //machetazo: for the music to be destroyed when loaded freezeToDead Scene
        if (SceneManager.GetActiveScene().name.Equals(secondSceneName))
        {
            print("3");
            Destroy(this.gameObject);
        }
      
    }
}
