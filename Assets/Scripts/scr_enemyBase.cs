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
    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
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
            animator.Play("Die");
            Invoke(nameof(deadFunc), 0.25f);
           // Destroy(thisEnemy);
        }
    }

    void deadFunc()
    {
        Destroy(thisEnemy);
    }
}
