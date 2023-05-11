using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scr_GroundEntryState : Scr_MeleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        attackIndex = 1;
        duration = 0.6f;
        animator.Play("Melee1");
        playerCtrl.InvokeAttack(0.125f);
        Debug.Log("Player Melee1 pressed!");

    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if(fixedtime >= duration)
        {
            if (shouldCombo)
            {
                stateMachine.SetNextState(new Scr_GroundComboState());
            }    
                

            else
            {
                stateMachine.SetNextStateToMain();
            }
                
        }
    }


}
