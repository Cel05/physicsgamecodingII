using UnityEngine;

public class HideOnClick : MonoBehaviour
{
    public GameObject objectToHide;

    public void Hide()

    {
        if (objectToHide != null)
            objectToHide.SetActive(false);
    }
}
