using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Score")]
    public int score = 20;
    public TextMeshProUGUI scoreText;

    [Header("End UI")]
    public GameObject winText;
    public GameObject gameOverText;

    [Header("Scene Flow")]
    public string nextLevelSceneName = "LV.2";
    public string endingSceneName = "Ending";
    public bool winGoesToNextLevel = true;
    public bool winGoesToEnding = false;

    [Header("Timing")]
    public float endDelay = 2f;

    private bool gameEnded = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {   //Make sure the game isn't frozen
        Time.timeScale = 1f;

        //Hide the cursor during gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        //Hide ending messages at the start of the level
        if (winText != null)
            winText.SetActive(false);

        if (gameOverText != null)
            gameOverText.SetActive(false);

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
        
        //If the score hits 0, game over.
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
        //Save this level's score before changing scenes.
        SaveCurrentLevelScore();

        if (winText != null)
            winText.SetActive(true);

        if (gameOverText != null)
            gameOverText.SetActive(false);

        if (winGoesToEnding)
        {
            StartCoroutine(LoadSceneAfterDelay(endingSceneName));
        }
        else if (winGoesToNextLevel)
        {
            StartCoroutine(LoadSceneAfterDelay(nextLevelSceneName));
        }
        else
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void GameOver()
    {
        if (gameEnded) return;

        gameEnded = true;

        if (gameOverText != null)
            gameOverText.SetActive(true);

        if (winText != null)
            winText.SetActive(false);
        //Restart current level after game over
        string currentSceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadSceneAfterDelay(currentSceneName));
    }

    void SaveCurrentLevelScore()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        //Save score of level 1 and 2
        if (sceneName == "LV.1")
        {
            RunScoreData.level1Score = score;
        }
        else if (sceneName == "LV.2")
        {
            RunScoreData.level2Score = score;
        }
    }

    IEnumerator LoadSceneAfterDelay(string sceneName)
    {
        //Freeze gameplay when the win or game over message is shown
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        yield return new WaitForSecondsRealtime(endDelay);

        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    void UpdateText()
    {
        //Update the score UI
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}