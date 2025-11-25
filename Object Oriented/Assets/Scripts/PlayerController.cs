using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Fields (>=5)
    public string playerName = "Hero";
    public float speed = 5f;
    public int maxHealth = 100;
    public int health;
    public bool hasWeapon = false;
    public int attackPower = 25;
    public int score = 0;

    // References
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public GameManager gameManager;
    public UIManager uiManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        uiManager.UpdateHealthText(health);
    }

    void Update()
    {
        // Movement input
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // Attack key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryAttack();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moveInput.normalized * speed;
    }

    // Methods
    public void TryAttack()
    {
        // basit: etraftaki düşmanlara kapsadayla damage ver
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.2f);
        foreach (var h in hits)
        {
            Enemy enemy = h.GetComponent<Enemy>();
            if (enemy != null)
            {
                Attack(enemy);
                break; // sadece bir düşmana vur
            }
        }
    }

    public void Attack(Enemy enemy)
    {
        if (enemy == null) return;
        enemy.TakeDamage(attackPower);
    }

    // Overloaded Heal
    public void Heal()
    {
        Heal(10);
    }

    public void Heal(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
        uiManager.UpdateHealthText(health);
    }

    public bool TakeDamage(int dmg)
    {
        health -= dmg;
        uiManager.UpdateHealthText(health);
        Debug.Log($"{playerName} took {dmg} damage. Health: {health}");
        if (health <= 0)
        {
            Die();
            return false;
        }
        return true;
    }

    private void Die()
    {
        Debug.Log($"{playerName} died.");
        gameManager.EndGame(false);
        // disable movement
        rb.velocity = Vector2.zero;
        this.enabled = false;
    }

    // Item pickup detection (OnTriggerEnter2D kullanımı)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            item.Use(this); // item ile etkileşim
            Destroy(collision.gameObject);
        }
    }

    public int GetHealth()
    {
        return health;
    }
}
