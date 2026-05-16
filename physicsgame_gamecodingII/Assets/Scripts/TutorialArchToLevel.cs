using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialArchToLevel : MonoBehaviour
{
    public string ballTag = "GolfBall";
    public string nextSceneName = "LV.1";
    
    private bool used = false;

    void OnTriggerEnter(Collider other)
    {
        if (used) return;
        if (!other.CompareTag(ballTag)) return;
        
        used = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneName);
    }
}
