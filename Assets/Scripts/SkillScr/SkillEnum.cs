using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkillEnum", menuName = "MyGame/SkillEnum")]
public class SkillEnum : ScriptableObject
{
    public enum SkillType
    {
        Offensive,
        NonOffensive,
        Passive,
        Any = Offensive | NonOffensive | Passive
    }

    public SkillType playerSkillType;
}
