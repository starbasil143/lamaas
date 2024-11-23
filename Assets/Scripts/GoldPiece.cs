using UnityEngine;

public class GoldPiece : MonoBehaviour
{
    ExpHandler handler;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        handler = FindFirstObjectByType<ExpHandler>();
        if (handler == null)
        {
            Debug.LogError("HANDLER DNE");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger is being detected");
        IncrementGold(collision);
    }

    private void IncrementGold(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            handler.goldAmount++;
            Debug.Log("Current Gold: " + handler.goldAmount);
            //PlayerPrefs.SetInt("goldAmount", goldAmount);
            Destroy(gameObject);
            Debug.Log("Object should be destroyed");
        }
 
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
