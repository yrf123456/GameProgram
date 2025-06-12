using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class EnemyBoss : MonoBehaviour
    {
        public int maxHealth = 100;
        int currentHealth;

        public float moveSpeed = 2f;
        public float attackRange = 2f;
        public float viewRange = 6f;
        public float meleeChaseSpeedMultiplier = 2f;

        public Transform player;
        private bool movingRight = true;
        private Rigidbody2D rb;

        public float patrolDistance = 5f;
        private Vector2 startPosition;

        public HealthBar healthBarPrefab;
        private HealthBar healthBar;

        public Constant.EnemyState enemyState = Constant.EnemyState.Move;

        public float attackRate = 0.2f;
        public float attackMeleeRate = 0.5f;
        private float nextAttackTime = 0f;

        public GameObject bulletPrefab;
        public Transform firePoint;

        private Animator animator;

        public int MeleeDamage = 10;

        public enum BossEnemyState { FirstStage, SecondStage }
        public BossEnemyState bossEnemyState = BossEnemyState.FirstStage;

        private bool isMeleeAtk;
        private bool isAtk;
        private float currAttackTime;

        private bool isChasingPlayer = false;

        private float patrolTimer = 0f;
        private float patrolDirectionDuration = 1f;

        public virtual void Start()
        {
            player = GameObject.FindWithTag("PlayerRole").transform;
            currentHealth = maxHealth;
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            startPosition = transform.position;

            healthBar = Instantiate(healthBarPrefab);
            healthBar.target = transform;
            healthBar.SetHealth(currentHealth, maxHealth);
            healthBar.offset = new Vector3(-0.4f, 1.3f, 0);
        }

        void Update()
        {
            if (player == null) return;

            if (currentHealth <= maxHealth / 2 && bossEnemyState == BossEnemyState.FirstStage)
                ChangeState(BossEnemyState.SecondStage);

            animator.SetBool("isMeleeAtk", isMeleeAtk);
            animator.SetBool("isAtk", isAtk);

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            isChasingPlayer = false;

            // ==== Simulated Behavior Tree Structure ====
            // Root Selector Node:
            // 1. Sequence: If player visible AND in range → Attack
            // 2. Sequence: If player visible → Chase
            // 3. Fallback: Patrol

            if (CanSeePlayer())
            {
                if (IsInAttackRange(distanceToPlayer))
                {
                    enemyState = Constant.EnemyState.ChaseLookOnly;
                    FacePlayer();
                    rb.velocity = Vector2.zero;

                    if (Time.time >= nextAttackTime)
                    {
                        if (bossEnemyState == BossEnemyState.FirstStage)
                        {
                            AttackMelee();
                            nextAttackTime = Time.time + attackMeleeRate;
                            currAttackTime = Time.time;
                        }
                        else
                        {
                            Attack();
                            nextAttackTime = Time.time + attackRate;
                            currAttackTime = Time.time;
                        }
                    }
                }
                else
                {
                    enemyState = Constant.EnemyState.Move;
                    isChasingPlayer = true;

                    FacePlayer();
                    float direction = Mathf.Sign(player.position.x - transform.position.x);
                    float chaseSpeed = (bossEnemyState == BossEnemyState.FirstStage)
                        ? moveSpeed * meleeChaseSpeedMultiplier
                        : moveSpeed;

                    rb.velocity = new Vector2(chaseSpeed * direction, rb.velocity.y);
                    animator.SetFloat("Speed", Mathf.Abs(direction));
                }
            }
            else
            {
                enemyState = Constant.EnemyState.Move;

                patrolTimer += Time.deltaTime;
                if (patrolTimer >= patrolDirectionDuration)
                {
                    movingRight = !movingRight;
                    patrolTimer = 0f;
                }

                float move = movingRight ? 1f : -1f;
                rb.velocity = new Vector2(moveSpeed * 0.5f * move, rb.velocity.y);

                Vector3 scale = transform.localScale;
                scale.x = Mathf.Sign(move) * Mathf.Abs(scale.x);
                transform.localScale = scale;

                animator.SetFloat("Speed", Mathf.Abs(move));
            }

            if ((isAtk || isMeleeAtk) && (Time.time - currAttackTime > 0.2f))
            {
                isAtk = false;
                isMeleeAtk = false;
            }
        }

        private bool CanSeePlayer()
        {
            return Vector2.Distance(transform.position, player.position) < viewRange;
        }

        private bool IsInAttackRange(float distance)
        {
            return distance < attackRange;
        }

        private void FacePlayer()
        {
            float direction = player.position.x - transform.position.x;
            movingRight = direction > 0f;

            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(direction) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        public void AttackMelee()
        {
            isMeleeAtk = true;
            enemyState = Constant.EnemyState.Attack;

            FacePlayer();

            HealthController health = player.GetComponent<HealthController>();
            if (health != null)
            {
                health.TakeDamage(MeleeDamage);
            }
        }

        public virtual void Attack()
        {
            isAtk = true;
            enemyState = Constant.EnemyState.Attack;

            FacePlayer();

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.transform.localScale = new Vector3(Mathf.Sign(transform.localScale.x), 1, 1);
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
            SceneManager.LoadScene("Main");
        }

        public void ChangeState(BossEnemyState newState)
        {
            bossEnemyState = newState;
            switch (bossEnemyState)
            {
                case BossEnemyState.FirstStage:
                    attackRange = 2f;
                    break;
                case BossEnemyState.SecondStage:
                    attackRange = 5f;
                    break;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Vector3 pos = transform.position;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(pos.x - attackRange, pos.y + 0.1f), new Vector3(pos.x + attackRange, pos.y + 0.1f));

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(new Vector3(pos.x - viewRange, pos.y + 0.2f), new Vector3(pos.x + viewRange, pos.y + 0.2f));
        }
#endif
    }
}
