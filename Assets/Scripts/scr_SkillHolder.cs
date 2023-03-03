using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_SkillHolder : MonoBehaviour
{
    public Skill skill;
    float cooldownTime, activeTime;

    enum SkillState
    {
        ready,
        active,
        cooldown
    }

    SkillState state = SkillState.ready;

    public KeyCode key;
    // Update is called once per frame
    void Update()
    {
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
                }
                else
                {
                    state = SkillState.ready;
                }
                break;


        }
        
    }
}
