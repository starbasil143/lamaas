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

    [Header("This Entry")]
    public EntryToSpawnAt _thisEntryNumber;

    public void Interact()
    {
        SceneTransitionManager.StartSceneChangeFromEntry(_sceneToLoad, _entryToSpawnAt);
    }

}
