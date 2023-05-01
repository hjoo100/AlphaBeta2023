using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_GroundFinishState : Scr_MeleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        attackIndex = 5;
        duration = 0.8f;
        animator.Play("Melee5");
        playerCtrl.InvokeAttack(0.25f);
        Debug.Log("Player melee5 pressed!");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if(fixedtime >= duration)
        {
            stateMachine.SetNextStateToMain();
        }
    }
}
