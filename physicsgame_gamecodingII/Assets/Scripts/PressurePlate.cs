using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    public float weightThreshold = 5f;

    public bool lockOnActivate = false;

    public UnityEvent onActivated;
    
    public UnityEvent onDeactivated;

    public Transform plate;

    public float pressDepth = 0.05f;

    float currentWeight = 0f;
    bool isActivated = false;
    bool isLocked = false;
    Vector3 plateResetPos;
    Vector3 platePressedPos;
    
    HashSet<PhysicsObjects> objectsOnPlate = new HashSet<PhysicsObjects>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (plate != null)
        {
            plateResetPos = plate.localPosition;
            platePressedPos = plateResetPos + Vector3. down * pressDepth;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PhysicsObjects physOb = other.GetComponent<PhysicsObjects>();
        if (physOb == null) return;

        if (physOb.isHeld) return;
        
        currentWeight += physOb.puzzleWeight;
        //Debug.Log($"{other.gameObject.name} entered plate. total weight ; {currentWeight}");

        if (objectsOnPlate.Add(physOb))
        {
            currentWeight += physOb.puzzleWeight;
            CheckActivation();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isLocked) return;
        PhysicsObjects physicsOb = other.GetComponent<PhysicsObjects>();
        if(physicsOb == null ) return;

        if (objectsOnPlate.Remove(physicsOb)) ;
        (
            currentWeight -= PhysicsObjects.puzzleWeight);
            currentWeight -= Mathf.Max(0f, currentWeight);
            CheckDeactivation();
            
            

    }
    // Update is called once per frame
    void CheckActivation()
    {
        if(!isActivated && currentWeight >= weightThreshold)
        {
            isActivated = true;
            if(lockOnActivate) isLocked = true;
            
            onActivated.Invoke();
            Debug.Log("Pressure Plate activated");

            if (plate != null)
            {
                plate.localPosition = platePressedPos;
            }
        }
    }

    void CheckDeactivation()
    {
        if (isActivated && !isLocked && currentWeight < weightThreshold)
        {
            isActivated = false;
            onDeactivated.Invoke();
            Debug.Log("Pressure Plate deactivated");

            if (plate != null)
            {
                plate.localPosition = plateResetPos;
            }
        }
    }
}
