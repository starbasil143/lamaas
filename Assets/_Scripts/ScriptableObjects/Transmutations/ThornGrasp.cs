using System.Collections;
using UnityEngine;

public class ThornGrasp : MonoBehaviour
{
    [Header("Drag Settings")]
    public float dragSpeed = 10f; // Speed at which the player is dragged to the enemy
    public float stopDistance = 0.5f; // Distance from the enemy at which the player stops being dragged

    public float returnSpeed = 15f; // Speed at which the thorn returns to the player
    public float maxLifetime = 1f; // Time before the thorn returns if no enemy is hit

    private GameObject player;
    private Rigidbody2D playerRigidbody;
    private bool isDragging = false; // Is Player getting dragged toward Target
    private Vector2 targetPosition;
    private Collider2D currentEnemy; // Reference to the enemy collider

    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;
    private BoxCollider2D currentCollider;

    private Enemy enemyScript;
    private Enemy.AttackBehavior originalEnemyBehavior;
    private bool isReturning = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentCollider = GetComponent<BoxCollider2D>();
        trailRenderer = GetComponent<TrailRenderer>();

        // Find the player object
        player = GameObject.FindGameObjectWithTag("Player");

        // Get the player's Rigidbody2D component
        if (player != null)
        {
            playerRigidbody = player.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogError("Player not found! Ensure the player has the 'Player' tag.");
        }

        // Start the coroutine to check for returning
        StartCoroutine(ReturnToPlayerAfterTimeout());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isReturning) return; // Prevent interactions while returning

        if ((other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Terrain") ) && playerRigidbody != null)
        {
            currentEnemy = other;
            if (other.gameObject.CompareTag("Enemy"))
            {
                // Save original enemy behavior
                originalEnemyBehavior = currentEnemy.GetComponentInChildren<Enemy>().attackBehavior;

                // Disable enemy's attacks
                currentEnemy.GetComponentInChildren<Enemy>().attackBehavior = Enemy.AttackBehavior.None;


            }
            //else if (other.gameObject.CompareTag("Terrain"))
            //{
            //    currentEnemy.GetComponent<Sprite>
            //}

            //Disable on Trigger
            spriteRenderer.enabled = false;
            trailRenderer.enabled = false;
            currentCollider.enabled = false;

            // Set the target position to the enemy's position
            targetPosition = other.transform.position;
            ChangeTargetColor(other);

            // Enable dragging
            isDragging = true;
        }
    }

    private void ChangeTargetColor(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.green;
        }
        if (other.gameObject.CompareTag("Terrain"))
        {
            other.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    private void FixedUpdate()
    {
        if (isDragging)
        {
            DragPlayerToTarget(currentEnemy);
        }
        else if (isReturning)
        {
            ReturnToPlayer();
        }
    }

    private void DragPlayerToTarget(Collider2D other)
    {
        // Calculate the direction toward the enemy
        Vector2 direction = (targetPosition - (Vector2)player.transform.position).normalized;

        // Apply velocity to the player
        playerRigidbody.linearVelocity = direction * dragSpeed;

        // Check if the player is close enough to stop
        if (Vector2.Distance(player.transform.position, targetPosition) <= stopDistance)
        {
            // Stop dragging and reset velocity
            isDragging = false;
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white; // Revert target color
                currentEnemy.GetComponentInChildren<Enemy>().attackBehavior = originalEnemyBehavior; // Revert enemy behavior
                playerRigidbody.linearVelocity = Vector2.zero;
            }
            else if (other.gameObject.CompareTag("Terrain") )
            {
                other.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    private void ReturnToPlayer()
    {
        // Calculate the direction toward the player
        Vector2 direction = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;

        // Move the thorn toward the player
        transform.position += (Vector3)(direction * returnSpeed * Time.deltaTime);

        // Destroy the thorn when it reaches the player
        if (Vector2.Distance(transform.position, player.transform.position) <= stopDistance)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator ReturnToPlayerAfterTimeout()
    {
        // Wait for the max lifetime
        yield return new WaitForSeconds(maxLifetime);

        if (!isDragging)
        {
            isReturning = true;
            //spriteRenderer.enabled = true; // Ensure thorn is visible when returning
            currentCollider.enabled = false; // Disable collisions during return
        }
    }
}
