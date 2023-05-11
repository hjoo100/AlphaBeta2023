using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Scr_TimerManager : MonoBehaviour
{

    public static Scr_TimerManager instance;

    [SerializeField]
    private TextMeshProUGUI timerText;

    private float[] bestTimes;
    private float currentTime;
    private bool isTiming;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        bestTimes = new float[4];

        // Read the saved best times
        for (int i = 0; i < bestTimes.Length; i++)
        {
            bestTimes[i] = PlayerPrefs.GetFloat("BestTime" + i, float.MaxValue);
        }
    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            StartTimer();
        }
        
    }
    private void Update()
    {
        if (isTiming && SceneManager.GetActiveScene().buildIndex != 0)
        {
            currentTime += Time.deltaTime;
            timerText.text = FormatTime(currentTime);
        }
    }

    public void StartTimer()
    {
        isTiming = true;
        currentTime = 0f;
    }

    public void StopTimer(int levelIndex)
    {
        isTiming = false;

        // Replace the best time 
        if (currentTime < bestTimes[levelIndex] || bestTimes[levelIndex] == 0)
        {
            bestTimes[levelIndex] = currentTime;

            // Save the new best time
            PlayerPrefs.SetFloat("BestTime" + levelIndex, currentTime);
            PlayerPrefs.Save();
        }
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public float GetBestTime(int levelIndex)
    {
        return bestTimes[levelIndex];
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
        int seconds = Mathf.FloorToInt(timeInSeconds - minutes * 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    public void SetBestTime(int levelIndex)
    {
        if(GameObject.FindGameObjectWithTag("BestTimeNum").GetComponent<TextMeshProUGUI>() == null)
        {
            Debug.Log("Best Time Num Missing");
        }
      

        GameObject.FindGameObjectWithTag("BestTimeNum").GetComponent<TextMeshProUGUI>().SetText(GetBestTimeFormatted(levelIndex));

    }
    public string GetBestTimeFormatted(int levelIndex)
    {
        float time = GetBestTime(levelIndex);

        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        int milliseconds = Mathf.FloorToInt((time - minutes * 60 - seconds) * 1000);

        return string.Format("{0:0}:{1:00}:{2:000}", minutes, seconds, milliseconds);

       
    }
}
