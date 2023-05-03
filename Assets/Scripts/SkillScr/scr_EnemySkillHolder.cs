using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_EnemySkillHolder : MonoBehaviour
{
    public Skill skill;
    public float cooldownTime, activeTime;
    public GameObject enemyObj;
    public enum SkillState
    {
        ready,
        active,
        cooldown
    }

   public SkillState state = SkillState.cooldown;

    private void Start()
    {
        enemyObj = gameObject;
        
    }
    // Update is called once per frame
    void Update()
    {
        if(enemyObj.GetComponent<scr_enemyBase>().theEnemyType== scr_enemyBase.enemyType.UnstoppableBoss) // is boss
        {
            if(enemyObj.GetComponent<scr_meleeBoss>().getAwake() == false)
            {
                return;
            }
        }

        if(enemyObj.GetComponent<scr_enemyBase>().theEnemyType == scr_enemyBase.enemyType.shieldedBoss)
        {
            if(enemyObj.GetComponent<scr_ShieldBossEnemy>().getAwake() == false)
            { 
                return;
            }
        }
        if(state == SkillState.ready)
        {
            skill.ActivateSkill(gameObject);
            state = SkillState.active;
            Debug.Log("state changed to active");
            activeTime = skill.activeTime;
        }

        if(state == SkillState.active)
        {
            if (activeTime > 0)
            {
                activeTime -= Time.deltaTime;
            }
            else
            {
                skill.StartSkillCD(gameObject);
                state = SkillState.cooldown;
                cooldownTime = skill.cooldownTime;
            }
        }

        if(state == SkillState.cooldown)
        {
            if (cooldownTime > 0)
            {
                cooldownTime -= Time.deltaTime;
            }
            else
            {
                state = SkillState.ready;
            }
        }
     

    }
}
