using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
    private Animator rangedEnemyAnimator;
    [SerializeField]
    private AudioSource audioSrc;
    [SerializeField]
    private AudioClip fireSound;

    private Scr_PauseManager pauseManager;

    [SerializeField]
    GameObject FireObj;
    //for multiple arrows
    public float arrowAngleOffset = 15f;



    public bool canAttack = true;
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

        if (!isDead && canAttack)
        {
            ShootingLogic();
        }
    }

    void ShootingLogic()
    {
        timer += Time.deltaTime;

        float distance = Vector2.Distance(FireObj.transform.position, player.transform.position);
        if (distance < detectDist)
        {
            Vector3 direction = player.transform.position - FireObj.transform.position;
            float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Flip the FireObj based on the player's position
            Vector3 localScale = FireObj.transform.localScale;
            if (player.transform.position.x > FireObj.transform.position.x)
            {
                localScale.y = Mathf.Abs(localScale.y);
                rot += 90;
            }
            else
            {
                localScale.y = -Mathf.Abs(localScale.y);
                rot -= 90;
            }
            FireObj.transform.localScale = localScale;
            FireObj.transform.rotation = Quaternion.Euler(0, 0, rot);

            if (timer > maxTime)
            {
                timer = 0;

                bowFireFunc();
            }
        }
    }

    void bowFireFunc()
    {
        rangedEnemyAnimator.Play("WoodBowManShoot");
        Invoke(nameof(FireBullet), 0.15f);
        Invoke(nameof(ResetAnimation), 0.3f);
    }

    void FireBullet()
    {
        float[] angles = { -arrowAngleOffset, 0f, arrowAngleOffset };
        float initialForce = 5f;

        // Get the direction from FireObj to the player
        Vector2 directionToPlayer = (player.transform.position - FireObj.transform.position).normalized;

        foreach (float angle in angles)
        {
            // Instantiate the arrow
            GameObject arrowInstance = Instantiate(bullet, bulletPos.position, Quaternion.Euler(0, 0, 0));

            // Calculate the final direction for the arrow
            float angleInRadians = angle * Mathf.Deg2Rad;
            Vector2 finalDirection = new Vector2(
                directionToPlayer.x * Mathf.Cos(angleInRadians) - directionToPlayer.y * Mathf.Sin(angleInRadians),
                directionToPlayer.x * Mathf.Sin(angleInRadians) + directionToPlayer.y * Mathf.Cos(angleInRadians)
            );

            // Set the arrow's rotation based on the final direction
            float finalAngle = Mathf.Atan2(finalDirection.y, finalDirection.x) * Mathf.Rad2Deg;
            arrowInstance.transform.rotation = Quaternion.Euler(0, 0, finalAngle - 90);

            // Add force to the arrow
            Rigidbody2D arrowRb = arrowInstance.GetComponent<Rigidbody2D>();
            arrowRb.AddForce(arrowInstance.transform.up * initialForce, ForceMode2D.Impulse);
        }

        // Play the firing sound
        audioSrc.clip = fireSound;
        audioSrc.Play();
    }


    private void ResetAnimation()
    {
        rangedEnemyAnimator.Play("WoodBowManIdle");
    }

    public void DeadFunc()
    {
        isDead = true;

        if (isDead)
        {
           rangedEnemyAnimator.Play("WoodBowManDie");
        }
    }

    public bool IsAttacking()
    {
        return rangedEnemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("WoodBowManShoot");
    }

    public float GetHitpoints()
    {
        return hitpoints;
    }

    public void CancelAttack()
    {
        canAttack = false;
        rangedEnemyAnimator.Play("WoodBowManBeaten");
        Invoke(nameof(ResumeAttack), 2f); 
    }

    private void ResumeAttack()
    {
        canAttack = true;
    }
}
