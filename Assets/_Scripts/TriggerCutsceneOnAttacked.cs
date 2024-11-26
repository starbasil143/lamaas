using UnityEngine;
using UnityEngine.Playables;

public class TriggerCutsceneOnAttacked : MonoBehaviour
{
    public PlayableDirector _playableDirector;
    [SerializeField] private bool destroySelf = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Harm"))
        {
            if (collision.gameObject.GetComponent<HarmfulObjectScript>().Source.CompareTag("Player"))
            {
                if (collision.gameObject.GetComponent<HarmfulObjectScript>().destroyOnContact)
                {
                    collision.gameObject.GetComponent<HarmfulObjectScript>().DestroySelf();
                }
                _playableDirector.gameObject.SetActive(true);
                _playableDirector.Play();
                if (destroySelf)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
