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

    [SerializeField]
    Animator turretAnimator;
    [SerializeField]
    AudioSource audioSrc;
    [SerializeField]
    AudioClip fireSound;




    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bullet.GetComponent<scr_enemyBullet>().dmg = dmg;
        turretAnimator = GetComponent<Animator>();



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
                transform.rotation = Quaternion.Euler(0, 0, rot - 90);

                if (timer > maxTime)
                {
                    timer = 0;

                    bowFireFunc();
                }
            }
        }


    }
    void bowFireFunc()
    {
        turretAnimator.Play("Firing");
        Invoke(nameof(fireBullet), 0.15f);
        Invoke(nameof(ResetAnimation), 0.3f);
    }
    void fireBullet()
    {
        Instantiate(bullet, bulletPos.position,Quaternion.identity);
        audioSrc.clip = fireSound;
        audioSrc.Play();
    }

    private void ResetAnimation()
    {
        turretAnimator.Play("Idle");
    }
    public void deadFunc()
    {
        isdead = true;
    }
}
