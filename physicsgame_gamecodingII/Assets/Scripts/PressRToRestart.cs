using UnityEngine;
using UnityEngine.SceneManagement;

public class PressRToRestart : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
