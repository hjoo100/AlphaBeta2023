using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BossStompSkill : Skill
{
    public float stompDmg = 40f;
    scr_meleeBoss bossScr;
    public override void ActivateSkill(GameObject Boss)
    {
        Debug.Log("stompCharging");
        bossScr = Boss.GetComponent<scr_meleeBoss>();
        //start charging
        bossScr.isCharging = true;
        //Cancel Defence stance
        bossScr.cancelDefence();
        //when enter coolDown, call stomp attack
    }

    public override void StartSkillCD(GameObject Boss)
    {
        Debug.Log("stomp coming!");
        if (bossScr.isCharging == true)
        {
            
            //increase dmg before attack
            bossScr.meleeDmg = stompDmg;

            bossScr.stompAttack();

            //resume dmg
            bossScr.meleeDmg = bossScr.basicDmg;
            bossScr.isCharging = false;
        }
      
    }
}
