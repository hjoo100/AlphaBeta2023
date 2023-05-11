using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_GroundFourthHitState: Scr_MeleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        attackIndex = 4;
        duration = 0.5f;
        animator.Play("Melee4");
        playerCtrl.InvokeAttack(0.125f);
        Debug.Log("Player melee4 pressed!");

    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (fixedtime >= duration)
        {
            if (shouldCombo)
            {
                stateMachine.SetNextState(new Scr_GroundFinishState());
            }

            else
            {
                stateMachine.SetNextStateToMain();
            }

        }
    }
}
