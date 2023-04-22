using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using UnityEngine.InputSystem.Controls;
using System;
using TMPro;

public class ComboSystem : MonoBehaviour
{
    public int comboCount;
    public int comboThreshold;
    public float comboResetTime = 2f;

    private float lastSuccessfulAttackTime;

    public bool UIShown = false;
    public event Action OnComboReached;
    public GameObject UIObj, TextObj;

    private void Start()
    {
      
    }
    private void Update()
    {
        if (Time.time - lastSuccessfulAttackTime > comboResetTime)
        {
            comboCount = 0;
            UIShown = false;


            UIObj.SetActive(false);
        }
    }

    public void IncrementCombo()
    {
        lastSuccessfulAttackTime = Time.time;
        comboCount++;

        if (comboCount % comboThreshold == 0) // trigger combo skill when reach the counter
        {
            OnComboReached?.Invoke();
        }

        if (UIShown == false)
        {
       
            UIShown = true;
        }

        

        if (UIObj != null)
        {
            Debug.Log("Found ComboObj, setting it to active");
            UIObj.SetActive(true);
        }
        else
        {
            Debug.Log("ComboObj not found");
        }

        
        if (TextObj == null)
        {
            Debug.Log("didn't find txt obj");
        }
        Debug.Log("TextObj name: " + TextObj.name);
        TextObj.GetComponent<TextMeshProUGUI>().text = comboCount.ToString();

        //debug
        /*
        GameObject[] uiobjs = GameObject.FindGameObjectsWithTag("ComboObj");
        foreach (GameObject obj in uiobjs)
        {
            Debug.Log(obj.name);
        }

        GameObject[] counterObjs = GameObject.FindGameObjectsWithTag("ComboCount");

        foreach(GameObject obj in counterObjs)
        {
            Debug.Log(obj.name);
        }*/
    }
}
