using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class scr_GManager : MonoBehaviour
{
    public bool allClear = false;
    private int SceneCount = 1;
    public GameObject GameOverImage, GameEndText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemyNum();
    }

    void CheckEnemyNum()
    {
        GameObject[] enemyGroup;
        enemyGroup = GameObject.FindGameObjectsWithTag("Enemy");
        
        if(enemyGroup.Length > 0)
        {
            allClear = false;
        }
        else
        {
            allClear = true;
            if (SceneManager.GetActiveScene().buildIndex >= SceneCount)
            {
                proceedToNextLevel();
            }
        }
    }

    public void proceedToNextLevel()
    {
        if (allClear)
        {
            if (SceneManager.GetActiveScene().buildIndex < SceneCount)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                WinFunc();
            }
        }

    }

    public void WinFunc()
    {
        GameEndText.SetActive(true);
        GameEndText.GetComponent<TextMeshProUGUI>().SetText("You Win!");
    }

    public void LoseFunc()
    {
        GameOverImage.SetActive(true);
        GameEndText.SetActive(true);
        GameEndText.GetComponent<TextMeshProUGUI>().SetText("Game Over");
    }
}
