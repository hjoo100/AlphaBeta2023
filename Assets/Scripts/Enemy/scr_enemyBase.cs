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
        thisEnemy = gameObject;
        //animator = GetComponent<Animator>();
        //Use enum instead of int
        if(theEnemyType == enemyType.melee)
        {
            hitpoints = thisEnemy.GetComponent<scr_MeleeEnemy>().enemyhitpoints;
        }
        if(theEnemyType == enemyType.turret)
        {
            hitpoints = thisEnemy.GetComponent<scr_turretEnemy>().hitpoints;
        }

        if(theEnemyType == enemyType.boss)
        {
            //Boss
            hitpoints = thisEnemy.GetComponent<scr_meleeBoss>().enemyhitpoints;
        }
        MaxHitpoints = hitpoints;
        enemyAudioSrc = gameObject.GetComponent<AudioSource>();
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
            if(bossScr.isDefending)
            {
                hitpoints -=((1 - bossScr.defendRate) * dmg);
            }else
            {
                hitpoints -= dmg;
            }
        }
        else
        {
            hitpoints -= dmg;
        }
       
        
        if(hitpoints <=0)
        {
            hitpoints = 0;
            GameObject playerArrow = GameObject.FindGameObjectWithTag("PlayerAttackArrow");
            playerArrow.GetComponent<scr_attackArrow>().removeEnemy(gameObject);
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
                thisEnemy.GetComponent<scr_turretEnemy>().deadFunc();
            }
            Invoke(nameof(deadFunc), 0.25f);
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
        Destroy(thisEnemy);
    }
}
