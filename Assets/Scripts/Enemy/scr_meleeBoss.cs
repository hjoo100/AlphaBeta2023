using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class scr_meleeBoss : MonoBehaviour
{
    private scr_enemyBase enemyBase;
    private NavMeshAgent agent;
    public Animator enemyAnimator;
    public Scr_PlayerCtrl player;
    public scr_meleeBossMove moveSys;

    public float enemyhitpoints = 50f;
    public float meleeDmg = 20f;
    public float attackCD = 2.5f;
    public scr_enemyAttackArrow attackArrow;
    public float moveSpd = 5f;

    public bool isAttacked = false, isinAir = false, isKnockedBack = false, isDead = false, isAlerted = false;
    public bool attacking = false;

    public bool isCharging = false, isDashing = false, isDefending = false;
    public float defendRate = 0;

    public AudioSource enemyAudio;
    public AudioClip punchAudio;
    // Start is called before the first frame update
    private void Awake()
    {
        enemyBase = GetComponent<scr_enemyBase>();
        enemyBase.hitpoints = enemyhitpoints;
        agent = GetComponent<NavMeshAgent>();
        //enemyAnimator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Scr_PlayerCtrl>();
        enemyAudio = GetComponent<AudioSource>();
    }
    void Start()
    {
        moveSys = GetComponent<scr_meleeBossMove>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Attacking();
    }

    public void Attacking()
    {
        //agent.SetDestination(transform.position);

        //transform.LookAt(PlayerTransform);

        if (!isAttacked && !isDead && !moveSys.isKnockedBack && !isDefending)
        {
            // attack player
            if (player != null)
            {
                if (attackArrow.IsInRange(player.gameObject))
                {
                    alertEnemy();
                    //attackArrow.attackEnemyInRange(meleeDmg);
                    //do melee attack 
                    Debug.Log("Enemy attacking (melee)");
                    enemyAnimator.Play("Attacking");

                    // add trigger to attack animation


                    isAttacked = true;
                    attacking = true;
                    enemyAudio.clip = punchAudio;
                    enemyAudio.Play();
                    Invoke(nameof(attackPlayer), 0.5f);
                    Invoke(nameof(stopAttackingAnim), 1.01f);
                    Invoke(nameof(ResetAttack), attackCD);
                }
            }

        }
    }

    void stopAttackingAnim()
    {
        Debug.Log("playing idle animation");
        //enemyAnimator.Play("Idle");
        attacking = false;
    }

    private void ResetAttack()
    {
        isAttacked = false;
    }

    public void DeadFunc()
    {
        isDead = true;
    }

    public void alertEnemy()
    {
        if (isAlerted == false)
        {
            isAlerted = true;
            moveSys.isAlerted = true;
        }

    }

    public void attackPlayer()
    {
        attackArrow.attackEnemyInRange(meleeDmg);
    }
}
