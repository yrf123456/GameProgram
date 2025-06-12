using UnityEngine;

namespace DefaultNamespace
{
    public class ExplodeEnemy : Enemy
    {
        public int damage = 20;
        public GameObject ExploadEff;

        // Set a fixed attack range, not affected by difficulty scaling
        private float fixedAttackRange = 1f; // You can change this to the desired fixed value

        public override void Start()
        {
            base.Start();
            canMove = false;
            attackRange = fixedAttackRange; // Override the value modified by SetDifficulty() in the base class
            enemyState = Constant.EnemyState.Idle;
            if (TryGetComponent<Rigidbody2D>(out var rigid))
            {       
                rigid.constraints = RigidbodyConstraints2D.FreezeAll;
            }       
        }

        public override void Attack()
        {
            HealthController Health = player.gameObject.GetComponent<HealthController>();
            if (Health != null)
            {
                Health.TakeDamage(damage);
                Instantiate(ExploadEff, transform.position, Quaternion.identity);
                Die();
            }
        }
    }
}
