using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLoot : MonoBehaviour
{
    [SerializeField]
    GameObject loot;
    GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject DropLootAfterDead()
    {
        return loot;
    }
    public GameObject Canvas()
    {
        return canvas;
    }


}
