using UnityEngine;

public class GoalWinTrigger : MonoBehaviour
{
    public string ballTag = "GolfBall";

    private bool used = false;

    void OnTriggerEnter(Collider other)
    {
        if (used) return;

        if (other.CompareTag(ballTag))
        {
            used = true;

            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.WinGame();
            }
        }
    }
}