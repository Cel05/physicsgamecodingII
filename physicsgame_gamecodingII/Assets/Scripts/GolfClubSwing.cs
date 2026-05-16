using System.Collections.Generic;
using UnityEngine;

public class GolfClubSwing : MonoBehaviour
{
    [Header("Swing")]
    public float backAngle = 45f;
    public float hitAngle = -80f;
    public Vector3 swingAxis = Vector3.up;

    [Header("Swing Speed")]
    public float backSpeed = 500f;
    public float hitSpeed = 1000f;
    public float returnSpeed = 600f;

    [Header("Hit")]
    public Transform clubHeadPoint;
    public float hitRadius = 0.7f;
    public float hitPower = 18f;
    public float upwardPower = 2f;
    public string ballTag = "GolfBall";

    [Header("Aim")]
    public Transform aimSource;

    private Quaternion baseRotation;
    private Quaternion currentRotation;

    private int swingState = 0;
    private HashSet<Rigidbody> hitBalls = new HashSet<Rigidbody>();

    void Awake()
    {
        baseRotation = transform.localRotation;
        currentRotation = baseRotation;

        if (aimSource == null && Camera.main != null)
        {
            aimSource = Camera.main.transform;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && swingState == 0)
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.LoseScore(1);
            }

            hitBalls.Clear();
            swingState = 1;
        }

        Quaternion targetRotation = baseRotation;

        if (swingState == 1)
        {
            targetRotation = baseRotation * Quaternion.AngleAxis(backAngle, swingAxis.normalized);

            if (Quaternion.Angle(currentRotation, targetRotation) < 1f)
            {
                swingState = 2;
            }
        }
        else if (swingState == 2)
        {
            targetRotation = baseRotation * Quaternion.AngleAxis(hitAngle, swingAxis.normalized);
            CheckHitBall();

            if (Quaternion.Angle(currentRotation, targetRotation) < 1f)
            {
                swingState = 3;
            }
        }
        else if (swingState == 3)
        {
            targetRotation = baseRotation;

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

        currentRotation = Quaternion.RotateTowards(
            currentRotation,
            targetRotation,
            speed * Time.deltaTime
        );

        transform.localRotation = currentRotation;
    }

    void CheckHitBall()
    {
        if (clubHeadPoint == null) return;

        Collider[] hits = Physics.OverlapSphere(clubHeadPoint.position, hitRadius);

        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag(ballTag)) continue;

            Rigidbody rb = hit.attachedRigidbody;
            if (rb == null) continue;
            if (hitBalls.Contains(rb)) continue;

            hitBalls.Add(rb);

            Vector3 direction = aimSource != null ? aimSource.forward : transform.forward;
            direction.y += upwardPower * 0.1f;
            direction.Normalize();

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(direction * hitPower, ForceMode.Impulse);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (clubHeadPoint == null) return;

        Gizmos.DrawWireSphere(clubHeadPoint.position, hitRadius);
    }
}