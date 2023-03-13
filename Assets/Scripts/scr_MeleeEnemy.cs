using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class scr_MeleeEnemy : MonoBehaviour
{
    private scr_enemyBase enemyBase;
    private NavMeshAgent agent;
    public Animator enemyAnimator;
    public Scr_PlayerCtrl player;
    public scr_meleeEnemyMove moveSys;

    public float enemyhitpoints = 50f;
    public float meleeDmg = 20f;
    public float attackCD = 1f;
    public scr_enemyAttackArrow attackArrow;
    public float moveSpd = 5f;

    public bool isAttacked = false,isinAir = false, isKnockedBack = false, isDead = false,isAlerted = false;
    public bool attacking = false;

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
        moveSys = GetComponent<scr_meleeEnemyMove>();
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

        if (!isAttacked && !isDead)
        {
            // attack player
            if (player != null)
            {
                if (attackArrow.IsInRange(player.gameObject))
                {
                    alertEnemy();
                    attackArrow.attackEnemyInRange(meleeDmg);
                    //do melee attack 
                    Debug.Log("Enemy attacking (melee)");
                    enemyAnimator.Play("Attacking");
                    player.takeDmg(meleeDmg);
                    // add trigger to attack animation


                    isAttacked = true;
                    enemyAudio.clip = punchAudio;
                    enemyAudio.Play();
                    Invoke(nameof(stopAttackingAnim), 0.2f);
                    Invoke(nameof(ResetAttack), attackCD);
                }
            }  
              
        }
    }

    void stopAttackingAnim()
    {
        enemyAnimator.Play("Idle");
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
}
