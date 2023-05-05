using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_enemyBase : MonoBehaviour
{
    //Enemy base should only include parameters like hitpoints
    public float hitpoints = 50f;
    public float MaxHitpoints = 50f;
    public GameObject thisEnemy;
    public bool isDead = false;

    public Animator animator;

    public AudioSource enemyAudioSrc;
    public AudioClip fallAudio;
    [SerializeField]
    private SpriteRenderer enemySprRenderer;
    [SerializeField]
    private Color defaultColor;

    [SerializeField]
    private ParticleSystem hitParticle;

    private Passive_Frenzy FrenzySkill;

    private Scr_PauseManager pauseManager;

    [SerializeField]
    public float shieldVal = 0;

    [SerializeField]
    public float shieldMaxVal = 0;

    [SerializeField]
    private bool shieldEnabled = false;

    public float detectDist = 5;

    private float sortingOrderOffset;
    private float yPosOffset;
    private Vector3 positionOffset;

    public enum enemyType
    {
        dummy,
        melee,
        turret,
        ranged,
        shielded,
        UnstoppableBoss,
        shieldedBoss
    }
    public enemyType theEnemyType = enemyType.melee;

    //Exp for player level up
    public int exp = 20;

    // Start is called before the first frame update
    void Start()
    {
        pauseManager = FindObjectOfType<Scr_PauseManager>();
        thisEnemy = gameObject;
        //animator = GetComponent<Animator>();
        //Use enum instead of int
        if(theEnemyType == enemyType.melee)
        {
            hitpoints = thisEnemy.GetComponent<scr_MeleeEnemy>().getHitpoints();
        }
        if(theEnemyType == enemyType.turret)
        {
            hitpoints = thisEnemy.GetComponent<scr_turretEnemy>().hitpoints;
        }

        if(theEnemyType == enemyType.ranged)
        {
            hitpoints = thisEnemy.GetComponent<scr_movingRangedEnemy>().GetHitpoints();
        }

        if(theEnemyType == enemyType.UnstoppableBoss)
        {
            //Boss
            hitpoints = thisEnemy.GetComponent<scr_meleeBoss>().getHitpoints();
        }

        if(theEnemyType == enemyType.shielded)
        {
            hitpoints = thisEnemy.GetComponent<scr_shieldEnemy>().getHitpoints();
            shieldEnabled = true;
            shieldVal = thisEnemy.GetComponent<scr_shieldEnemy>().getShieldVal();
            shieldMaxVal = shieldVal;
        }

        if (theEnemyType == enemyType.shieldedBoss)
        {
            hitpoints = thisEnemy.GetComponent<scr_ShieldBossEnemy>().getHitpoints();
            shieldEnabled = true;
            shieldVal = thisEnemy.GetComponent<scr_ShieldBossEnemy>().GetShieldVal();
            shieldMaxVal = shieldVal;
        }

        MaxHitpoints = hitpoints;
        enemyAudioSrc = gameObject.GetComponent<AudioSource>();

        if(enemySprRenderer == null)
        {
            Debug.Log(theEnemyType.ToString() + " has sprite that not binded correctly");
        }
        defaultColor = enemySprRenderer.color;


        positionOffset = new Vector3(0, Random.Range(-0.1f, 0.1f), 0);
        transform.position += positionOffset;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSortingOrder();
    }

    void UpdateSortingOrder()
    {
        float yPos = transform.position.y;
        enemySprRenderer.sortingOrder = (int)(yPos * -300);
    }
    public void receiveDmg(float dmg)
    {
        if(theEnemyType == enemyType.UnstoppableBoss)
        {
            var bossScr = gameObject.GetComponent<scr_meleeBoss>();
            if(bossScr.getDefendBool())
            {
                hitpoints -=((1 - bossScr.getDefendRate()) * dmg);
                HitChangeColor();
                
                
            }else
            {
                hitpoints -= dmg;
                HitChangeColor();
                if(hitParticle != null)
                {
                    showHitParticle();
                }
                
                Invoke(nameof(disableHitParticle), 0.45f);
            }
        }

       if(shieldEnabled)
        {
            if(shieldVal > 0)
            {
                shieldVal -= dmg;
                if(theEnemyType == enemyType.shieldedBoss)
                {
                    thisEnemy.GetComponent<scr_ShieldBossEnemy>().alertEnemy();
                }
                if(shieldVal <0)
                {
                    shieldVal = 0;
                    shieldEnabled = false;
                    return;
                }
                return;
            }


        }

        else
        {
           
            hitpoints -= dmg;
            HitChangeColor();
            if(hitParticle != null)
            {
                showHitParticle();
            }
            

            Invoke(nameof(disableHitParticle), 0.45f);
        }
       if(theEnemyType == enemyType.melee)
        {
            gameObject.GetComponent<scr_MeleeEnemy>().CancelAttack();
        }

       if(theEnemyType == enemyType.shielded)
        {
            gameObject.GetComponent<scr_shieldEnemy>().CancelAttack();
        }
       
        
        if(hitpoints <=0)
        {
            hitpoints = 0;
            GameObject playerArrow = GameObject.FindGameObjectWithTag("PlayerAttackArrow");
            isDead = true;
            if(theEnemyType == enemyType.dummy)
            {
                //Dummy
                deadFunc();

            }
            if (theEnemyType == enemyType.melee)
            {
                animator.Play("Die");
                enemyAudioSrc.clip = fallAudio;
                enemyAudioSrc.Play();
                thisEnemy.GetComponent<scr_MeleeEnemy>().DeadFunc();
            }
            if(theEnemyType == enemyType.turret)
            {
                enemyAudioSrc.clip = fallAudio;
                enemyAudioSrc.Play();
                thisEnemy.GetComponent<scr_turretEnemy>().deadFunc();
                
            }
            Invoke(nameof(deadFunc), 0.25f);

            if(theEnemyType == enemyType.UnstoppableBoss)
            {
                animator.Play("Die");
                enemyAudioSrc.clip = fallAudio;
                enemyAudioSrc.Play();
                thisEnemy.GetComponent<scr_meleeBoss>().DeadFunc();
            }

            if (theEnemyType == enemyType.ranged)
            {
                animator.Play("Die");
                enemyAudioSrc.clip = fallAudio;
                enemyAudioSrc.Play();
                thisEnemy.GetComponent<scr_movingRangedEnemy>().DeadFunc();
            }
            // Destroy(thisEnemy);
        }
        else
        {
            if(theEnemyType == enemyType.melee)
            {
                thisEnemy.GetComponent<scr_MeleeEnemy>().alertEnemy();
            }
            if(theEnemyType == enemyType.UnstoppableBoss)
            {
                thisEnemy.GetComponent<scr_meleeBoss>().alertEnemy();
            }
            
        }
    }

    public void receiveDmg(float dmg,bool shieldPen)
    {
        if (shieldEnabled && !shieldPen)
        {
            //not penning shield
            shieldVal -= dmg;
            if(shieldVal <0)
            {
                shieldVal = 0;
                shieldEnabled = false;
            }
        }
        else
        {
            if(shieldEnabled && shieldVal > 0)
            {
                shieldVal -= dmg;
                if (shieldVal < 0)
                {
                    shieldVal = 0;
                    shieldEnabled = false;
                }
            }

            
                hitpoints -= dmg;
                HitChangeColor();
                if (hitParticle != null)
                {
                    showHitParticle();
                }


                Invoke(nameof(disableHitParticle), 0.45f);
            }
            if (theEnemyType == enemyType.melee)
            {
                gameObject.GetComponent<scr_MeleeEnemy>().CancelAttack();
            }
        if (theEnemyType == enemyType.shielded)
        {
            gameObject.GetComponent<scr_shieldEnemy>().CancelAttack();
        }


            if (hitpoints <= 0)
            {
                hitpoints = 0;
                GameObject playerArrow = GameObject.FindGameObjectWithTag("PlayerAttackArrow");
                isDead = true;
                if (theEnemyType == enemyType.dummy)
                {
                    //Dummy
                    deadFunc();

                }
                if (theEnemyType == enemyType.melee)
                {
                    animator.Play("Die");
                    enemyAudioSrc.clip = fallAudio;
                    enemyAudioSrc.Play();
                    thisEnemy.GetComponent<scr_MeleeEnemy>().DeadFunc();
                }
                if (theEnemyType == enemyType.turret)
                {
                    enemyAudioSrc.clip = fallAudio;
                    enemyAudioSrc.Play();
                    thisEnemy.GetComponent<scr_turretEnemy>().deadFunc();

                }
                Invoke(nameof(deadFunc), 0.25f);

                if (theEnemyType == enemyType.UnstoppableBoss)
                {
                    animator.Play("Die");
                    enemyAudioSrc.clip = fallAudio;
                    enemyAudioSrc.Play();
                    thisEnemy.GetComponent<scr_meleeBoss>().DeadFunc();
                }

                if (theEnemyType == enemyType.ranged)
                {
                    animator.Play("Die");
                    enemyAudioSrc.clip = fallAudio;
                    enemyAudioSrc.Play();
                    thisEnemy.GetComponent<scr_movingRangedEnemy>().DeadFunc();
                }
                // Destroy(thisEnemy);
            }
            else
            {
                if (theEnemyType == enemyType.melee)
                {
                    thisEnemy.GetComponent<scr_MeleeEnemy>().alertEnemy();
                }
                if (theEnemyType == enemyType.UnstoppableBoss)
                {
                    thisEnemy.GetComponent<scr_meleeBoss>().alertEnemy();
                }

                if(theEnemyType == enemyType.shielded)
            {
                thisEnemy.GetComponent<scr_shieldEnemy>().alertEnemy();
            }

            

            

             }
    }
    void deadFunc()
    {
        var playerLv = GameObject.FindGameObjectWithTag("Player").GetComponent<scr_playerLevel>();
        playerLv.gainExp(exp);
        isDead = true;
        FindObjectOfType<Scr_PlayerCtrl>().NotifyEnemyDefeated();
        Destroy(thisEnemy);
    }

    void HitChangeColor()
    {
        enemySprRenderer.color = Color.green;
        Invoke(nameof(resumeColor), 0.2f);
    }

    void resumeColor()
    {
        enemySprRenderer.color = defaultColor;
    }

    void showHitParticle()
    {
        if (hitParticle == null)
        {
            return;
        }
        if (hitParticle.gameObject.activeSelf == false)
        {
            hitParticle.gameObject.SetActive(true);
            hitParticle.Play();
        }
        

    }

    void disableHitParticle()
    {
        if(hitParticle == null)
        {
            return;
        }
        if (hitParticle.gameObject.activeSelf)
        {
            hitParticle.Stop();
            hitParticle.gameObject.SetActive(false);
        }
        
        
    }
}
