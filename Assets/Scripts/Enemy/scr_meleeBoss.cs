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
    public float meleeDmg = 30f;
    public float basicDmg = 30f;
    public float attackCD = 2.5f;
    public scr_enemyAttackArrow attackArrow;
    public float moveSpd = 5f;
    public float basicSpd = 1f;
    [SerializeField]
    public bool isAttacked = false, isinAir = false, isKnockedBack = false, isDead = false, isAlerted = false;
    [SerializeField]
    public bool attacking = false;

    [SerializeField]
    public bool isCharging = false, isDashing = false, isDefending = false,isResting = false;
    public float restTime = 0;
    public float maxRestTime = 2f;
    public float defendRate = 0;
    [SerializeField]
    private bool isAwake = false;

    public AudioSource enemyAudio;
    public AudioClip punchAudio;
    public AudioClip stompAudio;
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
        if(isCharging || !isAwake)
        {
            return;
        }
        if(isResting)
        {
            resting();
            return;
        }
        Attacking();
    }

    private void FixedUpdate()
    {
        
    }

    public void Attacking()
    {
        //agent.SetDestination(transform.position);

        //transform.LookAt(PlayerTransform);

        if (!isAttacked && !isDead && !moveSys.isKnockedBack)
        {
            // attack player
            if (player != null)
            {
                if (attackArrow.IsInRange(player.gameObject))
                {
                    alertEnemy();
                    
                    //do melee attack 
                    Debug.Log("Enemy attacking (melee)");
                    enemyAnimator.Play("Attacking");
                    isAttacked = true;
                    attacking = true;
                    enemyAudio.clip = punchAudio;
                    //cancel defence state if attacking
                    cancelDefence();
                    Invoke(nameof(playAudio), 0.5f);
                    Invoke(nameof(attackPlayer), 0.5f);
                    Invoke(nameof(stopAttackingAnim), 1.01f);
                   // Invoke(nameof(cancelDefence), 1.02f);
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
            isAwake = true;
            isAlerted = true;
            moveSys.isAlerted = true;
        }

    }

    public void attackPlayer()
    {
        attackArrow.attackEnemyInRange(meleeDmg);
    }

    public void playAudio()
    {
        enemyAudio.Play();
    }

    public void cancelDefence()
    {
        isDefending = false;
        moveSpd = basicSpd;
    }

    public void resting()
    {
       
            restTime -= Time.deltaTime;
            if(restTime <= 0)
            {
                isResting = false;
                restTime = maxRestTime;
            }

        
     
    }

    public void stompAttack()
    {
        //enable the stomp collider and attack player once
        attackArrow.enableStompCollider();
        //Play stomp audio
        enemyAudio.clip = stompAudio;
        playAudio();
        attackPlayer();
        

        //then disable the stomp collider
        attackArrow.disableStompCollider();

        //start resting to avoid player die too quick
        isResting = true;
        restTime = maxRestTime;
    }

    public bool getAwake()
    {
        return isAwake;
    }
}
