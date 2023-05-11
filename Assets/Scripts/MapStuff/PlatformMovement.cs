using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlatformMovement : MonoBehaviour
{
    public Transform center;

    private void Update()
    {
        // Keep the platform horizontal
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}

