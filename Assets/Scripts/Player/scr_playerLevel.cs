using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_playerLevel : MonoBehaviour
{
    // Start is called before the first frame update
    public int playerLevel = 1;
    public float currExp = 0;
    public float ExpForLevelUp = 200;
    
    public SliceWaveSkill slicewaveLv0,slicewaveLv1,slicewaveLv2;
    public DashSkill dashSkillLv0,dashSkillLv1,dashSkillLv2;

    public Image ExpImage;
    public GameObject LvUpObj;
    bool LvObjActive = false;
    void Start()
    {
        ExpImage = GameObject.FindGameObjectWithTag("UI.Hud.Exp").GetComponent<Image>();
       // LvUpObj = GameObject.FindGameObjectWithTag("LvUpObj");
    }

    // Update is called once per frame
    void Update()
    {
        ExpImage.fillAmount = currExp / ExpForLevelUp;
        
        if(LvObjActive)
        {

            FixScaleFunc();
        }
    }

    public void gainExp(int exp)
    {
        currExp += exp;
        if(currExp >= ExpForLevelUp)
        {
            playerLevel += 1;
            levelUpFunc(playerLevel);
            currExp = 0;
        }
        ExpImage.fillAmount = currExp / ExpForLevelUp;
    }

    public void levelUpFunc(int level)
    {
        // level up: increase player melee damage, refill hit points of player
        // Also improve skills if level is high enough
        var playerScr = gameObject.GetComponent<Scr_PlayerCtrl>();
        playerScr.levelUP(level);
        LvUpObj.SetActive(true);
        LvObjActive = true;
        Invoke(nameof(disableLvUpAnim), 2f);

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

    void disableLvUpAnim()
    {
        LvUpObj.SetActive(false);
        LvObjActive=false;
    }

    void FixScaleFunc()
    {
        if(gameObject.transform.localScale.x < 0)
        {
            LvUpObj.transform.localScale = new Vector3(-1, 1);
        }
        else
        {
            LvUpObj.transform.localScale = new Vector3(1, 1);
        }
    }
}
