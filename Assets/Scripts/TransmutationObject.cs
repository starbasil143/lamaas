using UnityEngine;

public class TransmutationObject : MonoBehaviour
{
    public TransmutationSOBase _objectTransmutation;
    public string objectName; 
    private GameObject _player;
    private bool inList = false;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    public void PerformTransmutation()
    {
        _objectTransmutation.PerformTransmutation(_player);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(_player.GetComponent<PlayerCasting>().hasObjectUnlocked(objectName) && !inList)
            {
                Debug.Log("Adding " + gameObject.ToString() + "to list");
                _player.GetComponent<PlayerCasting>().addObjectToInRangeList(gameObject);
                inList = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(_player.GetComponent<PlayerCasting>().hasObjectUnlocked(objectName) && inList)
            {
                _player.GetComponent<PlayerCasting>().removeObjectFromInRangeList(gameObject);
                inList = false;
            }
        }
    }
}
