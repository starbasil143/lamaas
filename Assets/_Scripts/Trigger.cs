using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public bool destroySelf = true;
    public UnityEvent interactAction;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactAction.Invoke();   
            if (destroySelf)
            {
                Destroy(gameObject);
            }
        }
    }
}
