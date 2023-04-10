using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillEnum;

public class scr_PlayerSkillManager : MonoBehaviour
{
    public List<Skill> AllSkills;
    [SerializeField]
    private List<Skill> currentSkills = new List<Skill>();

    public List<Skill> GetCurrentSkills()
    {
        return currentSkills;
    }
    public void SelectSkills(int numSkillsPerType, bool fixedSlots)
    {
        currentSkills.Clear();

        Scr_PlayerCtrl playerScr = FindObjectOfType<Scr_PlayerCtrl>();
        int numSkillHolders = playerScr.getSkillHolders().Length;
        scr_SkillHolder[] skillHolders = playerScr.getSkillHolders();

        if (fixedSlots)
        {
            for (int i = 0; i < numSkillHolders; i++)
            {
                Skill currentSkill = skillHolders[i].GetCurrentSkill();
                if (currentSkill != null)
                {
                    Skill nextLevelSkill = GetNextLevelSkill(currentSkill);
                    if (nextLevelSkill != null)
                    {
                        currentSkills.Add(nextLevelSkill);
                    }
                    else
                    {
                        currentSkills.Add(currentSkill);
                    }
                }
                else
                {
                    // Create a list of all skill types
                    List<SkillType> skillList = new List<SkillType>(System.Enum.GetValues(typeof(SkillType)) as SkillType[]);

                    // Select a random skill from the list of skill types
                    Skill selectedSkill = GetRandomOrUpgradableSkill(skillList, fixedSlots);
                    if (selectedSkill != null)
                    {
                        currentSkills.Add(selectedSkill);
                    }
                }
            }
        }
        else
        {
            // use a HashSet to store the skill ids that have already been selected
            HashSet<int> selectedSkillIds = new HashSet<int>();

            for (int i = 0; i < numSkillHolders; i++)
            {
                // Create a list of all skill types
                List<SkillType> skillList = new List<SkillType>(System.Enum.GetValues(typeof(SkillType)) as SkillType[]);

                Skill selectedSkill = null;
                while (selectedSkill == null)
                {
                    selectedSkill = GetRandomOrUpgradableSkill(skillList, fixedSlots);
                    if (selectedSkill != null && selectedSkillIds.Contains(selectedSkill.SkillID))
                    {
                        selectedSkill = null;
                    }
                }

                selectedSkillIds.Add(selectedSkill.SkillID);
                currentSkills.Add(selectedSkill);
            }
        }
    }


    public Skill GetRandomOrUpgradableSkill(List<SkillType> types, bool fixedSlots)
    {
        Scr_PlayerCtrl playerScr = FindObjectOfType<Scr_PlayerCtrl>();
        scr_SkillHolder[] skillHolders = playerScr.getSkillHolders();

        // Choose a random skill type from the list
        int randomTypeIndex = Random.Range(0, types.Count);
        SkillType chosenType = types[randomTypeIndex];

        List<Skill> availableSkills = new List<Skill>();
        foreach (Skill skill in AllSkills)
        {
            if (skill.skillType == chosenType && skill.Level == 0)
            {
                bool skillAlreadyInSlot = false;

                foreach (scr_SkillHolder holder in skillHolders)
                {
                    if (holder.GetCurrentSkill() != null && holder.GetCurrentSkill().SkillID == skill.SkillID)
                    {
                        skillAlreadyInSlot = true;

                        // Check if the current skill can be upgraded
                        if (fixedSlots)
                        {
                            Skill nextLevelSkill = GetNextLevelSkill(holder.GetCurrentSkill());
                            if (nextLevelSkill != null)
                            {
                                availableSkills.Add(nextLevelSkill);
                            }
                        }

                        break;
                    }
                }

                if (!skillAlreadyInSlot && !currentSkills.Contains(skill))
                {
                    availableSkills.Add(skill);
                }
            }
        }

        if (availableSkills.Count > 0)
        {
            int randomIndex = Random.Range(0, availableSkills.Count);
            Skill chosenSkill = availableSkills[randomIndex];
            return chosenSkill;
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
            if (holder.GetCurrentSkill() == skillToUpgrade)
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
        if (skillIndex < 0 || skillIndex >= currentSkills.Count)
        {
            Debug.LogError("Invalid skill index");
            return;
        }
        // handle player selected skills here
        scr_SkillHolder skillHolder = null;
        Scr_PlayerCtrl playerScr = FindObjectOfType<Scr_PlayerCtrl>();

        skillHolder = playerScr.getSkillHolders()[skillIndex];

        // get selected skil
        Skill selectedSkill = currentSkills[skillIndex];

        // check if player already have this skill
        Skill existingSkill = skillHolder.GetCurrentSkill();
        if (existingSkill != null && existingSkill.SkillID == selectedSkill.SkillID)
        {
            // if player has this skill, upgrade it
            skillHolder.UpgradeSkill();

           
            Skill upgradedSkill = GetNextLevelSkill(existingSkill);
            if (upgradedSkill != null)
            {
                currentSkills[skillIndex] = upgradedSkill;
            }
        }
        else
        {
            // if not, pass the skill to GainSkill func
            skillHolder.GainSkill(selectedSkill);

            
        }
        SkillUpgradeMenu skillUpgradeMenu = FindObjectOfType<SkillUpgradeMenu>();
         skillUpgradeMenu.UpdateSkillSelectionUI();
    }

    public Skill GetSkillByIdAndLevel(int skillId, int level)
    {
        foreach (Skill skill in AllSkills)
        {
            if (skill.SkillID == skillId && skill.Level == level)
            {
                return skill;
            }
        }
        return null;
    }
    public List<Skill> GetInitialSkills()
    {
        List<Skill> initialSkills = new List<Skill>();
        foreach (Skill skill in currentSkills)
        {
            Skill initialSkill = GetSkillByIdAndLevel(skill.SkillID, 0);
            initialSkills.Add(initialSkill);
        }
        return initialSkills;
    }
    public Skill GetNextLevelSkill(Skill currentSkill)
    {
        int skillId = currentSkill.SkillID;
        int nextLevel = currentSkill.Level + 1;

        // Prevent upgrading the skill beyond level 2
        if (nextLevel > 2)
        {
            return null;
        }

        return GetSkillByIdAndLevel(skillId, nextLevel);
    }
}
