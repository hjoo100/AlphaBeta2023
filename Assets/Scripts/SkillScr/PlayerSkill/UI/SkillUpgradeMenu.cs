using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillUpgradeMenu : MonoBehaviour
{
    public GameObject menuPanel;
    public Button[] skillButtons;

    private Scr_PauseManager pauseManager;

    public Action<int> OnSkillSelected;

    private List<Skill> skillsToChooseFrom;
    private Action<int> onSkillChosen;
    [SerializeField]
    public Sprite[] SkillBorderImages;

    public event Action<bool> OnMenuVisibilityChanged;
    private void Awake()
    {
        pauseManager = FindObjectOfType<Scr_PauseManager>();
    }
    void Start()
    {
        menuPanel.SetActive(false); // hide menu when start

        // add listener for each button
        foreach (Button button in skillButtons)
        {
            button.onClick.AddListener(() => SelectSkill(button));
        }
    }

    public void Initialize(List<Skill> skillsToChooseFrom, Action<int> onSkillChosen)
    {
        if(skillsToChooseFrom == null)
        {
            Debug.Log("skills to choose is null!");
        }

        if(onSkillChosen == null)
        {
            Debug.Log("onskillchosen is null!");
        }
        this.skillsToChooseFrom = skillsToChooseFrom;
        this.onSkillChosen = onSkillChosen;

        // Set the skill icons in the menu

        for (int i = 0; i < skillsToChooseFrom.Count && i < skillButtons.Length; i++)
        {
            skillButtons[i].GetComponent<Image>().sprite = skillsToChooseFrom[i].skillIcon;
            Image BorderImage = skillButtons[i].transform.Find("SkillBorder1").GetComponent<Image>();
            BorderImage.sprite = SkillBorderImages[skillsToChooseFrom[i].Level];
        }
    }

    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        //  Time.timeScale = 0; // pause game
        pauseManager.PauseGame();
        OnMenuVisibilityChanged?.Invoke(true);
    }

    public void HideMenu()
    {
        menuPanel.SetActive(false);
        //  Time.timeScale = 1; // resume game
        pauseManager.ResumeGame();
        OnMenuVisibilityChanged?.Invoke(false);
    }

    private void SelectSkill(Button skillButton)
    {
        int numOfSkillSlot = Array.IndexOf(skillButtons, skillButton);
        Debug.Log("Selected skill index: " + numOfSkillSlot);
        OnSkillSelected?.Invoke(numOfSkillSlot);
        HideMenu();
    }


    private void UpdateSkillIcons()
    {
        for (int i = 0; i < skillsToChooseFrom.Count; i++)
        {
            
           // skillIcons[i].sprite = skillsToChooseFrom[i].skillIcon;
        }
    }

    public void OnSkillButtonClicked(int skillIndex)
    {
        if (onSkillChosen != null)
        {
            onSkillChosen(skillIndex);
        }

        HideMenu(); 
    }

    public void UpdateSkillSelectionUI()
    {
        scr_PlayerSkillManager skillManager = FindObjectOfType<scr_PlayerSkillManager>();
        Scr_PlayerCtrl playerCtrl = FindObjectOfType<Scr_PlayerCtrl>();


        // Loop through skill holders
        for (int i = 0; i < playerCtrl.getSkillHolders().Length; i++)
        {
            scr_SkillHolder skillHolder = playerCtrl.getSkillHolders()[i];
            Skill currentSkill = skillHolder.GetCurrentSkill();
            Button skillButton = skillButtons[i];

            if (currentSkill != null)
            {
                // check if skill can be upgraded
                Skill nextLevelSkill = skillManager.GetNextLevelSkill(currentSkill);
                if (nextLevelSkill != null)
                {
                    // Update the button's UI to display the next level skill
                    Debug.Log("Updating skill Icons for button " + i + " ");
                    skillButton.GetComponent<Image>().sprite = nextLevelSkill.skillIcon;
                    Image BorderImage = skillButton.transform.Find("SkillBorder1").GetComponent<Image>();
                    if (nextLevelSkill.Level == 0)
                    {
                        BorderImage.sprite = SkillBorderImages[0];

                    }

                    if (nextLevelSkill.Level == 1)
                    {
                        BorderImage.sprite = SkillBorderImages[1];

                    }

                    if (nextLevelSkill.Level == 2)
                    {
                        BorderImage.sprite = SkillBorderImages[2];

                    }
                    // Enable the button
                    skillButton.interactable = true;
                }
                else
                {
                    // Skill is at the highest level, disable the button
                    skillButton.interactable = false;
                }
            }
            else
            {
                // No skill in the holder, update the button's UI to display an empty slot or default icon
                // (e.g., skillButton.GetComponent<Image>().sprite = emptySlotIcon;)

                // Enable the button
                skillButton.interactable = true;
            }
        }
    }
}
