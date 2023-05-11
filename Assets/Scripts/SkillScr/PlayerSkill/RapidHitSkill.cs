using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RapidHitSkill : Skill
{
    // Start is called before the first frame update
    [SerializeField]
    private float damage,knockBackVal,powerattackDmg;
    [SerializeField]
    private bool comboStarted = false;
    [SerializeField]
    private int rapidComboCount;
    [SerializeField]
    private bool isMaxLvl = false;
   

    protected RapidHitSkill(string name, SkillEnum.SkillType skillType, int level) : base(name, skillType, level)
    {
        base.name = name;
        base.skillType = skillType;
        Level = level;
    }

    public override void ActivateSkill(GameObject playerObj)
    {
        if (playerObj.GetComponent<Scr_PlayerCtrl>().isImmune)
        {
            return;
        }

        if (comboStarted == false)
        {
            comboStarted = true;
            playerObj.GetComponent<Scr_PlayerCtrl>().StartRapidHitCombo();
            FindObjectOfType<Scr_PlayerAudioCtrl>().PlayAudio(4);//play skill audio
            playerObj.GetComponent<Scr_PlayerCtrl>().StartCoroutine(playerObj.GetComponent<Scr_PlayerCtrl>().RapidHitComboAttack(rapidComboCount, damage, powerattackDmg, knockBackVal, isMaxLvl));
            
        }
    }

    public override void StartSkillCD(GameObject playerObj)
    {
        if(isMaxLvl)
        {
            playerObj.GetComponent<Scr_PlayerCtrl>().PoweredKnockHit(powerattackDmg, knockBackVal);
        }
        comboStarted = false;
        playerObj.GetComponent<Scr_PlayerCtrl>().EndRapidHitCombo();
    }

   


}
