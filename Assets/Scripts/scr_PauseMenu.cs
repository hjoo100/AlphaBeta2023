using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class scr_PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private Scr_PauseManager pauseManager;
    //private InputAction pauseAction;
    private Scr_PlayerCtrl playerCtrl;

    void Awake()
    {
        
    }

    void Start()
    {
        pauseManager = FindObjectOfType<Scr_PauseManager>();
        playerCtrl = FindObjectOfType<Scr_PlayerCtrl>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPauseButtonPressed();
        }
    }

    public void OnPauseButtonPressed()
    {
        // Check if the player is currently choosing a skill upgrade
        if (playerCtrl.isChoosingSkill)
        {
            return;
        }

        if (pauseManager.IsPaused())
        {
            pauseMenuUI.SetActive(false);
            pauseManager.ResumeGame();
        }
        else
        {
            pauseMenuUI.SetActive(true);
            pauseManager.PauseGame();
        }
    }

    public void OnRetryButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
    public void OnResumeButtonClicked()
    {
        pauseMenuUI.SetActive(false);
        pauseManager.ResumeGame();
    }
    
}
