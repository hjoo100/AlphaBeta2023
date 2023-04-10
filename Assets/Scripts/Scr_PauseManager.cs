using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PauseManager : MonoBehaviour
{
    private bool isPaused = false;

    public void PauseGame()
    {
        isPaused = true;
        Scr_PlayerCtrl playerCtrl = FindObjectOfType<Scr_PlayerCtrl>();
        playerCtrl.resetVelocity();
    }

    public void ResumeGame()
    {
        isPaused = false;
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}
