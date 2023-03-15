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
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        follow();
    }   
    
    void Update()
    {
        
    }

    void follow()
    {
        Vector3 targetPos = followTarget.position + camOffset;
        targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minPos.y, maxPos.y);
        //Vector3 smoothedPos = Vector3.Lerp(followTarget.position, targetPos, smoothVal*Time.fixedDeltaTime);
        transform.position = targetPos;
    }
}
