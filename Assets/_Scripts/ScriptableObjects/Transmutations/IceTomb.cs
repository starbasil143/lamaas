using UnityEngine;

public class IceTomb : MonoBehaviour
{
    [Header("Settings")]
    public float freezeDuration = 5f; // Duration to freeze the enemy

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private Enemy enemyScript;
    private Rigidbody2D enemyRb;
    private SpriteRenderer enemySprite;

    private float freezeTimer = 0f; // Timer to track freeze duration
    private bool isFrozen = false; // Tracks if the enemy is frozen

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (isFrozen)
        {
            // Increment the timer while the enemy is frozen
            freezeTimer += Time.deltaTime;
            Debug.Log(freezeTimer);

            if (freezeTimer >= freezeDuration)
            {
                Debug.Log("Enemy will now unfreeze");
                UnFreezeEnemy();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.GetComponentInChildren<Enemy>().isFrozen)
            {
                Debug.Log("You've hit a frozen enemy");
                EnemyTakesDamageWhileFrozen(other);
                return;
            }

            Debug.Log("You have hit an enemy with Ice Tomb");

            spriteRenderer.enabled = false;

            enemyScript = other.GetComponentInChildren<Enemy>();
            enemyRb = other.GetComponent<Rigidbody2D>();
            enemySprite = other.GetComponentInChildren<SpriteRenderer>();

            if (enemyScript != null && enemyRb != null && enemySprite != null)
            {
                FreezeEnemy();
            }
            else
            {
                Debug.LogError("Enemy does not have the required components.");
            }
        }
    }

    private void FreezeEnemy()
    {
        isFrozen = true;
        freezeTimer = 0f; // Reset the freeze timer

        if (enemyRb != null)
        {
            enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        if (enemySprite != null)
        {
            enemySprite.color = Color.cyan;
        }

        Debug.Log("Enemy frozen");
    }

    private void UnFreezeEnemy()
    {
        isFrozen = false;

        if (enemyRb != null)
        {
            enemyRb.constraints = RigidbodyConstraints2D.None;
        }

        if (enemySprite != null)
        {
            enemySprite.color = Color.white;
        }

        Debug.Log("Enemy unfrozen");
    }

    private void EnemyTakesDamageWhileFrozen(Collider2D enemy)
    {
        Debug.Log("Enemy has taken damage while frozen");
        UnFreezeEnemy();
    }
}
