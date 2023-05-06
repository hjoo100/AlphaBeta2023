using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyJavelinBarrageSkill : Skill
{

    private GameObject JavelinLauncher1,JavelinLauncher2;



    public EnemyJavelinBarrageSkill(string name, SkillEnum.SkillType skillType, int level) : base(name, skillType, level)
    {
        base.name = name;
        base.skillType = skillType;
        base.Level = level;

        
    }

    

    public bool IsSkillActive { get; private set; }
    

    public override void Initialize(string name, SkillEnum.SkillType skillType, int level)
    {
        base.Initialize(name, skillType, level);

        

    }

    public void BindLaunchers()
    {
        JavelinLauncher1 = GameObject.FindWithTag("JavelinLauncher1");
        JavelinLauncher2 = GameObject.FindWithTag("JavelinLauncher2");
    }

    public override void ActivateSkill(GameObject obj)
    {
        base.ActivateSkill(obj);
        JavelinLauncher1.GetComponent<Scr_JavelinBarrageObj>().FireJavelin();

        JavelinLauncher2.GetComponent<Scr_JavelinBarrageObj>().FireJavelin();
    }

    public override void StartSkillCD(GameObject obj)
    {
        base.StartSkillCD(obj);
        
    }


    

    


}
