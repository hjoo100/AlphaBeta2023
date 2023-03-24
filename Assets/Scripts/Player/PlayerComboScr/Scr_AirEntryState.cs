using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_AirEntryState : Scr_MeleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        attackIndex = 1;
        duration = 0.5f;
        animator.Play("AirMelee");
        playerCtrl.applyAttack();
        playerCtrl.isAirAttacked = true;
        Debug.Log("Player Melee air pressed!");

    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (fixedtime >= duration)
        {
           
          stateMachine.SetNextStateToMain();
          

        }
    }
}
