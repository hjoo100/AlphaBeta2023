using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DashSkill : Skill
{
    public float dashVelocity;

    public override void ActivateSkill(GameObject player)
    {
        Scr_PlayerCtrl playerScr = player.GetComponent<Scr_PlayerCtrl>();
        

        playerScr.PlayerSpd = dashVelocity;
    }

    public override void StartSkillCD(GameObject player)
    {
        Scr_PlayerCtrl playerScr = player.GetComponent<Scr_PlayerCtrl>();
        playerScr.PlayerSpd = playerScr.basicSpd;
    }
}
