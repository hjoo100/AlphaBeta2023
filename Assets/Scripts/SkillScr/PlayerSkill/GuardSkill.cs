using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GuardSkill : Skill
{
    [SerializeField]
    private Scr_PlayerCtrl _playerCtrl;

    protected GuardSkill(string name, SkillEnum.SkillType skillType, int level) : base(name, skillType, level)
    {
        name = "Guard";
        skillType = SkillEnum.SkillType.NonOffensive;
        level = this.Level;
        
    }

    public override void ActivateSkill(GameObject playerObj)
    {
        _playerCtrl = playerObj.GetComponent<Scr_PlayerCtrl>();

        if (_playerCtrl.CanGuard())
        {
            _playerCtrl.getStateMachine().SetToGuardState();
        }

    }

    public override void StartSkillCD(GameObject playerObj)
    {
       
    }
}
