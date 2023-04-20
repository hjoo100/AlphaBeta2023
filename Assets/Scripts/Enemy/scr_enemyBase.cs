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
    //-1: dummy  1:melee 2:turret 3: boss 
    // public int enemyType = 1;
    public enum enemyType
    {
        dummy,
        melee,
        turret,
        boss
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

        if(theEnemyType == enemyType.boss)
        {
            //Boss
            hitpoints = thisEnemy.GetComponent<scr_meleeBoss>().getHitpoints();
        }
        MaxHitpoints = hitpoints;
        enemyAudioSrc = gameObject.GetComponent<AudioSource>();

        if(enemySprRenderer == null)
        {
            Debug.Log(theEnemyType.ToString() + " has sprite that not binded correctly");
        }
        defaultColor = enemySprRenderer.color;

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void receiveDmg(float dmg)
    {
        if(theEnemyType == enemyType.boss)
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

            if(theEnemyType == enemyType.boss)
            {
                animator.Play("Die");
                enemyAudioSrc.clip = fallAudio;
                enemyAudioSrc.Play();
                thisEnemy.GetComponent<scr_meleeBoss>().DeadFunc();
            }
            // Destroy(thisEnemy);
        }
        else
        {
            if(theEnemyType == enemyType.melee)
            {
                thisEnemy.GetComponent<scr_MeleeEnemy>().alertEnemy();
            }
            if(theEnemyType == enemyType.boss)
            {
                thisEnemy.GetComponent<scr_meleeBoss>().alertEnemy();
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
