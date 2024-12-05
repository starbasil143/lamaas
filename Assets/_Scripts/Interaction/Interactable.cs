
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool isInRange;
    public UnityEvent interactAction;
    public bool selfCue;
    
    void Update()
    {
        if (isInRange && !DialogueManager.instance.dialogueIsPlaying)
        {
            if (selfCue)
            {
                GetComponentInChildren<SpriteRenderer>().enabled = true;
            }
            if (InputManager.Interact)
            {   
                interactAction.Invoke();
            }
        }
        else
        {
            if (selfCue)
            {
                GetComponentInChildren<SpriteRenderer>().enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            if (!selfCue)
            {
                collision.gameObject.GetComponentInChildren<Player>().NotifyOn();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            if (!selfCue)
            {
                collision.gameObject.GetComponentInChildren<Player>().NotifyOff();
            }
        }
    }
}
