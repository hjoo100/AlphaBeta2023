using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;

public class scr_SkillHolder : MonoBehaviour
{
    public int skillNo = 0;
   
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

        if(currentSkill.skillType == SkillEnum.SkillType.Passive)
        {
            currentSkill.PassiveSkillBind(gameObject);
            Debug.Log("Passive skill binded");

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (pauseManager.IsPaused())
        {
            return; // Do not execute the rest of the Update logic if the game is paused
        }
        if (currentSkill == null || isUsableSkill != true )
        {
            return;
        }

        switch (state)
        {
            case SkillState.ready:
               if(Input.GetKeyDown(key))
                {
                    //guard
                    if (currentSkill.SkillID == 6 && !GetComponent<Scr_PlayerCtrl>().CanGuard())
                    {
                        return;
                    }

                    //use skill
                    if(gameObject.GetComponent<Scr_PlayerCtrl>().isImmune)
                    {
                        return;
                        //no skill allowed in immune state
                    }
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
        if(currentSkill == null)
        {
            Debug.Log("No skill in this slot, getting new 0 level skill");
            scr_PlayerSkillManager skillManager = FindObjectOfType<scr_PlayerSkillManager>();
            Skill correctSkill = skillManager.GetSkillByIdAndLevel(skillToGet.SkillID, 0);
            currentSkill = correctSkill;
            if(SkillImage.sprite == null)
            {
                Debug.Log("SkillImage spirte missing!");
            }

            if(currentSkill.skillIcon == null)
            {
                Debug.Log("currentSkill spirte missing!");
            }
            SkillImage.sprite = currentSkill.skillIcon;

            SkillImage.color = Color.white;
        }
        else
        {
            currentSkill = skillToGet;
        }
        //skills.Add(skillToGet);
        

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
        // get current skillID and next level
        int skillId = currentSkill.SkillID;
        int nextLevel = currentSkill.Level + 1;

        // use skillId and level to find next level of the skill
        scr_PlayerSkillManager skillManager = FindObjectOfType<scr_PlayerSkillManager>();
        Skill nextLevelSkill = skillManager.GetSkillByIdAndLevel(skillId, nextLevel);

        // bind skill
        if (nextLevelSkill != null)
        {
            currentSkill.UnbindSkill(FindObjectOfType<Scr_PlayerCtrl>().gameObject); // Add this line to unbind the old skill
            currentSkill = nextLevelSkill;
            currentSkill.SetLevel(nextLevel);

            currentSkill.PassiveSkillBind(FindObjectOfType<Scr_PlayerCtrl>().gameObject); // Add this line to bind the new skill

            Debug.Log("Skill: " + currentSkill.name + " upgraded! Level: " + currentSkill.Level);
            // update skillIcon
            if (nextLevelSkill.GetSkillType() == true)
            {
                SkillImage.sprite = nextLevelSkill.skillIcon;
            }
        }
    }

    public Skill GetCurrentSkill()
    {
        return currentSkill;
    }

    public void ResetSkillImage()
    {
        if (skillNo == 1)
        {
            SkillImage = GameObject.FindGameObjectWithTag("Skill1").GetComponent<Image>();
        }

        if (skillNo == 2)
        {
            SkillImage = GameObject.FindGameObjectWithTag("Skill2").GetComponent<Image>();
        }

        if (skillNo == 3)
        {
            SkillImage = GameObject.FindGameObjectWithTag("Skill3").GetComponent<Image>();
        }
    }

    public void SyncSkillImage()
    {
       
        if(currentSkill.skillIcon != null)
        {
            SkillImage.sprite = currentSkill.skillIcon;
            SkillImage.color = Color.white;
        }
       
        

    }

    public void ReduceCooldown(float amount)
    {
        if (cooldownTime > 0)
        {
            cooldownTime -= amount;
            Debug.Log(currentSkill.SkillID + " skill cooldown reduced by " + amount + " sec.");
            if (cooldownTime < 0)
            {
                cooldownTime = 0;
                float coolDownPercentage = (currentSkill.cooldownTime - cooldownTime) / currentSkill.cooldownTime;
                SkillImage.fillAmount = coolDownPercentage;
            }
        }
    }
}
