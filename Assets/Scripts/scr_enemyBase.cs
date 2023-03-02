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

    //1:melee 2:turret
    public int enemyType = 1;

    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
        if(enemyType == 1)
        {
            hitpoints = thisEnemy.GetComponent<scr_MeleeEnemy>().enemyhitpoints;
        }
        if(enemyType == 2)
        {
            hitpoints = thisEnemy.GetComponent<scr_turretEnemy>().hitpoints;
        }
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
            
            if (enemyType == 1)
            {
                animator.Play("Die");
                
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
