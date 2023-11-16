using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class OnlyOneSongActive : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       
    }
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       
    }
        // Update is called once per frame
        void LateUpdate()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Music");
        if (obj.Length > 1)
        {
            print("2");
            Destroy(obj[0]);
        }
    }

}
