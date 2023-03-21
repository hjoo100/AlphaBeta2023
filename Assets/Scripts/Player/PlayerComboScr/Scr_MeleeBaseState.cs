using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_MeleeBaseState : State
{
    //time for the state being active
    public float duration;

    protected Animator animator;

    protected bool shouldCombo;

    //No of the combo
    protected int attackIndex;

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        playerCtrl.isAttacking = true;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if(Input.GetKeyDown(KeyCode.Z))
        {
            shouldCombo = true;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        playerCtrl.isAttacking = false;
    }
}
