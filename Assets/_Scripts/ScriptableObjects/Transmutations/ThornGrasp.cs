using UnityEngine;

public class ThornGrasp : MonoBehaviour
{
    [Header("Drag Settings")]
    public float dragSpeed = 10f; // Speed at which the player is dragged to the enemy
    public float stopDistance = 0.5f; // Distance from the enemy at which the player stops being dragged

    private GameObject player;
    private Rigidbody2D playerRigidbody;
    private bool isDragging = false;
    private Vector2 targetPosition;

    private void Start()
    {
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("") )&& playerRigidbody != null)
        {
            // Set the target position to the enemy's position
            targetPosition = other.transform.position;

            // Enable dragging
            isDragging = true;
        }
    }

    private void FixedUpdate()
    {
        if (isDragging)
        {
            DragPlayerToEnemy();
        }
    }

    private void DragPlayerToEnemy()
    {
        // Calculate the direction toward the enemy
        Vector2 direction = (targetPosition - (Vector2)player.transform.position).normalized;

        // Apply velocity to the player
        playerRigidbody.velocity = direction * dragSpeed;

        //

        // Check if the player is close enough to stop
        if (Vector2.Distance(player.transform.position, targetPosition) <= stopDistance)
        {
            // Stop dragging and reset velocity
            isDragging = false;
            playerRigidbody.velocity = Vector2.zero;
        }
    }
}
