using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_enemy : MonoBehaviour
{
    public float hitpoints = 50f;
    public GameObject thisEnemy;
    // Start is called before the first frame update
    void Start()
    {
        
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
            Destroy(thisEnemy);
        }
    }
}
