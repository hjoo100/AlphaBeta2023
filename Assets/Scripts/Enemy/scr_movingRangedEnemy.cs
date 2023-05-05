using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_movingRangedEnemy : MonoBehaviour
{
    [SerializeField]
    private scr_enemyBase enemyBase;
    [SerializeField]
    private scr_rangeEnemyMove moveSys;
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float hitpoints, maxhitpoints, dmg, rotateSpd, detectDist;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform bulletPos;
    [SerializeField]
    private float timer, maxTime;
    [SerializeField]
    private bool isDead = false;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Animator turretAnimator;
    [SerializeField]
    private AudioSource audioSrc;
    [SerializeField]
    private AudioClip fireSound;

    private Scr_PauseManager pauseManager;

    [SerializeField]
    GameObject FireObj;
    //for multiple arrows
    public float arrowAngleOffset = 15f;
    void Start()
    {
        moveSys = GetComponent<scr_rangeEnemyMove>();
        moveSys.setMoveSpd(moveSpeed);

        player = GameObject.FindGameObjectWithTag("Player");
        bullet.GetComponent<scr_enemyBullet>().dmg = dmg;
        

        pauseManager = FindObjectOfType<Scr_PauseManager>();

        enemyBase = GetComponent<scr_enemyBase>();
        enemyBase.hitpoints = hitpoints;
        enemyBase.MaxHitpoints = maxhitpoints;
    }

    void Update()
    {
        if (pauseManager.IsPaused())
        {
            return;
        }

        if (!isDead)
        {
            timer += Time.deltaTime;

            float distance = Vector2.Distance(FireObj.transform.position, player.transform.position);
            if (distance < detectDist)
            {
                Vector3 direction = player.transform.position - FireObj.transform.position;
                float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
                FireObj.transform.rotation = Quaternion.Euler(0, 0, rot - 90);

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
        float[] angles = { -arrowAngleOffset, 0f, arrowAngleOffset };
        float initialForce = 5f;

        foreach (float angle in angles)
        {
            GameObject arrowInstance = Instantiate(bullet, bulletPos.position, Quaternion.Euler(0, 0, 0));
            Quaternion arrowRotation = FireObj.transform.rotation * Quaternion.Euler(0, 0, angle + 135f);
            arrowInstance.transform.rotation = arrowRotation * Quaternion.Euler(0, 0, 22.5f);

            Vector3 adjustedRight = Quaternion.Euler(0, 180, 0) * arrowInstance.transform.right;
            arrowInstance.transform.right = adjustedRight;

            Rigidbody2D arrowRb = arrowInstance.GetComponent<Rigidbody2D>();
            arrowRb.AddForce(arrowInstance.transform.up * initialForce, ForceMode2D.Impulse);
        }

        audioSrc.clip = fireSound;
        audioSrc.Play();

        
    }


    private void ResetAnimation()
    {
        turretAnimator.Play("Idle");
    }

    public void DeadFunc()
    {
        isDead = true;
    }

    public bool IsAttacking()
    {
        return turretAnimator.GetCurrentAnimatorStateInfo(0).IsName("Firing");
    }

    public float GetHitpoints()
    {
        return hitpoints;
    }
}
