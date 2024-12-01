using UnityEngine;

public class MusicChange : MonoBehaviour
{
    public MusicArea _musicArea;
    public void ChangeMusic()
    {
        AudioManager.instance.SetMusicArea(_musicArea);
    }
}
