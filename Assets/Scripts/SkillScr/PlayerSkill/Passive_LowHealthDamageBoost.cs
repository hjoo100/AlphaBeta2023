using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillEnum;

[CreateAssetMenu]
public class Passive_LowHealthDamageBoost : Skill
{
    [SerializeField]
    private float damageBoost = 0.3f; // Percentage of the base damage added as bonus damage
    [SerializeField]
    private float healthThresholdLevel1 = 0.3f;
    [SerializeField]
    private float healthThresholdLevel2 = 0.5f;

    bool damageBoostApplied =  false;
    protected Passive_LowHealthDamageBoost(string name, SkillType skillType, int level) : base(name, skillType, level)
    {

    }

    public override void PassiveSkillBind(GameObject obj)
    {
        base.PassiveSkillBind(obj);
        Scr_PlayerCtrl playerCtrl = FindObjectOfType<Scr_PlayerCtrl>();

        // Subscribe to the OnHealthChanged event
        playerCtrl.OnHealthChanged += OnHealthChanged;

        // Check if the player's current health is already below the threshold and apply the effect if necessary
        float healthPercentage = playerCtrl.hitpoints / playerCtrl.maxHp;
        if (healthPercentage <= healthThresholdLevel1 && Level < 2)
        {
            OnHealthChanged(playerCtrl.hitpoints, playerCtrl.maxHp);
        }
        if (healthPercentage <= healthThresholdLevel2 && Level == 2)
        {
            OnHealthChanged(playerCtrl.hitpoints, playerCtrl.maxHp);
        }
    }

    private void OnHealthChanged(float currentHealth, float maxHealth)
    {
        Scr_PlayerCtrl playerCtrl = FindObjectOfType<Scr_PlayerCtrl>();
        float healthPercentage = currentHealth / maxHealth;

        float threshold = Level == 1 ? 0.3f : 0.5f;
        if (healthPercentage <= threshold && !damageBoostApplied)
        {
            playerCtrl.meleeDmg = playerCtrl.basicMeleeDmg * damageBoost;
            damageBoostApplied = true;
        }
        else if (healthPercentage > threshold && damageBoostApplied)
        {
            playerCtrl.meleeDmg = playerCtrl.basicMeleeDmg;
            damageBoostApplied = false;
        }
    }

    //old verison of damage boost func
    private void ApplyDamageBoost()
    {
        Scr_PlayerCtrl playerCtrl = FindObjectOfType<Scr_PlayerCtrl>();
        float bonusDamage = playerCtrl.meleeDmg * damageBoost;
        playerCtrl.meleeDmg += bonusDamage;
    }

    private void RemoveDamageBoost()
    {
        Scr_PlayerCtrl playerCtrl = FindObjectOfType<Scr_PlayerCtrl>();
        float bonusDamage = playerCtrl.meleeDmg * damageBoost;
        playerCtrl.meleeDmg -= bonusDamage;
    }
}
