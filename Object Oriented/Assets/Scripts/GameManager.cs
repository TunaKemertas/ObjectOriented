using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Fields (en az 5)
    public int score = 0;
    public float gameTimer = 60f; // saniye
    public int currentRound = 1;
    public int scoreToWin = 10;
    public bool isGameRunning = false;
    public float enemySpawnInterval = 2f;
    public int enemiesPerRound = 3;
    public Item itemPrefab;


    // Referanslar
    public GameObject enemyPrefab;
    public Transform enemyParent;
    public UIManager uiManager;

    private float spawnTimer = 0f;
    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        // Başlangıçta UI ayarları
        uiManager.UpdateScoreText(score);
        uiManager.UpdateTimerText(gameTimer);
        uiManager.ShowStartMenu(true);
    }

    void Update()
    {
        if (!isGameRunning) return;

        UpdateTimer();
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= enemySpawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies(enemiesPerRound);
        }
    }

    // Methods (en az 5)
    public void StartGame()
    {
        score = 0;
        currentRound = 1;
        gameTimer = 60f;
        isGameRunning = true;
        uiManager.ShowStartMenu(false);
        uiManager.ShowGameOver(false);
        uiManager.UpdateScoreText(score);
    }

    public void EndGame(bool won)
    {
        isGameRunning = false;
        uiManager.ShowGameOver(true, won);
    }

    public void AddScore(int amount)
    {
        score += amount;
        uiManager.UpdateScoreText(score);
        if (score >= scoreToWin)
        {
            EndGame(true);
        }
    }

    // Spawn method with return value
    public int SpawnEnemies(int count)
    {
        int spawned = 0;
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = new Vector2(Random.Range(-6f, 6f), Random.Range(-3f, 3f));
            GameObject e = Instantiate(enemyPrefab, pos, Quaternion.identity, enemyParent);
            activeEnemies.Add(e);
            spawned++;
        }
        return spawned;
    }

    // Overloaded Pause (overloaded method example)
    public void PauseGame()
    {
        PauseGame(true);
    }

    public void SpawnRandomItem(Vector2 pos)
    {
        if (itemPrefab != null)
        {
            Item.Spawn(itemPrefab, pos);
        }
    }


    public void PauseGame(bool pause)
    {
        isGameRunning = !pause ? true : false;
        Time.timeScale = pause ? 0f : 1f;
    }

    // Timer update
    private void UpdateTimer()
    {
        gameTimer -= Time.deltaTime;
        if (gameTimer < 0) gameTimer = 0;
        uiManager.UpdateTimerText(gameTimer);
        if (gameTimer <= 0f)
        {
            EndGame(false);
        }
    }

    // Utility to remove dead enemy
    public void NotifyEnemyDeath(GameObject enemy, int scoreValue)
    {
        if (activeEnemies.Contains(enemy)) activeEnemies.Remove(enemy);
        AddScore(scoreValue);
    }
}
