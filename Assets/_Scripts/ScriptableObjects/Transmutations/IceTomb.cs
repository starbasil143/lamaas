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

    private bool hasFrozenEnemy;


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
        //hasFrozenEnemy = GetComponentInChildren<Enemy>().isFrozen;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.GetComponentInChildren<Enemy>().isFrozen == true )
            {
                EnemyTakesDamageWhileFrozen(other);
                return;
            }
            Debug.Log("You Have Hit an Enemy with Ice Tomb");

            // Disable player shots
            spriteRenderer.enabled = false;
            enemyRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
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
                StartCoroutine( FreezeEnemy(other) );
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

    private IEnumerator FreezeEnemy(Collider2D other)
    {
        // Disable movement and attacks
        enemyRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        other.GetComponentInChildren<Enemy>().isFrozen = true;


        if (enemyRb != null)
        {
            enemyRb.linearVelocity = Vector2.zero;
            enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        // Change enemy color to indicate freezing
        if (enemySprite != null)
            enemySprite.color = Color.cyan;

        Debug.Log("Freezing Enemy");

        // Wait for the freeze duration or until unfreezed
        float elapsedTime = 0f;
        while (elapsedTime < freezeDuration && other.GetComponentInChildren<Enemy>().isFrozen == true)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            Debug.Log(elapsedTime);
        }

        // If still frozen after the duration, unfreeze
        if (other.GetComponentInChildren<Enemy>().isFrozen == true)
        {
            Debug.Log("Enemy will now UnFreeze");
            UnFreezeEnemy(other);
        }
    }

    public void UnFreezeEnemy(Collider2D enemy)
    {
        enemyRB.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.None;
        enemy.GetComponentInChildren<Enemy>().isFrozen = false;

        // Restore the enemy's original state
        if (enemyScript != null)
            enemyScript.attackBehavior = originalEnemyBehavior;

        if (enemyRb != null)
            enemyRb.constraints = RigidbodyConstraints2D.None;

        if (enemySprite != null)
            enemySprite.color = Color.white;

        EnableEnemyBehavior();
    }

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
    private void EnemyTakesDamageWhileFrozen(Collider2D enemy)
    {

        Debug.Log("Enemy Has Taken Damage while frozen");
        //iceTomb.GetComponent<IceTomb>().UnFreezeEnemy(enemy);
    }
}
