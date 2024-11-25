using UnityEngine;

public class PlayerTObjectRange : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("TransmutationObject"))
        {
            collision.gameObject.GetComponent<TransmutationObject>().TryAddToList();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("TransmutationObject"))
        {
            collision.gameObject.GetComponent<TransmutationObject>().TryRemoveFromList();
        }
    }
}
