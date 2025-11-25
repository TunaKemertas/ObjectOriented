using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text healthText;
    public TMP_Text timerText;
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public TMP_Text gameOverText;
    public Button startButton;
    public Button restartButton;

    void Start()
    {
        
        ShowStartMenu(true);
        gameOverPanel.SetActive(false);

        
        startButton.onClick.AddListener(OnStartPressed);
        restartButton.onClick.AddListener(OnRestartPressed);
    }

    private void OnStartPressed()
    {
        GameManager gm = GameObject.FindObjectOfType<GameManager>();
        if (gm != null)
            gm.StartGame();
    }

    private void OnRestartPressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    
    public void UpdateScoreText(int score)
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }

    public void UpdateHealthText(int health)
    {
        if (healthText != null)
            healthText.text = $"Health: {health}";
    }

    public void UpdateTimerText(float timer)
    {
        if (timerText != null)
            timerText.text = $"Time: {Mathf.CeilToInt(timer)}";
    }

   
    public void ShowStartMenu(bool show)
    {
        if (startPanel != null)
            startPanel.SetActive(show);
    }

    
    public void ShowGameOver(bool show, bool won = false)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(show);
            if (show && gameOverText != null)
            {
                gameOverText.text = won ? "You Win!" : "Game Over";
            }
        }
    }
}
