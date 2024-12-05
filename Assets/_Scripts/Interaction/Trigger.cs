
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public bool destroySelf = true;
    public bool onExit;
    public UnityEvent interactAction;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !onExit)
        {
            interactAction.Invoke();   
            if (destroySelf)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && onExit)
        {
            interactAction.Invoke();   
            if (destroySelf)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetFlag(string flagToSet)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerProgress>().progressFlags[flagToSet] = true;
    }
}
