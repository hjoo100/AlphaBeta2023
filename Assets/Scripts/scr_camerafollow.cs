using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_camerafollow : MonoBehaviour
{
    public Transform followTarget;
    public Vector3 camOffset;
    public Vector2 maxPos, minPos; //for camera bound
    [Range(1, 15)]
    public float smoothVal;

    [Range(1, 10)]
    public float cameraZoom = 5f;

    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.2f;

    private Vector3 originalPos;
    private bool isShaking = false;

    public Vector2 initialMinPos, initialMaxPos;

    void Start()
    {
        originalPos = transform.localPosition;
        initialMaxPos = maxPos;
        initialMinPos = minPos;
    }
    private void FixedUpdate()
    {
        if (followTarget != null)
        {
            follow();
            ZoomCamera();
        }

    }

    void follow()
    {
        Vector3 targetPos = followTarget.position + camOffset;
        targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minPos.y, maxPos.y);
        transform.position = targetPos;
    }

    public void ResetCameraBounds()
    {
        minPos = initialMinPos;
        maxPos = initialMaxPos;
    }

    void ZoomCamera()
    {
        Camera mainCamera = Camera.main;
        float targetOrthographicSize = cameraZoom;
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetOrthographicSize, smoothVal * Time.fixedDeltaTime);
    }

    public void ShakeCamera()
    {
        if (!isShaking)
        {
            StartCoroutine(DoShake());
        }
    }

    IEnumerator DoShake()
    {
        isShaking = true;
        var pos = transform.localPosition;
        for (float t = 0; t <= shakeDuration; t += Time.deltaTime)
        {
            transform.localPosition = pos + Random.insideUnitSphere * shakeMagnitude;
            yield return null;
        }
        transform.localPosition = pos;
        isShaking = false;
    }
}
