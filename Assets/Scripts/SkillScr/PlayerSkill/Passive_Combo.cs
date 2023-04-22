using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillEnum;

[CreateAssetMenu]
public class Passive_Combo : Skill
{
    public float bonusDamage;
    public bool pierceArmorAtMaxLevel;

    protected Passive_Combo(string name, SkillType skillType, int level) : base(name, skillType, level)
    {

    }

    public override void PassiveSkillBind(GameObject obj)
    {
        base.PassiveSkillBind(obj);
        ComboSystem comboSystem = FindObjectOfType<Scr_PlayerCtrl>().GetComponent<ComboSystem>();

        comboSystem.OnComboReached += OnComboReached;
    }

    private void OnComboReached()
    {
        Scr_PlayerCtrl playerCtrl = FindObjectOfType<Scr_PlayerCtrl>();

        float damageAmount = playerCtrl.meleeDmg + bonusDamage;
        bool shouldPierceArmor = Level == 2 && pierceArmorAtMaxLevel;

        // Apply bonus damage and stun to the next enemy hit
        playerCtrl.ApplyComboDamage(damageAmount, shouldPierceArmor);
    }
}