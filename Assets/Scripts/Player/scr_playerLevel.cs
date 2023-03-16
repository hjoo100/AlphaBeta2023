using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_playerLevel : MonoBehaviour
{
    // Start is called before the first frame update
    public int playerLevel = 1;
    public int currExp = 0;
    public int ExpForLevelUp = 200;
    
    public SliceWaveSkill slicewaveLv0,slicewaveLv1,slicewaveLv2;
    public DashSkill dashSkillLv0,dashSkillLv1,dashSkillLv2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gainExp(int exp)
    {
        currExp += exp;
        if(currExp >= ExpForLevelUp)
        {
            playerLevel += 1;
            levelUpFunc(playerLevel);
        }
    }

    public void levelUpFunc(int level)
    {
        // level up: increase player melee damage, refill hit points of player
        // Also improve skills if level is high enough
        var playerScr = gameObject.GetComponent<Scr_PlayerCtrl>();
        playerScr.levelUP(level);

        if(level == 2)
        {
            var playerSkills = gameObject.GetComponents<scr_SkillHolder>();

            foreach(var holdingskill in playerSkills)
            {
                if(holdingskill.skill == slicewaveLv0)
                {
                    holdingskill.skill = slicewaveLv1;
                }
            }
        }
    }
}
