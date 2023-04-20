using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : ScriptableObject
{
    public new string name;

    public SkillEnum.SkillType skillType;

    public int Level;

    public int SkillID;
    public float cooldownTime, activeTime;

    [SerializeField]
    private bool isUsableSkill = true;
    [SerializeField]
    internal Sprite skillIcon;

    protected Skill(string name, SkillEnum.SkillType skillType, int level)
    {
        this.name = name;
        this.skillType = skillType;
        Level = level;
       
    }

    public virtual void Initialize(string name, SkillEnum.SkillType skillType, int level)
    {
        
    }

    public virtual void ActivateSkill(GameObject obj) { }
    public virtual void StartSkillCD(GameObject obj) { }

    public bool GetSkillType()
    {
        return isUsableSkill;
    }

    public virtual void PassiveSkillBind(GameObject obj) { }

    public virtual void OnEnemyDefeated() { }

    public virtual void UnbindSkill(GameObject obj)
    {
    }
    public void SetLevel(int level)
    {
        Level = level;
    }

}
