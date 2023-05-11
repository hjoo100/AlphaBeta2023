using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Trap_SwingHammerController : MonoBehaviour
{
    [SerializeField] 
    private float swingSpeed = 3f;

    [SerializeField]
    private float maxSwingAngle = 45f;

    

    private float currentAngle;

    void Update()
    {
        currentAngle = maxSwingAngle * Mathf.Sin(Time.time * swingSpeed);
        transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
    }

    
}