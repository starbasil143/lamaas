using Unity.VisualScripting;
using UnityEngine;

public class SceneStart : MonoBehaviour
{
    private GameObject _theFog;
    public bool sceneHasFog;
    private void Start()
    {
        _theFog = GameObject.FindGameObjectWithTag("Fog");
        _theFog.GetComponent<SpriteRenderer>().enabled = sceneHasFog;
    }  
    
}
