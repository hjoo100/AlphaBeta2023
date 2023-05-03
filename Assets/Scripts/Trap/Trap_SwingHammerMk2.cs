using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_SwingHammerMk2 : MonoBehaviour
{
    [SerializeField] private float swingSpeed = 2.0f;
    [SerializeField] private float swingAngle = 45.0f;

    private float startingRotation;

    private void Start()
    {
        startingRotation = transform.eulerAngles.z;
    }

    private void Update()
    {
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        transform.eulerAngles = new Vector3(0, 0, startingRotation + angle);
    }
}
