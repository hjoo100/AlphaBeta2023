using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PatrolingPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform pointA;
    [SerializeField]
    private Transform pointB;
    [SerializeField]
    private float speed = 1.0f;

    private Vector3 targetPosition;
    private Transform targetTransform;

    private void Start()
    {
        targetTransform = pointB;
        targetPosition = pointB.position;
    }

    private void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
        {
            if (targetTransform == pointB)
            {
                targetTransform = pointA;
            }
            else
            {
                targetTransform = pointB;
            }
            targetPosition = targetTransform.position;
        }
    }

}
