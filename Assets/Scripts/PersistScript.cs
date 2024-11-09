using Unity.VisualScripting;
using UnityEngine;

public class PersistScript : MonoBehaviour
{
    public static PersistScript instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }    
}
