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

    public Scr_PlayerCtrl playerScript; 
    private Vector3 normalOffset = new Vector3(0, 0, -10);  // Normal camera offset
    private Vector3 jumpOffset = new Vector3(0, -5, -10);  // Offset when player is jumping


    void Start()
    {
        originalPos = transform.localPosition;
        initialMaxPos = maxPos;
        initialMinPos = minPos;

        camOffset = normalOffset;
    }
    private void FixedUpdate()
    {
        if (followTarget != null)
        {
            follow();
            ZoomCamera();
        }

        // If player is jumping or not grounded, use jump offset
        if (playerScript.isJumping || !playerScript.isGrounded)
        {
            SetCamOffset(jumpOffset, 0.8f);
        }
        else  // Otherwise, use normal offset
        {
            SetCamOffset(normalOffset, 0.5f);
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

    public void SetCamOffset(Vector3 newOffset, float duration)
    {
        StartCoroutine(LerpCamOffset(newOffset, duration));
    }

    IEnumerator LerpCamOffset(Vector3 newOffset, float duration)
    {
        Vector3 initialOffset = camOffset;
        float time = 0;
        while (time < duration)
        {
            camOffset = Vector3.Lerp(initialOffset, newOffset, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        camOffset = newOffset;
    }
}
