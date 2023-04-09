using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DashSkill : Skill
{
    public float dashVelocity;
    public bool isMaxLvl;

    protected DashSkill(string name, SkillEnum.SkillType skillType, int level) : base(name, skillType, level)
    {
        base.name = name;
        base.skillType = skillType;
        Level = level;
    }

    public override void ActivateSkill(GameObject player)
    {
        Scr_PlayerCtrl playerScr = player.GetComponent<Scr_PlayerCtrl>();

        playerScr.PlayerSpd = dashVelocity;

        player.layer = LayerMask.NameToLayer("PlayerNoClip");
       
    }

    public override void StartSkillCD(GameObject player)
    {
        Scr_PlayerCtrl playerScr = player.GetComponent<Scr_PlayerCtrl>();
        playerScr.resetVelocity();
        playerScr.PlayerSpd = playerScr.basicSpd;
        player.layer = LayerMask.NameToLayer("Player");

        if(isMaxLvl)
        {
            playerScr.AddDamageBuff(1.15f, 5f);
        }

    }
}
