using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_BossRoomTrigger : MonoBehaviour
{
    public scr_BossRoomLockDoor lockRoomManager;
    public GameObject[] Bosses;
    public bool isLocked = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(isLocked == false)
        {
            if(collision.tag == "Player")
            {
                isLocked = true;
                lockRoomManager.StartCoroutine(lockRoomManager.MoveDoorToLockedPosition());
                awakeBosses();
            }
           
        }
    }

    void awakeBosses()
    {
        foreach(GameObject boss in Bosses)
        {
            if(boss.GetComponent<scr_enemyBase>().theEnemyType == scr_enemyBase.enemyType.UnstoppableBoss)
            {
                boss.GetComponent<scr_meleeBoss>().alertEnemy();
            }else
            {
                boss.GetComponent<scr_ShieldBossEnemy>().alertEnemy();
            }
        }
    }
}
