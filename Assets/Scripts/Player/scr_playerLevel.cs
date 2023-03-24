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

    [SerializeField]
    Sprite SliceWaveUIimg, DashUIimg;

    [SerializeField]
    Sprite SkillGainImg, SkillImproveImg;

    [SerializeField]
    GameObject SkillGainObj, SkillNameObj;

    bool LvObjActive = false,skillGainObjActive = false;
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

            FixLvUpScaleFunc();
        }

        if(skillGainObjActive)
        {
            FixSkillGainScaleFunc();
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
                if(holdingskill.skillNo == 2)
                {
                    holdingskill.skill = dashSkillLv0;
                    SkillGainObj.SetActive(true);
                    SkillGainObj.GetComponent<SpriteRenderer>().sprite = SkillGainImg;
                    SkillNameObj.SetActive(true);
                    SkillNameObj.GetComponent<SpriteRenderer>().sprite = DashUIimg;
                    skillGainObjActive = true;
                    Invoke(nameof(disableSkillGainImg), 2f);
                }
            }
        }

        if (level == 3)
        {
            var playerSkills = gameObject.GetComponents<scr_SkillHolder>();

            foreach (var holdingskill in playerSkills)
            {
                if (holdingskill.skillNo == 1)
                {
                    holdingskill.skill = slicewaveLv0;
                    SkillGainObj.SetActive(true);
                    SkillGainObj.GetComponent<SpriteRenderer>().sprite = SkillGainImg;
                    SkillNameObj.SetActive(true);
                    SkillNameObj.GetComponent<SpriteRenderer>().sprite = SliceWaveUIimg;
                    skillGainObjActive = true;
                    Invoke(nameof(disableSkillGainImg), 2f);
                }
            }
        }

        if(level == 5)
        {
            var playerSkills = gameObject.GetComponents<scr_SkillHolder>();

            foreach (var holdingskill in playerSkills)
            {
                if (holdingskill.skillNo == 1)
                {
                    holdingskill.skill = slicewaveLv1;
                    SkillGainObj.SetActive(true);
                    SkillGainObj.GetComponent<SpriteRenderer>().sprite = SkillImproveImg;
                    SkillNameObj.SetActive(true);
                    SkillNameObj.GetComponent<SpriteRenderer>().sprite = SliceWaveUIimg;
                    skillGainObjActive = true;
                    Invoke(nameof(disableSkillGainImg), 2f);
                }
            }
        }


    }

    void disableLvUpAnim()
    {
        LvUpObj.SetActive(false);
        LvObjActive=false;
    }

    void FixLvUpScaleFunc()
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

    void FixSkillGainScaleFunc()
    {
        if (gameObject.transform.localScale.x < 0)
        {
            SkillGainObj.transform.localScale = new Vector3(-0.47f, 0.47f);
            SkillGainObj.transform.localPosition = new Vector3(0.825f, 0.837f);
            SkillNameObj.transform.localScale = new Vector3(-0.47f, 0.47f);
            SkillNameObj.transform.localPosition = new Vector3(-0.490f, 0.837f);
        }
        else
        {
            SkillGainObj.transform.localScale = new Vector3(0.47f, 0.47f);
            SkillGainObj.transform.localPosition = new Vector3(-0.423f, 0.837f);
            SkillNameObj.transform.localScale = new Vector3(0.47f, 0.47f);
            SkillNameObj.transform.localPosition = new Vector3(0.877f, 0.837f);
        }
    }

    void disableSkillGainImg()
    {
        SkillGainObj.SetActive(false);
        SkillNameObj.SetActive(false);
        skillGainObjActive=false;
    }
}
