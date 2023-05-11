using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_BossRoomLockDoor : MonoBehaviour
{
    public GameObject door; 
    public Transform lockedPosition;  
    public float doorMoveSpeed = 5f; 
    private bool isDoorLocked = false;

    

    public IEnumerator MoveDoorToLockedPosition()
    {
        isDoorLocked = true;
        while ((door.transform.position - lockedPosition.position).magnitude > 0.02f) 
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, lockedPosition.position, doorMoveSpeed * Time.deltaTime);
            yield return null;
        }
        door.transform.position = lockedPosition.position; 
    }
}
