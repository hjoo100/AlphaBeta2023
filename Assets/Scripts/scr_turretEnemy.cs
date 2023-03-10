using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_turretEnemy : MonoBehaviour
{
    public float hitpoints, maxhitpoints, dmg, rotateSpd,detectDist;
    public GameObject bullet;
    public Transform bulletPos;
    public float timer, maxTime;

    public bool isdead = false;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bullet.GetComponent<scr_enemyBullet>().dmg = dmg;

    }

    // Update is called once per frame
    void Update()
    {
        if (isdead == false)
        {
            timer += Time.deltaTime;

            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < detectDist)
            {
                Vector3 direction = player.transform.position - transform.position;
                float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, rot - 30);

                if (timer > maxTime)
                {
                    timer = 0;
                    fireBullet();
                }
            }
        }


    }

    void fireBullet()
    {
        Instantiate(bullet, bulletPos.position,Quaternion.identity);
    }

    public void deadFunc()
    {
        isdead = true;
    }
}
