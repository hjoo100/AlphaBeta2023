using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public string customName;

    public State mainStateType;

    private Scr_PauseManager pauseManager;

    public State CurrentState { get; set; } //issue laying here
    

    public State nextState;

    [SerializeField]
    public Scr_PlayerCtrl playerscr;

    

    // Update is called once per frame
    private void Start()
    {
        pauseManager = FindObjectOfType<Scr_PauseManager>();
        mainStateType = new Scr_IdleComboState();
        CurrentState = new Scr_IdleComboState();



    }
    void Update()
    {
        if (pauseManager.IsPaused())
        {
            return; // Do not execute the rest of the Update logic if the game is paused
        }

        if (nextState != null)
        {
            SetState(nextState);
        }
        else
        {
           // nextState = new Scr_IdleComboState();
            //SetState(nextState);
        }

        if (CurrentState != null)
        {
            CurrentState.OnUpdate();
        }
            
    }

    private void SetState(State _newState)
    {
        nextState = null;
        if (CurrentState != null)
        {
            CurrentState.OnExit();
        }
        CurrentState = _newState;
        CurrentState.OnEnter(this);
    }

    public void SetNextState(State _newState)
    {
        if (_newState != null)
        {
            nextState = _newState;
        }
    }

    private void LateUpdate()
    {
        if (CurrentState != null)
            CurrentState.OnLateUpdate();
    }

    private void FixedUpdate()
    {
        if (CurrentState != null)
            CurrentState.OnFixedUpdate();
    }

    public void SetNextStateToMain()
    {
        nextState = mainStateType;
    }

    private void Awake()
    {
        SetNextStateToMain();

    }

    private void OnValidate()
    {
       
    }

}
