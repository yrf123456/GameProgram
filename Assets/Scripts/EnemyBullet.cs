using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = new Vector2(transform.localScale.x, 0) * speed;
    }

    

    public LayerMask targetLayers; // Select the target layers in the Inspector

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object belongs to the target layers
        if ((targetLayers & (1 << collision.gameObject.layer)) == 0)
            return;

        HealthController player = collision.gameObject.GetComponent<HealthController>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
