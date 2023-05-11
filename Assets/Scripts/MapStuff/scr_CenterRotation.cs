using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_CenterRotation : MonoBehaviour
{
    public float speed = 5f;

    private void Update()
    {
        // Rotate the center object
        transform.Rotate(Vector3.forward, speed * Time.deltaTime);
    }
}
