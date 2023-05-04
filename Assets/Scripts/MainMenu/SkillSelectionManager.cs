using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SkillSelectionManager : MonoBehaviour
{
    public Toggle[] skillToggles;
    public int maxSelectedSkills = 3;
    [SerializeField]
    private int currentSelectedSkills;
    public Skill[] AllSkills;
    [SerializeField]
    private Skill[] skillsChosen;

    [SerializeField]
    private List<Skill> SkillsChosenList;

    public GameObject SkillsPage;
    private void Start()
    {
        currentSelectedSkills = 0;

        foreach (Toggle skillToggle in skillToggles)
        {
            skillToggle.onValueChanged.AddListener((bool isSelected) => OnToggleValueChanged(skillToggle, isSelected));
        }

        skillsChosen = new Skill[3];
    }

    private void OnToggleValueChanged(Toggle skillToggle, bool isSelected)
    {
        if (isSelected)
        {
            if (currentSelectedSkills < maxSelectedSkills)
            {
                currentSelectedSkills++;
            }
            else
            {
                
                skillToggle.isOn = false;
            }
        }
        else
        {
            currentSelectedSkills--;
        }
    }

    public void ConfirmSkillSelection()
    {
        if (currentSelectedSkills > 0 && currentSelectedSkills <= maxSelectedSkills)
        {
            int skillsAdded = 0;

            for (int i = 0; i < skillToggles.Length && skillsAdded < currentSelectedSkills; i++)
            {
                Toggle skillToggle = skillToggles[i];
                if (skillToggle.isOn)
                {
                    for (int j = 0; j < skillsChosen.Length; j++)
                    {
                        if (skillsChosen[j] == null)
                        {
                            skillsChosen[j] = AllSkills[i];
                            skillsAdded++;
                            break;
                        }
                    }
                }

                
            }

            //save the skills and hide panel
            for(int i = 0; i < skillsAdded; i++)
            {
                SkillsChosenList.Add(skillsChosen[i]);
            }

            PlayerPreset.Instance.SetPresetSkills(SkillsChosenList);

            //debug
            for(int i=0;i<PlayerPreset.Instance.PresetSkills.Count;i++)
            {

                Debug.Log("Player skills no." + i + " is" + PlayerPreset.Instance.PresetSkills[i].name);
            }

            SkillsPage.SetActive(false);

            Invoke(nameof(LoadNextLevel), 1.2f);
            
        }
        
        if(currentSelectedSkills == 0)
        {
            //no skills chosen
            return;
        }


    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }
}
