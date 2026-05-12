using UnityEngine;

public class ClubFloppyVisual : MonoBehaviour
{
    [Header("Floppy Feel")]
    public float floppyAmount = 30f;    //How much it wiggles
    public float floppySpeed = 8f;      //Has fast it wiggles
    public float movementSwayAmount = 5f;   //The reaction of the club based on the camera mvmt
    public float returnSmooth = 10f;    //How smooth the club rotates

    [Header("Limit")]
    public float maxAngle = 30f;    //Maximum angle the club's allowed to rotate

    private Quaternion baseLocalRotation;   //Club's original local rotation
    private Vector3 lastParentPosition;     //Parent position from the previous frame

    void Awake()
    {
        //Save the main location of the club so it doesnt slide off of screen
        baseLocalRotation = transform.localRotation;
        // Use the parent's position if the club is attached to another object
        if (transform.parent != null)
        {
            lastParentPosition = transform.parent.position;
        }
        else
        {
            lastParentPosition = transform.position;
        }
    }

    void LateUpdate()
    {
        //Track how muck the parent has moved since last frame
        Vector3 currentParentPosition = transform.parent != null ? transform.parent.position : transform.position;
      //CHeck how much it moved since last frame
        Vector3 movement = currentParentPosition - lastParentPosition;
        //Make the club sway in the opposite direction
        float swayX = -movement.y * movementSwayAmount * 120f;
        float swayY = movement.x * movementSwayAmount * 120f;
        //Add small idle mation so it seems like floppy
        float idleX = Mathf.Sin(Time.time * floppySpeed) * floppyAmount * 0.25f;
        float idleY = Mathf.Cos(Time.time * floppySpeed * 0.7f) * floppyAmount * 0.18f;
        //Keep the final angle from becoming too extreme
        swayX = Mathf.Clamp(swayX + idleX, -maxAngle, maxAngle);
        swayY = Mathf.Clamp(swayY + idleY, -maxAngle, maxAngle);
        
        // Apply the sway on top of the original rotation.
        Quaternion targetRotation = baseLocalRotation * Quaternion.Euler(swayX, swayY, 0f);
        
        //Make it smooth 
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRotation,
            Time.deltaTime * returnSmooth
        );

        //Save this frame's position for next movement check
        lastParentPosition = currentParentPosition;
    }
}
