using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class S_FSMState 
{
    public Dictionary<S_FSMCondition, S_FSMState> mapping;

    public List<S_FSMCondition> conditions;

    public void ConfigMapping(S_FSMCondition condition, S_FSMState state)

    {

        mapping.Add(condition, state);

    }

    public void CheckCondition(S_FSMBase fsmBase)

    {

        //便利自身所有的条件

        foreach (S_FSMCondition condition in mapping.Keys)

        {

            if (condition.HandleCondition(fsmBase))

            {

                

                fsmBase.SwitchState(mapping[condition]);

                return;

            }

        }

    }

    
    
    public void CheckCondition()
    {
        foreach(S_FSMCondition condition in conditions)
        {
            //switch to corresponding state
            return;
        }
    }


    public abstract void HandleState();

    public abstract void OnStateEnter(S_FSMBase fsmBase);

    public abstract void OnStateStay(S_FSMBase fsmBase);

    public abstract void OnStateExit(S_FSMBase fsmBase);

    
}
