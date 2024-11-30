using Unity.VisualScripting;
using UnityEngine;

public class SceneStart : MonoBehaviour
{
    private GameObject _theFog;
    public bool sceneHasFog;
    public MusicArea startingMusicArea;
    public float startingSongVersion = 0;
    private void Start()
    {
        _theFog = GameObject.FindGameObjectWithTag("Fog");
        _theFog.GetComponent<SpriteRenderer>().enabled = sceneHasFog;

        
        if (startingMusicArea != MusicArea.NONE)
        {
            AudioManager.instance.SetMusicArea(startingMusicArea);
        }
        AudioManager.instance.SetSongVersion(startingSongVersion);


    }  
    
}
