using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectGrabber : MonoBehaviour
{
    [Header("Grab Settings")] [Tooltip("How far away the player can grab objects from")]
    public float grabRange;

    [Tooltip("How fast the held object moves to the hold point. higher = snappier")]
    public float holdSmoothing = 15f;
    
    public Transform holdPoint;

    public float throwForce = 15f;
    
    private Rigidbody heldObject;

    private bool isHolding = false;

    private InteractibleObjects currentHighLight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void FixedUpdate()
    {
        if(isHolding && heldObject != null) MoveHeldObject();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHighLight();
    }

    void TryGrab()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.forward * grabRange, Color.yellow, 0.5f);

        if (Physics.Raycast(ray, out hit, grabRange))
        {
            InteractibleObjects interactible = hit.collider.GetComponent<InteractibleObjects>();
            Debug.Log("Grabbed" + interactible);
            if (interactible != null)
            {
                Debug.Log("interactible does not equal null");
                heldObject = hit.collider.GetComponent<Rigidbody>();
                if (heldObject != null)
                {
                    heldObject.useGravity = false;
                    
                    heldObject.freezeRotation = true;
                    
                    heldObject.linearVelocity = Vector3.zero;
                    heldObject.angularVelocity = Vector3.zero;

                    interactible.UnhighLight();
                    currentHighLight = null;

                    isHolding = true;
                    Debug.Log($"Grabbed{heldObject.name}");
                }
            }
        }
    }

    void MoveHeldObject()
    {
        Vector3 targetPos = holdPoint.position;
        Vector3 currentPOs = heldObject.position;
        Vector3 newPos = Vector3.Lerp(currentPOs, targetPos, holdSmoothing * Time.fixedDeltaTime);
        
        heldObject.MovePosition(newPos);
    }

    void DropObject()
    {
        if (heldObject == null) return;
        
        heldObject.useGravity = true;
        heldObject.freezeRotation = false;

        heldObject = null;
        isHolding = false;
        Debug.Log("Dropped Object");
    }

    void ThrowObject()
    {
        if (heldObject == null) return;
        
        heldObject.useGravity = true;
        heldObject.freezeRotation = false;
        
        heldObject.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        
        heldObject = null;
        isHolding = false;
        Debug.Log("Threw Object");
    }

    public void OnGrabPerformed(InputAction.CallbackContext context)
    {
        if(isHolding) DropObject();
        else TryGrab();
        
        Debug.Log("Grabbed");
    }

    public void OnThrowPerformed(InputAction.CallbackContext context)
    {
        if(isHolding) ThrowObject();
    }

    void UpdateHighLight()
    {
        if (isHolding) return;
        
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * grabRange, Color.red);

        if (Physics.Raycast(ray, out hit, grabRange))
        {
            InteractibleObjects interactible = hit.collider.GetComponent<InteractibleObjects>();
            if (interactible != null)
            {
                if (currentHighLight != null && currentHighLight != interactible)
                {
                    currentHighLight.UnhighLight();
                    Debug.Log("Unhighlighted");
                }

                interactible.HighLight();
                currentHighLight = interactible;
                return;
            }
            
            if(currentHighLight != null)
                currentHighLight.UnhighLight();
            currentHighLight = null;
        }
    }
}
