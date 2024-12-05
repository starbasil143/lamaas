using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
    [SerializeField] PlayableDirector _playableDirector;
    [SerializeField] private bool destroySelf = true;
    private GameObject _player;
    public bool onExit;
    public bool onCoordinateExit;
    public float ymin;
    public float ymax;
    public float xmin;
    public float xmax;


    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (onCoordinateExit)
        {
            if (_player.transform.position.y < ymin || _player.transform.position.y > ymax || 
            _player.transform.position.x < xmin || _player.transform.position.x > xmax)
            {
                _playableDirector.gameObject.SetActive(true);
                _playableDirector.Play();
                if (destroySelf)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !onExit && !onCoordinateExit)
        {
            _playableDirector.gameObject.SetActive(true);
            _playableDirector.Play();
            if (destroySelf)
            {
                Destroy(gameObject);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && onExit && !onCoordinateExit)
        {
            _playableDirector.gameObject.SetActive(true);
            _playableDirector.Play();
            if (destroySelf)
            {
                Destroy(gameObject);
            }
        }
    }
}
