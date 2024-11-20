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
    public void TryAddToList()
    {
        if(_player.GetComponentInChildren<PlayerCasting>().hasObjectUnlocked(objectName) && !inList)
        {
            _player.GetComponentInChildren<PlayerCasting>().addObjectToInRangeList(gameObject);
            inList = true;
        }
    }
    public void TryRemoveFromList()
    {
        if(_player.GetComponentInChildren<PlayerCasting>().hasObjectUnlocked(objectName) && inList)
        {
            _player.GetComponentInChildren<PlayerCasting>().removeObjectFromInRangeList(gameObject);
            inList = false;
        } 
    }
}
