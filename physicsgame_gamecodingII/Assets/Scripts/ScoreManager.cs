using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Score")]
    public int score = 20;
    public TextMeshProUGUI scoreText;

    [Header("End Texts")]
    public GameObject gameOverText;
    public GameObject winText;

    [Header("End Settings")]
    public bool freezeGameOnEnd = true;

    private bool gameEnded = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;

        if (gameOverText != null)
        {
            gameOverText.SetActive(false);
        }

        if (winText != null)
        {
            winText.SetActive(false);
        }

        UpdateText();
    }

    public void AddScore(int amount)
    {
        if (gameEnded) return;

        score += amount;
        UpdateText();
    }

    public void LoseScore(int amount)
    {
        if (gameEnded) return;

        score -= amount;

        if (score <= 0)
        {
            score = 0;
            UpdateText();
            GameOver();
            return;
        }

        UpdateText();
    }

    public void WinGame()
    {
        if (gameEnded) return;

        gameEnded = true;

        if (winText != null)
        {
            winText.SetActive(true);
        }

        if (gameOverText != null)
        {
            gameOverText.SetActive(false);
        }

        if (freezeGameOnEnd)
        {
            Time.timeScale = 0f;
        }
    }

    void GameOver()
    {
        if (gameEnded) return;

        gameEnded = true;

        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
        }

        if (winText != null)
        {
            winText.SetActive(false);
        }

        if (freezeGameOnEnd)
        {
            Time.timeScale = 0f;
        }
    }

    void UpdateText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}