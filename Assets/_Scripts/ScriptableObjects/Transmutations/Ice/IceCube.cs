using UnityEngine;

public class IceCube : MonoBehaviour
{
    IceTomb iceTomb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        iceTomb = FindAnyObjectByType<IceTomb>();
        if (iceTomb == null)
        {
            Debug.LogError("ICETOMB DNE");
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Harm"))
        {
            Debug.Log("Hit Ice Cube");

            Destroy(other.gameObject);
            Destroy(gameObject);

            iceTomb.UnFreezeEnemy();


        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
