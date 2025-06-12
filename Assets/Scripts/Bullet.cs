using DefaultNamespace;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = new Vector2(transform.localScale.x, 0) * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
       

       
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != 30 && collision.gameObject.layer != 29)    return;
        Destroy(gameObject);
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        else
        {
            EnemyBoss enemyBoss = collision.gameObject.GetComponent<EnemyBoss>();
            if (enemyBoss != null)
            {
                enemyBoss.TakeDamage(damage);
            }
        }
    }
} 