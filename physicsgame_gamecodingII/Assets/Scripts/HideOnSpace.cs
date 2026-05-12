using System;
using UnityEngine;

public class HideOnSpace : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.SetActive(false);

        }
    }

    
}
