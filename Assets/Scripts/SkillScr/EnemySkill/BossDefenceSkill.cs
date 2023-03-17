using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BossDefenceSkill : Skill
{
    public float defendRate = 0.3f;
    public float DefenceMoveSpd = 0.15f;
    scr_meleeBoss bossScr;
    public override void ActivateSkill(GameObject Boss)
    {
        if(bossScr.isCharging)
        {
            return;
        }
        Debug.Log("defenceskill active function");
        bossScr = Boss.GetComponent<scr_meleeBoss>();
        bossScr.isDefending = true;
        bossScr.defendRate = defendRate;
        bossScr.moveSpd = DefenceMoveSpd;
    }

    public override void StartSkillCD(GameObject Boss)
    {
        Debug.Log("defenceskill cd function");
        if(bossScr.isDefending == true)
        {
            bossScr = Boss.GetComponent<scr_meleeBoss>();
            bossScr.cancelDefence();
           
        }
        
    }
}
