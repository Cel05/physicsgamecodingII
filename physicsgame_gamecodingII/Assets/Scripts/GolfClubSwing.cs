using System.Collections.Generic;
using UnityEngine;

public class GolfClubSwing : MonoBehaviour
{
    [Header("Swing")]
    public float backAngle = 45f; //how much club moves back
    public float hitAngle = -80f; //how far the club swings
    public Vector3 swingAxis = Vector3.up; //Axis of the club rotation swing

    [Header("Swing Speed")]
    public float backSpeed = 500f; //speed of the club coming back aft the hit
    public float hitSpeed = 1000f; //how fast it hits
    public float returnSpeed = 600f; //how fast the club comes back to its original position

    [Header("Hit")]
    public Transform clubHeadPoint; //empty obj at the head golf club
    public float hitRadius = 0.7f;  //area where it detects the ball
    public float hitPower = 18f;    //the power of the hit on the ball
    public float upwardPower = 2f;  //Add little bit of up direction to add bounciness
    public string ballTag = "GolfBall"; //so that only the tagged ball can be hit

    [Header("Aim")]
    public Transform aimSource; //the main camera

    private Quaternion baseRotation;    //club's base rotation
    private Quaternion currentRotation; 

    private int swingState = 0;
    //0 = ready
    //1 = moving back
    //2 = hit
    //3= return
    private HashSet<Rigidbody> hitBalls = new HashSet<Rigidbody>();
    //prevents from hitting the same ball multiple times in one hit

    void Awake()
    {
        //Save the starting point so the club can return to it after the swing
        baseRotation = transform.localRotation;
        currentRotation = baseRotation;
        
        //The main aim would be the main camera
        if (aimSource == null && Camera.main != null)
        {
            aimSource = Camera.main.transform;
        }
    }

    void Update()
    {
        //Start a new swing if ready with left mouse click
        if (Input.GetMouseButtonDown(0) && swingState == 0)
        {   //Each swing -1 pnt
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.LoseScore(1);
            }
            
            hitBalls.Clear();
            swingState = 1;
        }

        Quaternion targetRotation = baseRotation;
        //Move the club back before the hit
        if (swingState == 1)
        {
            targetRotation = baseRotation * Quaternion.AngleAxis(backAngle, swingAxis.normalized);
            //Once the club was moved back, move on to state 2
            if (Quaternion.Angle(currentRotation, targetRotation) < 1f)
            {
                swingState = 2;
            }
        }
        //Swing forward and check if the club hits the ball
        else if (swingState == 2)
        {
            targetRotation = baseRotation * Quaternion.AngleAxis(hitAngle, swingAxis.normalized);
            //During the swing check if the ball is within the radius
            CheckHitBall();
            //Aft the hit, move to state 3
            if (Quaternion.Angle(currentRotation, targetRotation) < 1f)
            {
                swingState = 3;
            }
        }
        //Return the club to original position
        else if (swingState == 3)
        {
            targetRotation = baseRotation;
            //When the club is back to normal, reset for another swing
            if (Quaternion.Angle(currentRotation, baseRotation) < 1f)
            {
                currentRotation = baseRotation;
                swingState = 0;
            }
        }
        
        float speed = returnSpeed;
        
        if (swingState == 1)
        {
            speed = backSpeed;
        }
        else if (swingState == 2)
        {
            speed = hitSpeed;
        }
        //Rotate the club toward the target 
        currentRotation = Quaternion.RotateTowards(
            currentRotation,
            targetRotation,
            speed * Time.deltaTime
        );
        //Apply the rotation to the club
        transform.localRotation = currentRotation;
    }

    void CheckHitBall()
    {   
        if (clubHeadPoint == null) return;
        
        //Look for colliders near the club head
        Collider[] hits = Physics.OverlapSphere(clubHeadPoint.position, hitRadius);

        foreach (Collider hit in hits)
        {   //Hit only the golf ball
            if (!hit.CompareTag(ballTag)) continue;
            
            Rigidbody rb = hit.attachedRigidbody;
            if (rb == null) continue;
            //Prevent the same ball being hit several times in one swing
            if (hitBalls.Contains(rb)) continue;

            hitBalls.Add(rb);

            //Hit the ball toward the camera aim
            //change from forward to left or right transform.right aimSource.left?
            Vector3 direction = aimSource != null ? aimSource.forward : transform.forward;
            //So that the ball has a small upward angle
            direction.y += upwardPower * 0.1f;
            direction.Normalize();
            
            //Reset the ball's current movement before the new hit
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            //Apply force to the ball
            rb.AddForce(direction * hitPower, ForceMode.Impulse);
        }
    }

    void OnDrawGizmosSelected()
    {      //Draw the detection radius in Scene view
        if (clubHeadPoint == null) return;
        //Show the hit radius in the scene view
        Gizmos.DrawWireSphere(clubHeadPoint.position, hitRadius);
    }
}