using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    Scr_PauseManager pauseManager;

    
    void Start()
    {
        ExpImage = GameObject.FindGameObjectWithTag("UI.Hud.Exp").GetComponent<Image>();
        // LvUpObj = GameObject.FindGameObjectWithTag("LvUpObj");
        pauseManager = FindObjectOfType<Scr_PauseManager>();
        SkillUpgradeMenu skillUpgradeMenu = FindObjectOfType<SkillUpgradeMenu>();

        scr_PlayerSkillManager skillManager = FindObjectOfType<scr_PlayerSkillManager>();

        if (skillUpgradeMenu != null)
        {
            skillUpgradeMenu.OnSkillSelected += skillManager.HandleSelectedSkill;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (pauseManager.IsPaused())
        {
            return; // Do not execute the rest of the Update logic if the game is paused
        }

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

        if (level == 2 || level == 3 || level == 5)
        {
            StartCoroutine(ChooseSkill(level));
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

    

    public IEnumerator ChooseSkill(int level)
    {
        scr_PlayerSkillManager skillManager = gameObject.GetComponent<scr_PlayerSkillManager>();

        // Count the number of empty skill holders
        var playerSkillHolders = gameObject.GetComponents<scr_SkillHolder>();
        int emptySkillHolders = playerSkillHolders.Count(s => s.GetCurrentSkill() == null);

        // Generate random skills based on the number of empty skill holders
        skillManager.SelectSkills(emptySkillHolders, true);

        // Get the list of current skills to choose from
        List<Skill> skillsToChooseFrom = skillManager.GetCurrentSkills();

        // Show the skill selection UI to the player and wait for their input
        SkillUpgradeMenu skillUpgradeMenu = FindObjectOfType<SkillUpgradeMenu>();
        int? chosenSkillIndex = null;
        skillUpgradeMenu.Initialize(skillsToChooseFrom, skillIndex =>
        {
            chosenSkillIndex = skillIndex;
        });
        skillUpgradeMenu.ShowMenu();

        yield return new WaitUntil(() => chosenSkillIndex.HasValue);

        // Get the chosen skill from the currentSkills list
        Skill chosenSkill = skillManager.GetCurrentSkills()[chosenSkillIndex.Value];

        // Check if the player has the chosen skill
        var existingSkill = playerSkillHolders.FirstOrDefault(s => s.GetCurrentSkill() != null && s.GetCurrentSkill().SkillID == chosenSkill.SkillID);

        if (existingSkill != null)
        {
            // Upgrade the existing skill
            existingSkill.UpgradeSkill();
        }
        else
        {
            if (chosenSkill != null)
            {
                // Find an empty skill holder
                scr_SkillHolder emptySkillHolder = null;
                foreach (scr_SkillHolder skillHolder in playerSkillHolders)
                {
                    if (skillHolder.GetCurrentSkill() == null)
                    {
                        emptySkillHolder = skillHolder;
                        break;
                    }
                }

                if (emptySkillHolder != null)
                {
                    emptySkillHolder.GainSkill(chosenSkill);

                    SkillGainObj.SetActive(true);
                    SkillGainObj.GetComponent<SpriteRenderer>().sprite = SkillGainImg;
                    SkillNameObj.SetActive(true);
                    SkillNameObj.GetComponent<SpriteRenderer>().sprite = SliceWaveUIimg; // Replace this with the chosen skill's UI image
                    skillGainObjActive = true;
                    Invoke(nameof(disableSkillGainImg), 2f);
                }
            }
        }
    }

    private void OnSkillButtonClicked(int skillIndex)
    {
        // add code here for something happen after player choose skill
        Debug.Log("Skill button clicked with index: " + skillIndex);
    }
}
