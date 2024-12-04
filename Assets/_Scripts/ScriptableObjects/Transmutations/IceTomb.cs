using UnityEngine;

public class IceTomb : MonoBehaviour
{
    [Header("Settings")]
    public float freezeDuration = 5f; // Duration to freeze the enemy
    public GameObject iceCubePrefab;
    private GameObject iceCube;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private Enemy enemyScript;
    private Rigidbody2D enemyRb;
    private SpriteRenderer enemySprite;
    private Animator enemyAnimator;
    private Transform enemyTransform;

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
            if (other.GetComponentInChildren<Enemy>().isFrozen == true)
            {
                Debug.Log("You've hit a frozen enemy");
                EnemyTakesDamageWhileFrozen(other);
                return;
            }

            Debug.Log("You have hit an enemy with Ice Tomb");

            spriteRenderer.enabled = false;
            boxCollider.enabled = false;

            enemyScript = other.GetComponentInChildren<Enemy>();
            enemyRb = other.GetComponent<Rigidbody2D>();
            enemySprite = other.GetComponentInChildren<SpriteRenderer>();
            enemyTransform = other.GetComponent<Transform>();
            enemyAnimator = other.GetComponentInChildren<Animator>();

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
        enemyAnimator.speed = 0f;
        iceCube = Instantiate(iceCubePrefab, enemyTransform.position + new Vector3(0,.5f,0), Quaternion.identity);

        if (enemySprite != null)
        {
            enemySprite.color = Color.cyan;
            Vector2 enemySize = enemySprite.bounds.size; // Get the enemy's size from its SpriteRenderer bounds
            iceCube.transform.localScale = enemySize * 1.5f;    // Adjust the iceCube's scale to match the enemy
        }

        Debug.Log("Enemy frozen");
    }

    public void UnFreezeEnemy()
    {
        enemyAnimator.speed = 1f;
        isFrozen = false;

        if (enemyRb != null)
        {
            enemyRb.constraints = RigidbodyConstraints2D.None;
        }

        if (enemySprite != null)
        {
            enemySprite.color = Color.white;
            Destroy(iceCube);
        }

        Debug.Log("Enemy unfrozen");
    }

    private void EnemyTakesDamageWhileFrozen(Collider2D enemy)
    {
        Debug.Log("Enemy has taken damage while frozen");
        UnFreezeEnemy();
    }
}
