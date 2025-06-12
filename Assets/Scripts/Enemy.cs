using DefaultNamespace;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the enemy
    int currentHealth; // Current health

    public float moveSpeed = 2f; // Movement speed
    public float attackRange = 2f; // Attack range
    public Transform player; // Player reference
    private bool movingRight = true; // Whether moving right
    private Rigidbody2D rb; // Rigidbody component

    public float patrolDistance = 5f; // Max patrol distance
    private Vector2 startPosition; // Initial position

    public HealthBar healthBarPrefab; // Health bar prefab
    private HealthBar healthBar; // Instantiated health bar

    public Constant.EnemyState enemyState = Constant.EnemyState.Move; // Current enemy state

    public float attackRate = 0.5f; // Attack interval

    private float nextAttackTime = 0f; // Time for next attack

    public GameObject bulletPrefab; // Bullet prefab
    public Transform firePoint; // Bullet spawn point

    private float leftLimit; // Left patrol boundary
    private float rightLimit; // Right patrol boundary

    private Animator animator; // Animator component

    public float verticalAttackTolerance = 0.5f; // Allowed vertical offset for attack

    // Start is called before the first frame update
    public virtual void Start()
    {
        player = GameObject.FindWithTag("PlayerRole").gameObject.transform; // Find player
        SetDifficulty(); // Adjust attributes based on difficulty
        currentHealth = maxHealth; // Initialize current health
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody
        animator = GetComponent<Animator>(); // Get Animator
        startPosition = transform.position; // Record initial position

        // Set patrol boundaries
        leftLimit = startPosition.x - patrolDistance;
        rightLimit = startPosition.x + patrolDistance;

        // Create health bar
        healthBar = Instantiate(healthBarPrefab);
        healthBar.target = transform;
        healthBar.SetHealth(currentHealth, maxHealth);
    }

    void Update()
    {
        if (player == null) return;

        // Patrol logic
        if (enemyState == Constant.EnemyState.Move)
        {
            float move = movingRight ? 1f : -1f;
            rb.velocity = new Vector2(moveSpeed * move, rb.velocity.y);

            // Flip sprite based on direction
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(move) * Mathf.Abs(scale.x);
            transform.localScale = scale;
            animator.SetFloat("Speed", Mathf.Abs(move));

            // Change direction if reaching patrol boundary
            if ((movingRight && transform.position.x >= rightLimit) ||
                (!movingRight && transform.position.x <= leftLimit))
            {
                movingRight = !movingRight;
            }
        }
        else
        {
            rb.velocity = Vector2.zero; // Stay still
        }

        // Check attack conditions
        float horizontalDistance = Mathf.Abs(transform.position.x - player.position.x);
        float verticalDistance = Mathf.Abs(transform.position.y - player.position.y);
        if (horizontalDistance < attackRange && verticalDistance < verticalAttackTolerance)
        {
            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackRate;
            }
        }
        else
        {
            if (enemyState != Constant.EnemyState.Idle)
                enemyState = Constant.EnemyState.Move;
        }
    }

    // Attack behavior
    public virtual void Attack()
    {
        Debug.Log("Player is within attack range!");
        enemyState = Constant.EnemyState.Attack;

        // Flip toward the player
        float direction = player.position.x - transform.position.x;
        movingRight = direction > 0f;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(direction) * Mathf.Abs(scale.x);
        transform.localScale = scale;

        // Fire a bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.transform.localScale = new Vector3(Mathf.Sign(transform.localScale.x), 1, 1);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Uncomment if you want to change direction on wall collision
        // if (collision.gameObject.CompareTag("Wall"))
        // {
        //     movingRight = !movingRight;
        // }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject); // Destroy enemy object
        Destroy(healthBar.gameObject); // Destroy health bar
    }

    // Initialize attributes based on difficulty
    public void SetDifficulty()
    {
        float difficulty = GameManager.Instance.currDifficulty;
        maxHealth = (int)(maxHealth * difficulty);
        attackRange = attackRange * difficulty;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // Get current position
        Vector3 enemyPosition = transform.position;

        // Calculate patrol range
        float leftBoundX = enemyPosition.x - patrolDistance;
        float rightBoundX = enemyPosition.x + patrolDistance;

        Vector3 leftBound = new Vector3(leftBoundX, enemyPosition.y, enemyPosition.z);
        Vector3 rightBound = new Vector3(rightBoundX, enemyPosition.y, enemyPosition.z);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(leftBound, rightBound);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(enemyPosition, 0.2f);

        // Draw attack range
        float leftBoundAX = enemyPosition.x - attackRange;
        float rightBoundAX = enemyPosition.x + attackRange;
        Vector3 leftBoundA = new Vector3(leftBoundAX, enemyPosition.y, enemyPosition.z);
        Vector3 rightBoundA = new Vector3(rightBoundAX, enemyPosition.y, enemyPosition.z);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(leftBoundA, rightBoundA);
    }
#endif
}
