
using UnityEngine;

public class HarmfulObjectScript : MonoBehaviour
{
    public float damageAmount;
    public bool isBlockable;
    public bool destroyOnContact;
    public bool canDamagePlayer;
    public bool canDamageEnemy;
    public bool thwartedByWalls;
    public GameObject Source;
    public GameObject ImpactObject;

    [SerializeField] private float existenceTime = 10f;
    private float existenceTimer = 0f;

    private void Awake()
    {
        
    }
    private void Update()
    {
        existenceTimer += Time.deltaTime;
        if (existenceTimer >= existenceTime)
        {
            Destroy(gameObject);
        }
    }
    public void DestroySelf()
    {
        if (ImpactObject)
        {
            GameObject impact = Instantiate(ImpactObject, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }

}
