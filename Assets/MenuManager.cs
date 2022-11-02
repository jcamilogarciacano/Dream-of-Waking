using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour {

	    public void AccionBotones(string AccionBotones)
    {
        SceneManager.LoadScene(AccionBotones);
    }

    public void QuitGame()
    {
                
    }
    public AudioMixer Audio;
    public void volumen(float volumen)
    {
        Audio.SetFloat("volumen", volumen);

    }
    public void resolucion(int calidadResolucion) 
    {
        QualitySettings.SetQualityLevel(calidadResolucion); 
    }


}
