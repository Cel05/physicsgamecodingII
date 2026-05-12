using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public string gameSceneName = "LV.1";

    public void StartGame()
    {
        //Game starts normally
        Time.timeScale = 1f;
        
        //Cursor unlocked
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        //Reset scores before new play
        RunScoreData.ResetScores();
        
        //Load scene
        SceneManager.LoadScene(gameSceneName);
    }
}