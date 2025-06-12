using DefaultNamespace;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;

    public float moveSpeed = 2f;
    public float attackRange = 2f;
    public Transform player;
    private bool movingRight = true;
    private Rigidbody2D rb;

    public float patrolDistance = 5f;
    private Vector2 startPosition;

    public HealthBar healthBarPrefab;
    private HealthBar healthBar;

    public Constant.EnemyState enemyState = Constant.EnemyState.Move;

    public float attackRate = 0.5f;
    private float nextAttackTime = 0f;

    public GameObject bulletPrefab;
    public Transform firePoint;

    private float leftLimit;
    private float rightLimit;

    private Animator animator;

    public float verticalAttackTolerance = 0.5f;

    // Movement control flag (used by subclasses)
    public bool canMove = true;

    public virtual void Start()
    {
        player = GameObject.FindWithTag("PlayerRole").gameObject.transform;
        SetDifficulty();
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;

        leftLimit = startPosition.x - patrolDistance;
        rightLimit = startPosition.x + patrolDistance;

        healthBar = Instantiate(healthBarPrefab);
        healthBar.target = transform;
        healthBar.SetHealth(currentHealth, maxHealth);
    }

    void Update()
    {
        if (player == null) return;

        float horizontalDistance = Mathf.Abs(transform.position.x - player.position.x);
        float verticalDistance = Mathf.Abs(transform.position.y - player.position.y);

        float scoreAttack = ScoreAttack(horizontalDistance, verticalDistance);
        float scorePatrol = ScorePatrol(horizontalDistance);

        if (scoreAttack > scorePatrol)
        {
            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackRate;
            }
        }
        else
        {
            if (canMove)
                Patrol();
            else
                enemyState = Constant.EnemyState.Idle;
        }
    }

    protected virtual void Patrol()
    {
        enemyState = Constant.EnemyState.Move;

        float move = movingRight ? 1f : -1f;
        rb.velocity = new Vector2(moveSpeed * move, rb.velocity.y);

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(move) * Mathf.Abs(scale.x);
        transform.localScale = scale;
        animator.SetFloat("Speed", Mathf.Abs(move));

        if ((movingRight && transform.position.x >= rightLimit) ||
            (!movingRight && transform.position.x <= leftLimit))
        {
            movingRight = !movingRight;
        }
    }

    public virtual void Attack()
    {
        enemyState = Constant.EnemyState.Attack;

        float direction = player.position.x - transform.position.x;
        movingRight = direction > 0f;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(direction) * Mathf.Abs(scale.x);
        transform.localScale = scale;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.transform.localScale = new Vector3(Mathf.Sign(transform.localScale.x), 1, 1);

        Debug.Log("Enemy attacks player!");
    }

    private float ScoreAttack(float hDist, float vDist)
    {
        if (hDist > attackRange || vDist > verticalAttackTolerance) return 0f;
        return 1f - (hDist / attackRange);
    }

    private float ScorePatrol(float hDist)
    {
        return hDist > attackRange ? 1f : 0.2f;
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
        Destroy(gameObject);
        Destroy(healthBar.gameObject);
    }

    public void SetDifficulty()
    {
        float difficulty = GameManager.Instance.currDifficulty;
        maxHealth = (int)(maxHealth * difficulty);
        attackRange = attackRange * difficulty;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 enemyPosition = transform.position;

        float leftBoundX = enemyPosition.x - patrolDistance;
        float rightBoundX = enemyPosition.x + patrolDistance;
        Vector3 leftBound = new Vector3(leftBoundX, enemyPosition.y, enemyPosition.z);
        Vector3 rightBound = new Vector3(rightBoundX, enemyPosition.y, enemyPosition.z);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(leftBound, rightBound);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(enemyPosition, 0.2f);

        float leftBoundAX = enemyPosition.x - attackRange;
        float rightBoundAX = enemyPosition.x + attackRange;
        Vector3 leftBoundA = new Vector3(leftBoundAX, enemyPosition.y, enemyPosition.z);
        Vector3 rightBoundA = new Vector3(rightBoundAX, enemyPosition.y, enemyPosition.z);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(leftBoundA, rightBoundA);
    }
#endif
}
