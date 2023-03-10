using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class scr_GManager : MonoBehaviour
{
    public bool allClear = false;
    public bool failed = false;
    // public bool spawnerSwitch = false;
    public int spawnerType = 0;
    public float spawnerTimer = 0;
    public float spawnerMaxCD = 10f;
    public int spawnerCounter = 0,maxSpawnCount = 4;
    public int SceneMode = 0;
    public Transform[] spawnLoc;
    public GameObject MeleeEnemy;
    private int SceneCount = 1;
    public GameObject GameOverImage, GameEndText,RetryText;
    public Animator clearAnim;
    // Start is called before the first frame update
    void Start()
    {
        clearAnim = GameObject.FindGameObjectWithTag("UI.Hud.Clear").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if(SceneMode == 1)
        {
            if(spawnerCounter < maxSpawnCount)
            spawnCounter();
        }
        


        CheckEnemyNum();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }

        if(failed)
        {
            if (Input.GetKeyDown(KeyCode.R) == true)
            {
                retry();
            }

        }
    }

    void CheckEnemyNum()
    {
        if(SceneMode == 0) //No spawner mode
        {
            GameObject[] enemyGroup;
            enemyGroup = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemyGroup.Length > 0)
            {
                allClear = false;
            }
            else if (!allClear)
            {
                allClear = true;
                clearAnim.Play("StageClear");
                Invoke(nameof(resetClearImg), 5f);

                if (SceneManager.GetActiveScene().buildIndex >= SceneCount)
                {
                    proceedToNextLevel();
                }
            }
        }
        if (SceneMode == 1)
        {
            //Spawner mode
            GameObject[] enemyGroup;
            enemyGroup = GameObject.FindGameObjectsWithTag("Enemy");

            if(enemyGroup.Length <=0 && spawnerCounter >= 4)
            {
                if(!allClear)
                {
                    allClear = true;
                    clearAnim.Play("StageClear");
                    Invoke(nameof(resetClearImg), 5f);

                    if (SceneManager.GetActiveScene().buildIndex >= SceneCount)
                    {
                        proceedToNextLevel();
                    }
                }
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
        failed = true;
        GameOverImage.SetActive(true);
        GameEndText.SetActive(true);
        GameEndText.GetComponent<TextMeshProUGUI>().SetText("Game Over");
        RetryText.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void resetClearImg()
    {
        clearAnim.Play("ClearImgReturn");
    }

    public void spawnCounter()
    {

        spawnerTimer += Time.deltaTime;
        if(spawnerTimer > spawnerMaxCD)
        {
            spawnFunc();
            spawnerCounter += 1;
            spawnerTimer = 0;
            
        }
    }
    public void spawnFunc()
    {
        switch(spawnerType)
        {
            case 0:
                break;
            case 1:
                //Spawn melee enemies
                foreach(Transform spawnPos in spawnLoc)
                {
                    Instantiate(MeleeEnemy, spawnPos.position, Quaternion.identity);
                    
                }
                break;
            default:
                break;
                
                

        }
        return;
    }
}
