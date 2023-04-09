using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class scr_SkillHolder : MonoBehaviour
{
    public int skillNo = 0;
    [SerializeField]
    public List<Skill> skills;
    [SerializeField]
    private Skill currentSkill;
    float cooldownTime, activeTime;
    [SerializeField]
    private Image SkillImage;
    [SerializeField]
    private bool isUsableSkill = true;

    private Scr_PauseManager pauseManager;
    enum SkillState
    {
        ready,
        active,
        cooldown
    }

    SkillState state = SkillState.ready;

    public KeyCode key;

    private void Start()
    {
        pauseManager = FindObjectOfType<Scr_PauseManager>();
        if(skillNo == 1)
        {
            SkillImage = GameObject.FindGameObjectWithTag("Skill1").GetComponent<Image>();
        }

        if(skillNo == 2)
        {
            SkillImage = GameObject.FindGameObjectWithTag("Skill2").GetComponent<Image>();
        }

        if (skillNo == 3)
        {
            SkillImage = GameObject.FindGameObjectWithTag("Skill3").GetComponent<Image>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (pauseManager.IsPaused())
        {
            return; // Do not execute the rest of the Update logic if the game is paused
        }
        if (skills.Count == 0 || isUsableSkill != true)
        {
            return;
        }

        switch(state)
        {
            case SkillState.ready:
               if(Input.GetKeyDown(key))
                {
                    //use skill
                    currentSkill.ActivateSkill(gameObject);
                    state = SkillState.active;
                    activeTime = currentSkill.activeTime;
                }
            break;
            case SkillState.active:
                {
                    if(activeTime >0)
                    {
                        activeTime -= Time.deltaTime;
                    }else
                    {
                        currentSkill.StartSkillCD(gameObject);
                        state = SkillState.cooldown;
                        cooldownTime = currentSkill.cooldownTime;
                    }
                }
            break;
            case SkillState.cooldown:
                if (cooldownTime > 0)
                {
                    cooldownTime -= Time.deltaTime;

                    float coolDownPercentage = (currentSkill.cooldownTime - cooldownTime) / currentSkill.cooldownTime;
                    SkillImage.fillAmount = coolDownPercentage;
                }
                else
                {
                    state = SkillState.ready;
                }
                break;


        }
        
    }

    public void GainSkill(Skill skillToGet)
    {
        skills.Add(skillToGet);
        currentSkill = skillToGet;

        if (skillToGet.GetSkillType() == true)
        {
            //active skill

        }
        else
        {
            //passive skill
            skillToGet.PassiveSkillBind(gameObject);

        }

       
    }


    public void UpgradeSkill()
    {
        if (currentSkill.Level < skills.Count - 1)
        {
            currentSkill.Level++;
        }
    }

    public Skill GetCurrentSkill()
    {
        return currentSkill;
    }
}
