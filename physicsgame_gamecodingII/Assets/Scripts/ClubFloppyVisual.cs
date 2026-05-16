using UnityEngine;

public class ClubFloppyVisual : MonoBehaviour
{
    [Header("Floppy Feel")]
    public float floppyAmount = 30f;
    public float floppySpeed = 8f;
    public float movementSwayAmount = 5f;
    public float returnSmooth = 10f;

    [Header("Limit")]
    public float maxAngle = 30f;

    private Quaternion baseLocalRotation;
    private Vector3 lastParentPosition;

    void Awake()
    {
        baseLocalRotation = transform.localRotation;

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
        Vector3 currentParentPosition = transform.parent != null ? transform.parent.position : transform.position;
        Vector3 movement = currentParentPosition - lastParentPosition;

        float swayX = -movement.y * movementSwayAmount * 120f;
        float swayY = movement.x * movementSwayAmount * 120f;

        float idleX = Mathf.Sin(Time.time * floppySpeed) * floppyAmount * 0.25f;
        float idleY = Mathf.Cos(Time.time * floppySpeed * 0.7f) * floppyAmount * 0.18f;

        swayX = Mathf.Clamp(swayX + idleX, -maxAngle, maxAngle);
        swayY = Mathf.Clamp(swayY + idleY, -maxAngle, maxAngle);

        Quaternion targetRotation = baseLocalRotation * Quaternion.Euler(swayX, swayY, 0f);

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRotation,
            Time.deltaTime * returnSmooth
        );

        lastParentPosition = currentParentPosition;
    }
}
