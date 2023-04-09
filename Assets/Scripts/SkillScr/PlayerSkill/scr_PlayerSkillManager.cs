using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillEnum;

public class scr_PlayerSkillManager : MonoBehaviour
{
    public List<Skill> AllSkills; 
    private List<Skill> currentSkills = new List<Skill>();

    public List<Skill> GetCurrentSkills()
    {
        return currentSkills;
    }
    public void SelectSkills(int numSkillsPerType, bool fixedSlots)
    {
        currentSkills.Clear();

        for (int i = 0; i < numSkillsPerType; i++)
        {
            foreach (SkillType type in System.Enum.GetValues(typeof(SkillType)))
            {
                List<SkillType> skillList = new List<SkillType>();
                skillList.Add(type);
                Skill selectedSkill = GetRandomSkill(skillList, fixedSlots);

                if (selectedSkill != null)
                {
                    if (ShouldUpgradeSkill(selectedSkill))
                    {
                        UpgradeSkill(selectedSkill);
                    }
                    else
                    {
                        currentSkills.Add(selectedSkill);
                    }
                }
            }
        }
    }


    public Skill GetRandomSkill(List<SkillType> types, bool fixedSlots)
    {
        // Choose a random skill type from the list
        int randomTypeIndex = Random.Range(0, types.Count);
        SkillType chosenType = types[randomTypeIndex];

        List<Skill> availableSkills = new List<Skill>();
        foreach (Skill skill in AllSkills)
        {
            if ((!fixedSlots || skill.skillType == chosenType) && !currentSkills.Contains(skill))
            {
                availableSkills.Add(skill);
            }
        }

        if (availableSkills.Count > 0)
        {
            int randomIndex = Random.Range(0, availableSkills.Count);
            return availableSkills[randomIndex];
        }

        return null;
    }

    private bool ShouldUpgradeSkill(Skill newSkill)
    {
        if (newSkill == null)
        {
            return false;
        }
        foreach (Skill currentSkill in currentSkills)
        {
            
            if (currentSkill.name == newSkill.name )
            {
                return true;
            }
        }

        return false;
    }

    private void UpgradeSkill(Skill skillToUpgrade)
    {
        scr_SkillHolder skillHolder = null;
        foreach (scr_SkillHolder holder in FindObjectsOfType<scr_SkillHolder>())
        {
            if (holder.skills.Contains(skillToUpgrade))
            {
                skillHolder = holder;
                break;
            }
        }

        if (skillHolder != null)
        {
            skillHolder.UpgradeSkill();
        }
    }

    public void HandleSelectedSkill(int skillIndex)
    {
        // 在这里处理玩家选择的技能，例如将技能绑定到技能槽位等
        scr_SkillHolder skillHolder = null;
        Scr_PlayerCtrl playerScr = FindObjectOfType<Scr_PlayerCtrl>();
        
        skillHolder = playerScr.getSkillHolders()[skillIndex];


        skillHolder.GainSkill(currentSkills[skillIndex]);
    }
}
