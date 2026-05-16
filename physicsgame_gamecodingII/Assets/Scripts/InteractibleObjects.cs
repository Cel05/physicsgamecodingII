using UnityEngine;

public class InteractibleObjects : MonoBehaviour
{
    public Color highLightColor = new Color(1f, 0.95f, 0.6f);

    [Range(0, 1f)] public float highLightStrangth = .4f;

    private Renderer objectRenderer;
    
    private Color originalColor;

    private bool isHighLighted = false;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.sharedMaterial.color;
        }

        else
        {
            Debug.Log($"Interactable object on {gameObject.name} has no renderer. highLightwont work");
        }
    }

    public void HighLight()

    {
        if (isHighLighted || objectRenderer == null)
        {
            Debug.Log("no obj renderer & ishighlighted is true");
            return;
        }

        objectRenderer.material.color = Color.Lerp(originalColor, highLightColor, highLightStrangth);
        isHighLighted = true;
    }

    public void UnhighLight()
    {
        if (!isHighLighted || objectRenderer == null) return;
        objectRenderer.material.color = originalColor;
        isHighLighted = false;
    }

  
}
