using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
    [SerializeField] PlayableDirector _playableDirector;
    [SerializeField] private bool destroySelf = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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
