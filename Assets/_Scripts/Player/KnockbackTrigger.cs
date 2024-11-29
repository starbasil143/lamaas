using UnityEngine;

public class KnockbackTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float knockbackForce = 10f; // Adjustable knockback force

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object has the "enemy" tag
        if (other.CompareTag("Enemy"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();


            if (rb != null)
            {
                // Get the player's current velocity
                Vector2 currentVelocity = rb.linearVelocity;

                // Calculate the knockback direction (opposite of current movement)
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
                //if (currentVelocity == Vector2.zero)
                //{
                //    //knockbackDirection = (other.transform.position - transform.position).normalized;
                //}



                // Apply knockback force
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}
