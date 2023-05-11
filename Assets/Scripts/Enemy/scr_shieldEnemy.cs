using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_shieldEnemy : MonoBehaviour
{
    [SerializeField]
    private scr_enemyBase enemyBase;

    [SerializeField]
    private Animator enemyAnimator;
    [SerializeField]
    private Scr_PlayerCtrl player;
    [SerializeField]
    private scr_shieldEnemyMove moveSys;
    [SerializeField]
    private float enemyhitpoints = 50f;
    [SerializeField]
    private float meleeDmg = 20f;
    [SerializeField]
    private float attackCD = 1f;
    [SerializeField]
    private scr_enemyAttackArrow attackArrow;
    [SerializeField]
    private float moveSpd = 5f;

    [SerializeField]
    private bool isAttacked = false, isinAir = false, isKnockedBack = false, isDead = false, isAlerted = false;
    [SerializeField]
    private bool attacking = false;

    [SerializeField]
    private AudioSource enemyAudio;
    [SerializeField]
    private AudioClip punchAudio;



    [SerializeField]
    private float shieldVal = 85f; //maxShieldVal = 85f;
    // Start is called before the first frame update
    private void Awake()
    {
        enemyBase = GetComponent<scr_enemyBase>();
        enemyBase.hitpoints = enemyhitpoints;

        //enemyAnimator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Scr_PlayerCtrl>();
        enemyAudio = GetComponent<AudioSource>();
    }
    void Start()
    {
        moveSys = GetComponent<scr_shieldEnemyMove>();
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

        if (!isAttacked && !isDead && !moveSys.isKnockedBack)
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

                    Invoke(nameof(playAudio), 0.92f);
                    Invoke(nameof(attackPlayer), 0.92f);
                    Invoke(nameof(stopAttackingAnim), 1.34f);
                    Invoke(nameof(ResetAttack), attackCD);
                }
            }

        }
    }

    void stopAttackingAnim()
    {
        if (attacking == true)
        {

            enemyAnimator.Play("Idle");
            attacking = false;
        }

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
        if (isKnockedBack == false)
        {
            attackArrow.attackEnemyInRange(meleeDmg);
        }

    }

    public void playAudio()
    {
        enemyAudio.Play();
    }

    //for getting values
    public float getHitpoints()
    {
        return enemyhitpoints;
    }





    public float getMoveSpd()
    {
        return moveSpd;
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

    public void CancelAttack()
    {
        CancelInvoke(nameof(attackPlayer));
        CancelInvoke(nameof(playAudio));
    }

    public float getShieldVal()
    {
        return shieldVal;
    }
}
