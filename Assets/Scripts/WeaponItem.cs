using UnityEngine;

namespace DefaultNamespace
{
    public class WeaponItem : MonoBehaviour
    {
        public float attackRate = 0.1f;
        public LayerMask targetLayers; // Select target layers in the Inspector

        void OnCollisionEnter2D(Collision2D collision)
        {
            // Check if the collided object belongs to the target layers
            if ((targetLayers & (1 << collision.gameObject.layer)) == 0)
                return;

            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.SetAttackRate(attackRate);
                Destroy(gameObject);
            }
        }
    }
}
