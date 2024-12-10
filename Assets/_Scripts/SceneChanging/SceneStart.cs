using Unity.VisualScripting;
using UnityEngine;

public class SceneStart : MonoBehaviour
{
    private GameObject _theFog;
    private GameObject _theWind;
    public bool sceneHasFog;
    public bool sceneHasWind;
    private void Start()
    {
        _theFog = GameObject.FindGameObjectWithTag("Fog");
        _theFog.GetComponent<SpriteRenderer>().enabled = sceneHasFog;

        _theWind = GameObject.FindGameObjectWithTag("Wind");

        if (!sceneHasWind)
        {
            _theWind.GetComponent<ParticleSystem>().Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        else
        {
            _theWind.GetComponent<ParticleSystem>().Play();
        }
    }  
    
}
