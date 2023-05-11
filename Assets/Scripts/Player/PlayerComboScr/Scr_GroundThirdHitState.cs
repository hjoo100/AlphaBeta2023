using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_GroundThirdHitState: Scr_MeleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        attackIndex = 3;
        duration = 0.5f;
        animator.Play("Melee3");
        playerCtrl.InvokeAttack(0.125f);
        Debug.Log("Player melee3 pressed!");

    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (fixedtime >= duration)
        {
            if (shouldCombo)
            {
                stateMachine.SetNextState(new Scr_GroundFourthHitState());
            }

            else
            {
                stateMachine.SetNextStateToMain();
            }

        }
    }
}
