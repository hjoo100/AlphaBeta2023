using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static SkillEnum;

[CreateAssetMenu]
public class Passive_Frenzy : Skill
{
    public float healthRecoveryAmount; 
    public float cooldownRecoveryAmount;

    public Passive_Frenzy(string name, SkillType skillType, int level) : base(name, skillType, level)
    {
       
    }

    public override void PassiveSkillBind(GameObject obj)
    {
        base.PassiveSkillBind(obj);


        Scr_PlayerCtrl playerCtrl = FindObjectOfType<Scr_PlayerCtrl>();


        playerCtrl.EnemyDefeated += OnEnemyDefeated;

        Debug.Log("Frenzy acquired, level: " + Level);
    }

    public override void OnEnemyDefeated()
    {
        if(Level != 2)
        {
            Debug.Log("Frenzy not maxed out! current level: " + Level);
        }
        if (Level == 2)
        {

            // Reduce the cooldown of other active skills
            scr_SkillHolder[] skillHolders = FindObjectOfType<Scr_PlayerCtrl>().getSkillHolders();
            foreach (scr_SkillHolder skillHolder in skillHolders)
            {
                //Debug.Log("skill check: skill same? : " + (skillHolder.GetCurrentSkill() != this) + "skill type offensive? : " + (skillHolder.GetCurrentSkill().skillType == SkillEnum.SkillType.Offensive));
                if (skillHolder.GetCurrentSkill() != this && skillHolder.GetCurrentSkill().skillType == SkillEnum.SkillType.Offensive)
                {
                    Debug.Log("reducing cool down!");
                    skillHolder.ReduceCooldown(cooldownRecoveryAmount);
                }
            }
        }


        // FindObjectOfType<Scr_PlayerCtrl>().hitpoints += healthRecoveryAmount;
        FindObjectOfType<Scr_PlayerCtrl>().RestoreHp(healthRecoveryAmount);
    }

    public override void Initialize(string name, SkillType skillType, int level)
    {
        base.name = name;
        base.skillType = skillType;
        Level = level;
    }

    public override void UnbindSkill(GameObject obj)
    {
        Scr_PlayerCtrl playerCtrl = FindObjectOfType<Scr_PlayerCtrl>();
        playerCtrl.EnemyDefeated -= OnEnemyDefeated;
    }
}
