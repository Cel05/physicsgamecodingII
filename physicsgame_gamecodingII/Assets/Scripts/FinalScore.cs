using TMPro;
using UnityEngine;

public class EndingScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;

    void Start()
    {
        //Make sure the screen is not frozen 
        Time.timeScale = 1f;
        
        //Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        //Get the score from each levels
        int level1 = RunScoreData.level1Score;
        int level2 = RunScoreData.level2Score;
        int total = RunScoreData.TotalScore();
        
        //Show the final result 
        if (finalScoreText != null)
        {
            finalScoreText.text =
                "Congratulations!\n\n" +
                "LV.1 Score: " + level1 + "\n" +
                "LV.2 Score: " + level2 + "\n\n" +
                "Total Score: " + total;
        }
    }
}