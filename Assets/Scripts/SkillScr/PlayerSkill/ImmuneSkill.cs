using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ImmuneSkill : Skill
{
    [SerializeField]
    private Scr_PlayerCtrl _playerCtrl;

    protected ImmuneSkill(string name, SkillEnum.SkillType skillType, int level) : base(name, skillType, level)
    {
        name = "Immune";
        skillType = SkillEnum.SkillType.NonOffensive;
        level = this.Level;
    }

    public override void ActivateSkill(GameObject playerObj)
    {
        _playerCtrl = playerObj.GetComponent<Scr_PlayerCtrl>();
        _playerCtrl.TriggerImmune();
    }

    public override void StartSkillCD(GameObject playerObj)
    {
        _playerCtrl.StopImmune();
    }
}
