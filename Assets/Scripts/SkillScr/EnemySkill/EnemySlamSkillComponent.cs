using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlamSkillComponent : MonoBehaviour
{
    [SerializeField] private Skill enemySlamSkill;
    private EnemySlamSkill slamSkillInstance;
    private float cooldownTimer;
    private bool isSkillActive;

    private void Start()
    {
        if (enemySlamSkill is EnemySlamSkill)
        {
            slamSkillInstance = Instantiate(enemySlamSkill) as EnemySlamSkill;
            slamSkillInstance.Initialize("EnemySlamSkill",SkillEnum.SkillType.Offensive,0);
        }
        else
        {
            Debug.LogError("Assigned skill is not of type EnemySlamSkill.");
        }
    }

    private void Update()
    {
        if (isSkillActive)
        {
            slamSkillInstance.activeTime -= Time.deltaTime;
            if (slamSkillInstance.activeTime <= 0)
            {
                DeactivateSkill();
            }
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                ActivateSkill();
            }
        }
    }

    private void ActivateSkill()
    {
        if (slamSkillInstance != null)
        {
            slamSkillInstance.ActivateSkill(gameObject);
            isSkillActive = true;
            cooldownTimer = slamSkillInstance.cooldownTime;
        }
    }

    private void DeactivateSkill()
    {
        if (slamSkillInstance != null)
        {
            slamSkillInstance.StartSkillCD(gameObject);
            isSkillActive = false;
        }
    }
}
