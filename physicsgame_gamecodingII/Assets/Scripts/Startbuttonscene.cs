using UnityEngine;
using UnityEngine.SceneManagement;

public class Startbuttonscene : MonoBehaviour
{
    public string sceneName = "Ingame";

    public void StartGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}
