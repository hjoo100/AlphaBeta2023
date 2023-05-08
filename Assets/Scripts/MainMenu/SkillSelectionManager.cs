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
    public Skill[] AllSkills;
    [SerializeField]
    private Skill[] skillsChosen;

    [SerializeField]
    private List<Skill> SkillsChosenList;

    public GameObject SkillsPage;

    private HashSet<Toggle> selectedToggles;

    [SerializeField]
    private int selectedLevel = 1;
    private void Start()
    {
        selectedToggles = new HashSet<Toggle>();

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
            if (selectedToggles.Count < maxSelectedSkills)
            {
                selectedToggles.Add(skillToggle);
            }
            else
            {
                skillToggle.isOn = false;
            }
        }
        else
        {
            selectedToggles.Remove(skillToggle);
        }
    }
    public void ConfirmSkillSelection()
    {
        if (selectedToggles.Count > 0 && selectedToggles.Count <= maxSelectedSkills)
        {
            int skillsAdded = 0;

            foreach (Toggle skillToggle in selectedToggles)
            {
                for (int i = 0; i < skillToggles.Length; i++)
                {
                    if (skillToggle == skillToggles[i])
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
            }

            // Save the skills and hide the panel
            for (int i = 0; i < skillsAdded; i++)
            {
                SkillsChosenList.Add(skillsChosen[i]);
            }

            PlayerPreset.Instance.SetPresetSkills(SkillsChosenList);

            // Debug
            for (int i = 0; i < PlayerPreset.Instance.PresetSkills.Count; i++)
            {
                Debug.Log("Player skills no." + i + " is" + PlayerPreset.Instance.PresetSkills[i].name);
            }

            SkillsPage.SetActive(false);

            Invoke(nameof(LoadNextLevel), 1.2f);
        }

        if (selectedToggles.Count == 0)
        {
            // No skills chosen
            return;
        }


    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(selectedLevel);
    }
    public void SelectLevel(int levelNumber)
    {
        if (levelNumber >= 1 && levelNumber <= 3)
        {
            selectedLevel = levelNumber;
        }
    }

   
}
