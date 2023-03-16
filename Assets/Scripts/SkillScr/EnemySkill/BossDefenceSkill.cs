using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BossDefenceSkill : Skill
{
    public float defendRate = 0.3f;
    scr_meleeBoss bossScr;
    public override void ActivateSkill(GameObject Boss)
    {
        bossScr = Boss.GetComponent<scr_meleeBoss>();
        bossScr.isDefending = true;
        bossScr.defendRate = defendRate;
    }

    public override void StartSkillCD(GameObject Boss)
    {
        bossScr = Boss.GetComponent<scr_meleeBoss>();
        bossScr.isDefending = false;
    }
}
