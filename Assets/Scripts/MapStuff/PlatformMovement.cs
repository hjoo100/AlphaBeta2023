using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlatformMovement : MonoBehaviour
{
    public Transform center; 
    public float speed = 5f; 
    public float radius = 5f;
    public float initialAngle;

    private float currentAngle; 

    private void Start()
    {
        currentAngle = initialAngle;
    }

    private void Update()
    {
        
        currentAngle += Time.deltaTime * speed;

        
        float x = center.position.x + radius * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = center.position.y + radius * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
        Vector3 newPosition = new Vector3(x, y, transform.position.z);

        
        transform.position = newPosition;
    }
}

