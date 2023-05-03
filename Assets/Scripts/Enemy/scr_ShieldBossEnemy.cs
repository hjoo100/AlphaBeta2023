using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class scr_ShieldBossEnemy : MonoBehaviour
{
    [SerializeField]
    private scr_enemyBase enemyBase;
    [SerializeField]
    private Animator enemyAnimator;
    [SerializeField]
    private Scr_PlayerCtrl player;
    [SerializeField]
    private scr_meleeBossMove moveSys;
    [SerializeField]
    private float enemyhitpoints = 50f;
    [SerializeField]
    private float meleeDmg = 30f;
    [SerializeField]
    private float basicDmg = 30f;
    [SerializeField]
    private float attackCD = 2.5f;
    [SerializeField]
    private scr_enemyAttackArrow attackArrow;
    [SerializeField]
    private float moveSpd = 5f;
    [SerializeField]
    private float basicSpd = 1f;
    [SerializeField]
    private bool isAttacked = false, isinAir = false, isDead = false, isAlerted = false;
    [SerializeField]
    private bool attacking = false;

    [SerializeField]
    private bool isCharging = false, isDefending = false, isResting = false, isStomping = false;
    [SerializeField]
    private float restTime = 0;
    [SerializeField]
    private float maxRestTime = 2f;
    [SerializeField]
    private float defendRate = 0;
    [SerializeField]
    private bool isAwake = false;
    [SerializeField]
    private AudioSource enemyAudio;
    [SerializeField]
    private AudioClip punchAudio;
    

    [SerializeField]
    private EnemySlamSkill slamSkill;

    // Start is called before the first frame update

    private Scr_PauseManager pauseManager;
    private void Awake()
    {
        pauseManager = FindObjectOfType<Scr_PauseManager>();

        enemyBase = GetComponent<scr_enemyBase>();
        enemyBase.hitpoints = enemyhitpoints;

        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Scr_PlayerCtrl>();
        enemyAudio = GetComponent<AudioSource>();
    }
    void Start()
    {
        moveSys = GetComponent<scr_meleeBossMove>();

        if (slamSkill != null)
        {
            slamSkill.Initialize("SlamSkill", SkillEnum.SkillType.Offensive, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseManager.IsPaused())
        {
            return; // Do not execute the rest of the Update logic if the game is paused
        }

        if (isCharging || !isAwake || isStomping)
        {
            return;
        }
        if (isResting)
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
                    int randInt = Random.Range(1, 3);
                    if (randInt == 1)
                    {
                        enemyAnimator.Play("Attacking");
                        isAttacked = true;
                        attacking = true;
                        enemyAudio.clip = punchAudio;
                        //cancel defence state if attacking
                        cancelDefence();
                        Invoke(nameof(playAudio), 0.92f);
                        Invoke(nameof(attackPlayer), 0.92f);
                        Invoke(nameof(stopAttackingAnim), 1.01f);
                        // Invoke(nameof(cancelDefence), 1.02f);
                        Invoke(nameof(ResetAttack), attackCD);
                        return;
                    }
                    else
                    {
                        enemyAnimator.Play("hitGroundAttack");
                        isAttacked = true;
                        attacking = true;
                        enemyAudio.clip = punchAudio;
                        cancelDefence();
                        Invoke(nameof(playAudio), 0.92f);
                        Invoke(nameof(hitGroundAttack), 0.92f);
                        Invoke(nameof(stopAttackingAnim), 1.01f);
                        // Invoke(nameof(cancelDefence), 1.02f);
                        Invoke(nameof(ResetAttack), attackCD);
                        return;

                    }

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
        if (restTime <= 0)
        {
            isResting = false;
            restTime = maxRestTime;
        }



    }

    
    public void hitGroundAttack()
    {

        attackArrow.enableHitGroundCollider();

        attackPlayer();

        //then disable the stomp collider
        attackArrow.disableHitGroundCollider();


    }

    public bool getAwake()
    {
        return isAwake;
    }

    public void stompComplete()
    {
        isStomping = false;
        moveSys.isStomping = false;
        isResting = true;
        restTime = maxRestTime;
    }

    //get value funcs
    public float getHitpoints()
    {
        return enemyhitpoints;
    }

    public bool getDefendBool()
    {
        return isDefending;
    }

    public float getDefendRate()
    {
        return defendRate;
    }

    public float getMoveSpd()
    {
        return moveSpd;
    }

    public bool getChargingBool()
    {
        return isCharging;
    }

    public bool getStompingBool()
    {
        return isStomping;
    }

    public bool getIsDead()
    {
        return isDead;
    }

    public bool getInairBool()
    {
        return isinAir;
    }

    public bool getAttackingBool()
    {
        return attacking;
    }

    public bool getAttackedBool()
    {
        return isAttacked;
    }

    public float getMeleeDmg()
    {
        return meleeDmg;
    }




    // set funcs
    public void setCharging(bool ischargingBool)
    {
        isCharging = ischargingBool;
    }

    public void setMeleeDmg(float meleeDmgVal)
    {
        meleeDmg = meleeDmgVal;
    }

    public void resetMeleeDmg()
    {
        meleeDmg = basicDmg;
    }

    public void setDefending(bool defendBool)
    {
        isDefending = defendBool;
    }

    public void setDefendingRate(float defendrateFloat)
    {
        defendRate = defendrateFloat;
    }

    public void setMoveSpd(float moveSpdFloat)
    {
        moveSpd = moveSpdFloat;
    }
}
