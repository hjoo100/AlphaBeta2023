using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BossDefenceSkill : Skill
{
    public float defendRate = 0.3f;
    public float DefenceMoveSpd = 1f;
    
    scr_meleeBoss bossScr;

    
    public override void ActivateSkill(GameObject Boss)
    {
        bossScr = Boss.GetComponent<scr_meleeBoss>();
        if(bossScr.getChargingBool())
        {
            return;
        }
        Debug.Log("defenceskill active function");
        bossScr = Boss.GetComponent<scr_meleeBoss>();
        bossScr.setDefending(true);
        bossScr.setDefendingRate(defendRate);
        bossScr.setMoveSpd(DefenceMoveSpd);
    }

    public override void StartSkillCD(GameObject Boss)
    {
        Debug.Log("defenceskill cd function");
        if(bossScr.getDefendBool() == true)
        {
            bossScr = Boss.GetComponent<scr_meleeBoss>();
            bossScr.cancelDefence();
           
        }
        
    }
}
