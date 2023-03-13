using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_enemyBase : MonoBehaviour
{
    //Enemy base should only include parameters like hitpoints
    public float hitpoints = 50f;
    public GameObject thisEnemy;
    public bool isDead = false;

    public Animator animator;

    public AudioSource enemyAudioSrc;
    public AudioClip fallAudio;
    //1:melee 2:turret
    public int enemyType = 1;

    // Start is called before the first frame update
    void Start()
    {
        thisEnemy = gameObject;
        //animator = GetComponent<Animator>();
        if(enemyType == 1)
        {
            hitpoints = thisEnemy.GetComponent<scr_MeleeEnemy>().enemyhitpoints;
        }
        if(enemyType == 2)
        {
            hitpoints = thisEnemy.GetComponent<scr_turretEnemy>().hitpoints;
        }

        enemyAudioSrc = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void receiveDmg(float dmg)
    {
        hitpoints -= dmg;
        
        if(hitpoints <=0)
        {
            hitpoints = 0;
            GameObject playerArrow = GameObject.FindGameObjectWithTag("PlayerAttackArrow");
            playerArrow.GetComponent<scr_attackArrow>().removeEnemy(gameObject);
            if(enemyType == -1)
            {
                //Dummy
                deadFunc();

            }
            if (enemyType == 1)
            {
                animator.Play("Die");
                enemyAudioSrc.clip = fallAudio;
                enemyAudioSrc.Play();
                thisEnemy.GetComponent<scr_MeleeEnemy>().DeadFunc();
            }
            if(enemyType == 2)
            {
                thisEnemy.GetComponent<scr_turretEnemy>().deadFunc();
            }
            Invoke(nameof(deadFunc), 0.25f);
            // Destroy(thisEnemy);
        }
        else
        {
            if(enemyType == 1)
            {
                thisEnemy.GetComponent<scr_MeleeEnemy>().alertEnemy();
            }
        }
    }

    void deadFunc()
    {
        Destroy(thisEnemy);
    }
}
