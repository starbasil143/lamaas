using System;
using UnityEngine;

public class EntryInteraction : MonoBehaviour
{
    public enum EntryToSpawnAt
    {
        None,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
    }

    [Header("Destination")]
    public string _sceneToLoad;
    public EntryToSpawnAt _entryToSpawnAt;
    public MusicArea _musicArea;
    public float _songVersion;

    [Header("This Entry")]
    public EntryToSpawnAt _thisEntryNumber;
    public bool locked;

    public void Interact()
    {
        if (!locked)
        {
            SceneTransitionManager.StartSceneChangeFromEntry(_sceneToLoad, _entryToSpawnAt);

            
            if (_musicArea != MusicArea.NONE)
            {
                AudioManager.instance.SetMusicArea(_musicArea);
            }
            AudioManager.instance.SetSongVersion(_songVersion);
        }

    }

}
