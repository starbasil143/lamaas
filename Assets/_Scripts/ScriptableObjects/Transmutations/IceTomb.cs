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

    private bool hasFrozenEnemy = false;


    private Enemy enemyScript;
    private Collider2D currentEnemy; // Reference to the enemy collider
    private Enemy.AttackBehavior originalEnemyBehavior;
    private Enemy.EnemyState originalEnemyState;
    private Enemy.HostileBehavior originalHostileBehavior;
    private Rigidbody2D enemyRB;
    private SpriteRenderer enemySprite;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        enemyRB = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("You Have Hit an Enemy");

            // Disable player shots
            spriteRenderer.enabled = false;
            //boxCollider.enabled = false; Needs to be disabled so the player can still take damage

            // Get references to the enemy's components
            enemyScript = other.GetComponentInChildren<Enemy>();
            enemyRb = other.GetComponent<Rigidbody2D>();
            enemySprite = other.GetComponentInChildren<SpriteRenderer>();
            currentEnemy = other.GetComponentInChildren<BoxCollider2D>();

            ErrorChecking();

            if (enemyScript != null && enemyRb != null && enemySprite != null)
            {
                // Save the original attack behavior
                originalEnemyBehavior = enemyScript.attackBehavior;
                originalEnemyState = enemyScript.currentState;
                originalHostileBehavior = enemyScript.hostileBehavior;

                // Freeze the enemy
                DisableEnemyBehavior();
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
        // Freeze the enemy's movement
        if (enemyRb != null)
        {
            enemyRb.velocity = Vector2.zero;
            enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        // Change enemy color to indicate freezing
        if (enemySprite != null)
            enemySprite.color = Color.cyan;

        Debug.Log("Freezing Enemy");

        // Wait for the freeze duration or until manually unfrozen
        float elapsedTime = 0f;
        while (elapsedTime < freezeDuration && hasFrozenEnemy)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        // Unfreeze if still frozen after the duration
        if (hasFrozenEnemy)
        {
            Debug.Log("Enemy will now UnFreeze");
            UnFreezeEnemy();
        }
    }

    private void UnFreezeEnemy()
    {
        if (enemyRb != null)
            enemyRb.constraints = RigidbodyConstraints2D.None; // Allow full movement

        hasFrozenEnemy = false;

        // Restore the enemy's original state
        if (enemyScript != null)
        {
            enemyScript.attackBehavior = originalEnemyBehavior;
            enemyScript.currentState = originalEnemyState;
            enemyScript.hostileBehavior = originalHostileBehavior;
        }

        if (enemySprite != null)
            enemySprite.color = Color.white; // Reset color

        Debug.Log("Enemy Unfrozen");
    }


    //private IEnumerator FreezeEnemy()
    //{
    //    // Disable movement and attacks
    //    enemyRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    //    hasFrozenEnemy = true;

    //    if (enemyRb != null)
    //    {
    //        enemyRb.velocity = Vector2.zero;
    //        enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;
    //    }

    //    // Change enemy color to indicate freezing
    //    if (enemySprite != null)
    //        enemySprite.color = Color.cyan;

    //    Debug.Log("Freezing Enemy");

    //    // Wait for the freeze duration or until unfreezed
    //    float elapsedTime = 0f;
    //    while (elapsedTime < freezeDuration && hasFrozenEnemy)
    //    {
    //        yield return null;
    //        elapsedTime += Time.deltaTime;
    //    }

    //    // If still frozen after the duration, unfreeze
    //    if (hasFrozenEnemy)
    //    {
    //        Debug.Log("Enemy will now UnFreeze");
    //        UnFreezeEnemy();
    //    }
    //}

    //private void UnFreezeEnemy()
    //{
    //    enemyRB.constraints = RigidbodyConstraints2D.None;
    //    hasFrozenEnemy = false;

    //    // Restore the enemy's original state
    //    if (enemyScript != null)
    //        enemyScript.attackBehavior = originalEnemyBehavior;

    //    if (enemyRb != null)
    //        enemyRb.constraints = RigidbodyConstraints2D.None;

    //    if (enemySprite != null)
    //        enemySprite.color = Color.white;

    //    EnableEnemyBehavior();
    //}

    private void DisableEnemyBehavior()
    {
        if (enemyScript != null)
        {
            enemyScript.attackBehavior = Enemy.AttackBehavior.None;
            enemyScript.currentState = Enemy.EnemyState.Idle;
            enemyScript.hostileBehavior = Enemy.HostileBehavior.None;
        }
    }

    private void EnableEnemyBehavior()
    {
        if (enemyScript != null)
        {
            enemyScript.attackBehavior = originalEnemyBehavior; // Revert enemy behavior
            enemyScript.currentState = originalEnemyState;
            enemyScript.hostileBehavior = originalHostileBehavior;
        }
    }

    public void EnemyTakesDamage()
    {
        if (hasFrozenEnemy)
        {
            Debug.Log("Enemy Has Taken Damage while frozen");
            UnFreezeEnemy();
        }
    }
}
