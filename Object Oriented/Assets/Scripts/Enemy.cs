using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Fields (>=5)
    public string enemyName = "Grunt";
    public int health = 50;
    public int damage = 10;
    public float speed = 2f;
    public bool isAggressive = true;
    public int scoreValue = 1;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindObjectOfType<PlayerController>().transform;
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (playerTransform != null && isAggressive)
        {
            MoveTowards(playerTransform.position);
        }
    }

    // Methods
    public void MoveTowards(Vector2 target)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        rb.velocity = dir * speed;
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log($"{enemyName} took {dmg}. Health now {health}");
        if (health <= 0) Die();
    }

    private void Die()
    {
        Debug.Log($"{enemyName} died.");
        // Notify game manager
        if (gameManager != null) gameManager.NotifyEnemyDeath(this.gameObject, scoreValue);
        // Spawn an item sometimes
        if (Random.value < 0.4f)
        {
            SpawnItem();
        }
        Destroy(gameObject);
    }

    void SpawnItem()
    {
        // Basit spawn, GameManager veya Item prefab kullanabilirsin
        Item itemPrefab = Resources.Load<Item>("Prefabs/Item"); // alternatif olarak GameManager referansı kullan
        if (itemPrefab != null)
        {
            Instantiate(itemPrefab, transform.position, Quaternion.identity);
        }
    }

    // Overloaded Attack: with player or with damage amount
    public void Attack(PlayerController player)
    {
        if (player != null) player.TakeDamage(damage);
    }

    public void Attack(PlayerController player, int customDamage)
    {
        if (player != null) player.TakeDamage(customDamage);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController p = collision.gameObject.GetComponent<PlayerController>();
        if (p != null)
        {
            Attack(p);
        }
    }
}
