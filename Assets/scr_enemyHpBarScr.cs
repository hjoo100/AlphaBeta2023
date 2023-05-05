using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_enemyHpBarScr : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    scr_enemyBase enemyStats;
    [SerializeField]
    GameObject hpBar, hpObj;
    [SerializeField]
    float hpMax, hpCurr;

    [SerializeField]
    float yOffset;

    void Start()
    {
        hpMax = enemyStats.MaxHitpoints;
        yOffset = Random.Range(-0.2f, 0.2f);
        hpBar.transform.localPosition += new Vector3(0, yOffset, 0);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        hpMax = enemyStats.MaxHitpoints;
        hpCurr = enemyStats.hitpoints;
        hpObj.transform.localScale = new Vector3(hpCurr / hpMax, 1f);
    }
}
