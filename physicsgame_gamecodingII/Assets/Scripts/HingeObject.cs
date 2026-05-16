using UnityEngine;
using UnityEngine.Events;

public class HingeObject : MonoBehaviour
{
    public float minAngle;
    public float maxAngle;

    public bool useSpring;
    public float springTargetAngle;
    public float springForce;
    public float springDamper;
    public UnityEvent OnReachMax;
    
    public UnityEvent OnReachMin;
    private HingeJoint hinge;
    bool maxEventFired = false;
    bool minEventFired = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ConfigureHinge()
    {
        JointLimits limits = hinge.limits;
        limits.min = minAngle;
        limits.max = maxAngle;
        limits.bounciness = 0f;
        limits.bounceMinVelocity = 0.2f;
        hinge.limits = limits;
        hinge.useLimits = true;

        if (useSpring)
        {
            JointSpring spring = hinge.spring;
            spring.targetPosition = springTargetAngle;
            spring.spring = springForce;
            spring.damper = springDamper;
            hinge.spring = spring;
            hinge.useSpring = true;
        }
        else
        {
            hinge.useSpring = false;
        }
        
    }
    
    public void DriveToMax()
    {
        SetMotorTarget(maxAngle);
    }

    public void DriveToMin()
    {
        SetMotorTarget(minAngle);
    }

    void SetMotorTarget(float targetAngle)
    {
        JointMotor motor = hinge.motor;
        motor.targetVelocity = targetAngle > hinge.angle ? 50f : -50f;
        motor.force = 100f;
        motor.freeSpin = false;
        hinge.motor = motor;
        hinge.useMotor = true;
    }
}
