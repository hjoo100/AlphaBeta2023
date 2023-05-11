using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public bool winned = false;

    public GameObject[] BossObj;

    public Scr_TimerManager timerManager;

    public GameObject WinMenu;
    // Start is called before the first frame update
    void Start()
    {
        clearAnim = GameObject.FindGameObjectWithTag("UI.Hud.Clear").GetComponent<Animator>();
        timerManager = FindObjectOfType<Scr_TimerManager>();
        //WinMenu = GameObject.FindGameObjectWithTag("WinMenu");
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
        if(SceneMode == 3)
        {
            //kill boss to win
            checkBossFunc();
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
        timerManager.StopTimer(SceneManager.GetActiveScene().buildIndex);
        WinMenu.SetActive(true);
        GameObject.FindGameObjectWithTag("ClearTimeText").GetComponent<TextMeshProUGUI>().SetText(timerManager.FormatTime(timerManager.GetCurrentTime()));
        Cursor.visible = true;
        
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

    void checkBossFunc()
    {
        if (BossObj.All(boss => boss == null) && winned == false)
        {
            allClear = true;
            winned = true;
            clearAnim.Play("StageClear");
            Invoke(nameof(resetClearImg), 5f);
            WinFunc();
        }
    }

   public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    
}
