using UnityEngine;

public class ArchDisappearTrigger : MonoBehaviour
{
    public string ballTag = "GolfBall";
    public GameObject objectToHide;
    public int bonusScore = 1;

    private bool used = false;

    void OnTriggerEnter(Collider other)
    {
        TryUse(other);
    }

    void OnTriggerStay(Collider other)
    {
        TryUse(other);
    }

    void TryUse(Collider other)
    {
        if (used) return;
        if (!other.CompareTag(ballTag)) return;

        used = true;

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(bonusScore);
        }

        if (objectToHide != null)
        {
            objectToHide.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}