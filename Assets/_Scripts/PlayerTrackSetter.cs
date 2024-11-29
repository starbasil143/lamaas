using UnityEngine;
using UnityEngine.Playables;

public class PlayerTrackSetter : MonoBehaviour
{
    private PlayableDirector _director;
    private GameObject _player;
    private GameObject _cameraTarget;

    public bool bindPlayer;
    public bool bindCamera;
    void Awake()
    {
        _director = GetComponent<PlayableDirector>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _cameraTarget = _player.transform.Find("CameraFollowPoint").gameObject;
        if (bindPlayer)
        {
            BindPlayer();
        }

        if (bindCamera)
        {
            BindCamera();
        }
    }

    private void BindPlayer()
    {
        foreach (var output in _director.playableAsset.outputs)
        {
            if (output.streamName == "Player")
            {
                _director.SetGenericBinding(output.sourceObject, _player);
            }
        }
    }

    private void BindCamera()
    {
        foreach (var output in _director.playableAsset.outputs)
        {
            if (output.streamName == "Camera Track")
            {
                _director.SetGenericBinding(output.sourceObject, _cameraTarget);
            }
        }
    }

    
}
