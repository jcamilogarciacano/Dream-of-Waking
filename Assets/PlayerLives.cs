using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerLives : MonoBehaviour
{
    public RawImage[] _hearts;
    //Select a Texture in the Inspector to change to
    public Texture half_heart;
    // Start is called before the first frame update
    void Start()
    {
      //  _hearts[_hearts.Length-1].texture = half_heart;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
