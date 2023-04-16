using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_GuardState : State
{
    public float duration = 0.5f;
    public bool isDefending = false;
    Animator playerAnimator;

  
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        isDefending = true;
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        playerAnimator.Play("Guard");
        playerCtrl.SetDefend(true);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if(isDefending)
        {
            duration -= Time.deltaTime;
            if(duration <= 0)
            {
                isDefending = false;
                playerCtrl.SetDefend(false);
                stateMachine.SetNextStateToMain();
            }
        }
        
    }
    public override void OnExit()
    {


        base.OnExit();

    }
}
