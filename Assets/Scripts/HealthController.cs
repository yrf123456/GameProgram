using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    public int maxHP = 100;
    private int currentHP;

    public HealthBar healthBarPrefab;
    private HealthBar healthBar;

    // Auto-heal cooldown time
    public float autoHPCD = 2f;

    public float autoHP = 0.1f;

    // Auto-heal frequency
    public float autoHPFrequency = 0.5f;
    private float currAutoHPFrequency = 0.5f;
    private float autoHPTime = 0f;
    private bool isAutoHP = false;

    void Start()
    {
        currentHP = maxHP;

        // Create health bar
        healthBar = Instantiate(healthBarPrefab);
        healthBar.target = transform;
        healthBar.SetHealth(currentHP, maxHP);
        healthBar.offset = new Vector3(-0.5f, 0.6f, 0);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        healthBar.SetHealth(currentHP, maxHP);
        // Debug.Log("Took " + damage + " damage. Current health: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
        else
        {
            autoHPTime = Time.time;
            isAutoHP = false;
        }
    }

    private void Update()
    {
        if (currentHP > 0 && Time.time > autoHPTime + autoHPCD)
        {
            if (!isAutoHP && Time.time > currAutoHPFrequency + autoHPFrequency)
            {
                currAutoHPFrequency = Time.time;
                currentHP += (int)(autoHP * maxHP);
                if (currentHP >= maxHP)
                {
                    currentHP = maxHP;
                    isAutoHP = true;
                }

                healthBar.SetHealth(currentHP, maxHP);
                autoHPTime = Time.time;
            }
        }
    }

    void Die()
    {
        // Debug.Log("Player died!");
        Destroy(gameObject);
        Destroy(healthBar.gameObject);
        SceneManager.LoadScene("Main");
    }
}
