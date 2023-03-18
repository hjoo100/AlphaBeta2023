using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_enemyHpBarScr : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    scr_enemyBase enemyStats;
    [SerializeField]
    GameObject hpBar;
    [SerializeField]
    float hpMax, hpCurr;

    void Start()
    {
        hpMax = enemyStats.MaxHitpoints;
    }

    // Update is called once per frame
    void Update()
    {
        hpMax = enemyStats.MaxHitpoints;
        hpCurr = enemyStats.hitpoints;
        hpBar.transform.localScale = new Vector3(hpCurr / hpMax, 1f);
    }
}
