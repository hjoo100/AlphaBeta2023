using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_FSMBase:MonoBehaviour
{
    protected S_FSMState currentState;

    protected List<S_FSMState> states;

    protected void AddStateAndMapping()
    {
        states = new List<S_FSMState>();
        //states.add

    }

    public void SwitchState(S_FSMState fsmState)
    {
        currentState.OnStateExit(this);//exit old state

        currentState = fsmState;

        currentState.OnStateEnter(this);//enter New state

       
    }

    protected void Start()
    {
        AddStateAndMapping();
    }

    private void Update()
    {
        currentState.CheckCondition(this);
        currentState.OnStateStay(this);

    }

}
