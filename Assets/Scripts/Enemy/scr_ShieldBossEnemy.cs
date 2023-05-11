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
    private scr_ShieldBossMove moveSys;
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
    private float shieldVal = 85f; //maxShieldVal = 85f;

    [SerializeField]
    private EnemySlamSkill slamSkill;
    public float lastSlamTime = 0;

    [SerializeField]
    private bool isShieldBroken = false;
   

    private Scr_PauseManager pauseManager;

    
    public bool isJumpSlashing = false;


    public EnemyJavelinBarrageSkill JavelinBarrageSkillObj;

    
    public bool isUsingBarrageSkill = false;

    public AudioClip BarrageSkillSound;

    public AudioClip SlamSkillSound;

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
        moveSys = GetComponent<scr_ShieldBossMove>();

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

        if (isCharging || !isAwake)
        {
            return;
        }
        if (isResting)
        {
            resting();
            return;
        }

        if(enemyBase.shieldVal <= 0 && !isShieldBroken)
        {
            isShieldBroken = true;
            AddNewSkillToEnemy();
        }
        Attacking();
    }

    private void FixedUpdate()
    {
        if (slamSkill != null)
        {
            slamSkill.FixedUpdateSkill(gameObject);
        }
    }

    public void Attacking()
    {
        //agent.SetDestination(transform.position);

        //transform.LookAt(PlayerTransform);

        // Check if enough time has passed since the last slam
        if (Time.time < lastSlamTime + attackCD)
        {
            return; // Do not attempt to attack if the cooldown has not passed
        }

        if (isJumpSlashing)
        {
            return; // Do not attempt to attack if the enemy is performing a jumpslash
        }

        if (!isAttacked && !isDead && !moveSys.isKnockedBack && !isUsingBarrageSkill)
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
                        enemyAnimator.Play("Attack");
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
                        enemyAnimator.Play("Attack");
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

    public float GetShieldVal()
    {
        return shieldVal;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (slamSkill != null && slamSkill.IsSkillActive)
        {
            slamSkill.HandleCollision(gameObject, collision);
        }
    }

    private void CheckPlayerInLandingZone()
    {
        // Define a radius for the landing zone
        float landingZoneRadius = 1.5f;

        // Get the player's Rigidbody2D component
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // Check if the player is in the landing zone
        if (distanceToPlayer <= landingZoneRadius)
        {
            // Calculate the knockback force
            Vector2 knockbackForce = (player.transform.position - transform.position).normalized * 5f;

            // Apply the knockback force to the player
            playerRb.AddForce(knockbackForce, ForceMode2D.Impulse);
        }
    }

    public IEnumerator DisableCollisionWithPlayer(float duration)
    {
        // Get all the colliders attached to the enemy
        Collider2D[] enemyColliders = GetComponents<Collider2D>();
        Collider2D[] playerColliders = player.GetComponents<Collider2D>();

        // Disable the collision between each enemy collider and the player collider
        foreach (Collider2D enemyCollider in enemyColliders)
        {

            foreach (Collider2D playercollider in playerColliders)
            {
                Physics2D.IgnoreCollision(enemyCollider, playercollider, true);
            }
        }

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Enable the collision between each enemy collider and the player collider
        foreach (Collider2D enemyCollider in enemyColliders)
        {
            foreach (Collider2D playercollider in playerColliders)
            {
                Physics2D.IgnoreCollision(enemyCollider, playercollider, false);
            }
            
        }
    }

    public void invokeCheckLandingZone(float time)
    {
        Invoke(nameof(CheckPlayerInLandingZone), time);
    }

    public void StartDisableCollisionCoroutine(float duration)
    {
        StartCoroutine(DisableCollisionWithPlayer(duration));
    }

    private void AddNewSkillToEnemy()
    {
        

        // add skill holder component to this enemy
        scr_EnemySkillHolder NewSkillHolder = gameObject.AddComponent<scr_EnemySkillHolder>();

        


        NewSkillHolder.skill = JavelinBarrageSkillObj;
        NewSkillHolder.cooldownTime = 8f; 
        NewSkillHolder.activeTime = 1.5f;

        JavelinBarrageSkillObj.BindLaunchers();
        
        NewSkillHolder.state = scr_EnemySkillHolder.SkillState.cooldown;
    }

    public void PlayJumpSlashAnimation()
    {
        if (!isJumpSlashing && !attacking && !isUsingBarrageSkill)
        {
            isJumpSlashing = true;
            enemyAnimator.Play("JumpSlash");
            GetComponent<AudioSource>().clip = SlamSkillSound;
            GetComponent<AudioSource>().Play();
            Invoke(nameof(ResetJumpSlash), 1.5f);
        }
    }

    private void ResetJumpSlash()
    {
        isJumpSlashing = false;
    }

    public void JavelinBarrageSkillAnimation()
    {
        if (!isUsingBarrageSkill)
        {
            isUsingBarrageSkill = true;
            enemyAnimator.Play("BarrageSkill");
            GetComponent<AudioSource>().clip = BarrageSkillSound;
            GetComponent<AudioSource>().Play();
            Invoke(nameof(ResetBarrageSkill), 1f);
        }
    }

    private void ResetBarrageSkill()
    {
        isUsingBarrageSkill = false;
    }


}
