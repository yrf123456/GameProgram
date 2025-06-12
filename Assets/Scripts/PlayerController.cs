using DefaultNamespace;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Player movement speed
    public float jumpForce = 10f; // Jump force

    private Rigidbody2D rb; // Rigidbody component
    private bool isGrounded; // Whether the player is on the ground
    private Animator animator; // Animator controller

    public GameObject bulletPrefab; // Bullet prefab
    public Transform firePoint;     // Fire point

    public float attackRate = 0.5f; // Attack rate
    private float nextAttackTime = 0f; // Time for the next attack
    private float currAttackTime; // Time of current attack
    
    private bool isAtk; // Whether the player is in attack state
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody component
        animator = GetComponent<Animator>(); // Get Animator component
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal"); // Get horizontal input
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y); // Set player velocity

        // Control animation parameters
        animator.SetFloat("Speed", Mathf.Abs(move)); // Set speed parameter
        animator.SetBool("isJumping", !isGrounded); // Set jumping parameter
        animator.SetBool("isAtk", isAtk); // Set attack parameter

        if (Input.GetButtonDown("Jump") && isGrounded) // Check jump input and if grounded
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Apply jump velocity
        }

        // Attack logic
        if (Time.time >= nextAttackTime) // Check if attack is allowed
        {
            if (Input.GetButtonDown("Fire")) // Check attack input
            {
                Attack(); // Perform attack
                isAtk = true; // Set attack state
                nextAttackTime = Time.time + attackRate; // Update next attack time
                currAttackTime = Time.time; // Record current attack time
            }
        }

        if (isAtk && (Time.time - currAttackTime > 0.2f)) // Check if attack animation should end
        {
            isAtk = false;
        }

        // Flip character based on movement direction
        if (move != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(move) * Mathf.Abs(scale.x); // Flip X based on direction
            transform.localScale = scale;
        }
    }

    void Attack()
    {
        // Play shooting sound effect
        AudioManager.Instance.PlayShootSound();
        // Instantiate bullet and set its direction
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.transform.localScale = new Vector3(Mathf.Sign(transform.localScale.x), 1, 1);
    }

    public LayerMask targetLayers; // Select target layers in the Inspector

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Layer check
        if ((targetLayers & (1 << collision.gameObject.layer)) == 0)
            return;
       
        if (collision.contacts[0].normal.y > 0.5f) // Check if collision comes from below
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Layer check
        if ((targetLayers & (1 << collision.gameObject.layer)) == 0)
            return;
        isGrounded = false;
    }

    // Modify weapon attack rate
    public void SetAttackRate(float rate)
    {
        attackRate = rate;
    }
}
