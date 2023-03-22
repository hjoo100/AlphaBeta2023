//using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_GroundComboState : Scr_MeleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

       

        attackIndex = 2;
        duration = 0.5f;
        animator.Play("Melee2");
        playerCtrl.applyAttack();
        Debug.Log("Player melee2 pressed!");

    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if(fixedtime >= duration)
        {
            if (shouldCombo)
            {
                stateMachine.SetNextState(new Scr_GroundThirdHitState());
            }
                
            else
            {
                stateMachine.SetNextStateToMain();
            }
                
        }
    }
}
