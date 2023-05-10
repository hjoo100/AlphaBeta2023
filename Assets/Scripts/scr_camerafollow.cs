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
    private Vector3 normalOffset = new Vector3(0, 0, -20);  // Normal camera offset
   // private Vector3 jumpOffset = new Vector3(0, -2, -20);  // Offset when player is jumping
    public float followSpeed = 0.05f; // The speed when the camera follows the player
    public float jumpFollowSpeed = 0.02f; // The speed when the camera follows the player when jumping

    private Coroutine currentLerpCoroutine;
    void Start()
    {
        originalPos = transform.localPosition;
        initialMaxPos = maxPos;
        initialMinPos = minPos;

        camOffset = normalOffset;

        playerScript = FindObjectOfType<Scr_PlayerCtrl>();
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

        // Use slower follow speed if the player is jumping
        float currentFollowSpeed = (playerScript.isJumping || !playerScript.isGrounded) ? jumpFollowSpeed : followSpeed;

        // Smoothly interpolate the camera position
        transform.position = Vector3.Lerp(transform.position, targetPos, currentFollowSpeed);
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

    /*
    public void SetCamOffset(Vector3 newOffset, float duration)
    {
        // If a lerp coroutine is already running, stop it
        if (currentLerpCoroutine != null)
        {
            StopCoroutine(currentLerpCoroutine);
        }

        // Start a new lerp coroutine and keep a reference to it
        currentLerpCoroutine = StartCoroutine(LerpCamOffset(newOffset, duration));
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
    }*/
}
