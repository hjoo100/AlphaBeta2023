using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_WinMenu : MonoBehaviour
{
    public void OnReturnToMenuClicked()
    {
        SceneManager.LoadScene(0);
    }

    public void OnExitGameClicked()
    {
        Application.Quit();
    }
}
