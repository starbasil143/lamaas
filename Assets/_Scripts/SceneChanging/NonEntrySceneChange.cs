using System;
using UnityEngine;

public class NonEntrySceneChange : MonoBehaviour
{


    [Header("Destination")]
    public string _sceneToLoad;
    public Vector3 _positionToSpawnAt;
    public MusicArea _musicArea;
    public float _songVersion;

    public void GoToSceneAtPosition()
    {
        SceneTransitionManager.StartSceneChangeFromPosition(_sceneToLoad, _positionToSpawnAt);

        
        if (_musicArea != MusicArea.NONE)
        {
            AudioManager.instance.SetMusicArea(_musicArea);
        }
        AudioManager.instance.SetSongVersion(_songVersion);

    }

}
