using System.Collections;
using UnityEngine;

public class IceTomb : MonoBehaviour
{
    [Header("Settings")]
    public float freezeDuration = 5f; // Duration to freeze the enemy

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    // Reference to enemy components
    private Rigidbody2D enemyRb;
    private Enemy enemyScript;
    private Enemy.AttackBehavior originalEnemyBehavior;
    private SpriteRenderer enemySprite;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("You Have Hit an Enemy");

            //Disable player shots
            spriteRenderer.enabled = false;
            boxCollider.enabled = false;

            // Get references to the enemy's components
            enemyScript = other.GetComponentInChildren<Enemy>();
            enemyRb = other.GetComponent<Rigidbody2D>();
            enemySprite = other.GetComponentInChildren<SpriteRenderer>();
            
            ErrorChecking();

            if (enemyScript != null && enemyRb != null && enemySprite != null)
            {
                // Save the original attack behavior
                originalEnemyBehavior = enemyScript.attackBehavior;

                // Freeze the enemy
                StartCoroutine(FreezeEnemy());
            }
            else
            {
                Debug.LogError("Enemy does not have the required components.");
            }
        }
    }

    private void ErrorChecking()
    {
        if (enemySprite == null)
        {
            Debug.LogError("Enemy Sprite DNE");
        }
        if (enemyRb == null)
        {
            Debug.LogError("Enemy RB DNE");
        }
        if (enemyScript == null)
        {
            Debug.LogError("Enemy Script DNE");
        }
    }

    private IEnumerator FreezeEnemy()
    {
        // Disable movement and attacks
        enemyScript.attackBehavior = Enemy.AttackBehavior.None;
        if (enemyRb != null)
        {
            enemyRb.linearVelocity = Vector2.zero;
            enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        // Change enemy color to indicate freezing
        if (enemySprite != null)
            enemySprite.color = Color.cyan;
        Debug.Log("Freezing Enemy");
        // Wait for the freeze duration
        yield return new WaitForSeconds(freezeDuration);
        Debug.Log("Enemy will now UnFreeze");

        UnFreezeEnemy();
    }

    private void UnFreezeEnemy()
    {
        // Restore the enemy's original state
        if (enemyScript != null)
            enemyScript.attackBehavior = originalEnemyBehavior;

        if (enemyRb != null)
            enemyRb.constraints = RigidbodyConstraints2D.None;

        if (enemySprite != null)
            enemySprite.color = Color.white;
    }
}
