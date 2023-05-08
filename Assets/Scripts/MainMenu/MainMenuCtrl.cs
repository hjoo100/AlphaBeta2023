using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject mainpageObj,levelSelectionObj, ExitGameButtonObj,levelShowcaseObj,SkillSelectionObj,RougeLikeModeButtonObj;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickOnLevelSelect()
    {
        mainpageObj.SetActive(false);

        levelSelectionObj.SetActive(true);
    }

    public void ClickOnSkillSelection()
    {
        SkillSelectionObj.SetActive(true);
    }

    public void HideSkillSelection()
    {
        SkillSelectionObj.SetActive(false);
    }

    public void HideLevelSecection()
    {
        levelSelectionObj.SetActive(false);
    }

    public void RougeStart()
    {
        SkillSelectionManager skillManager = GameObject.FindObjectOfType<SkillSelectionManager>();
        SceneManager.LoadScene(skillManager.selectedLevel);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BackToMainPage()
    {
        SkillSelectionObj.SetActive(false);
        levelSelectionObj.SetActive(false);

        mainpageObj.SetActive(true);
    }
}
