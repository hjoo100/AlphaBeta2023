using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : ScriptableObject
{
    public new string name;
    public float cooldownTime, activeTime;

    public virtual void ActivateSkill(GameObject obj) { }
    public virtual void StartSkillCD(GameObject obj) { }
}
