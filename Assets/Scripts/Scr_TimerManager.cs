using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    }

    private void Start()
    {
        StartTimer();
    }
    private void Update()
    {
        if (isTiming)
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

        // Replace the best time if the current time is better
        if (currentTime < bestTimes[levelIndex] || bestTimes[levelIndex] == 0)
        {
            bestTimes[levelIndex] = currentTime;
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
}
