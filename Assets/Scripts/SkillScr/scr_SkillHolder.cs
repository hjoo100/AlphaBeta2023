using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_SkillHolder : MonoBehaviour
{
    public int skillNo = 0;
    public Skill skill;
    float cooldownTime, activeTime;
    [SerializeField]
    private Image SkillImage;
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
        if(skillNo == 1)
        {
            SkillImage = GameObject.FindGameObjectWithTag("Skill1").GetComponent<Image>();
        }

        if(skillNo == 2)
        {
            SkillImage = GameObject.FindGameObjectWithTag("Skill2").GetComponent<Image>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(skill == null)
        {
            return;
        }

        switch(state)
        {
            case SkillState.ready:
               if(Input.GetKeyDown(key))
                {
                    //use skill
                    skill.ActivateSkill(gameObject);
                    state = SkillState.active;
                    activeTime = skill.activeTime;
                }
            break;
            case SkillState.active:
                {
                    if(activeTime >0)
                    {
                        activeTime -= Time.deltaTime;
                    }else
                    {
                        skill.StartSkillCD(gameObject);
                        state = SkillState.cooldown;
                        cooldownTime = skill.cooldownTime;
                    }
                }
            break;
            case SkillState.cooldown:
                if (cooldownTime > 0)
                {
                    cooldownTime -= Time.deltaTime;

                    float coolDownPercentage = (skill.cooldownTime - cooldownTime) / skill.cooldownTime;
                    SkillImage.fillAmount = coolDownPercentage;
                }
                else
                {
                    state = SkillState.ready;
                }
                break;


        }
        
    }
}
