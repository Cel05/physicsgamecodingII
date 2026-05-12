using UnityEngine;

public class ArchDisappearTrigger : MonoBehaviour
{
    //public settings for what can activate this trigger and what should happen after
    public string ballTag = "GolfBall";
    //The object disappears aft the ball passes. 
    public GameObject objectToHide;
    //Bonus score when the ball passes through
    public int bonusScore = 1;

    //Keeps track if this trigger has been used 
    private bool used = false;
    
    //Runs when another collider is in the trigger
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
        if (used) return; //prevents from reusing them
        //Ignore anything else then golf ball
        if (!other.CompareTag(ballTag)) return;
        //Mark this when golf ball touches the arch
        used = true;
        
        //+1 score if the ball passes the arch
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(bonusScore);
        }
        //Hides the asigned object
        if (objectToHide != null)
        {
            objectToHide.SetActive(false);
        }
        else
        {   //If no object was assigned, hide this trigger instead.
            gameObject.SetActive(false);
        }
    }
}