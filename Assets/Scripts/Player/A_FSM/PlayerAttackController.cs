using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerAttackController : AdvancedFSM
{
    public Animator playerAnimator;

    //Initialize the Finite state machine for the NPC tank
    protected override void Initialize()
    {
        playerAnimator = playerScr.gameObject.GetComponent<Animator>();

        //Start Doing the Finite State Machine
        ConstructFSM();
    }

    //Update each frame
    protected override void FSMUpdate()
    {
        
    }

    protected override void FSMFixedUpdate()
    {
       
    }

    public void SetTransition(Transition t)
    {
        PerformTransition(t);
    }

    private void ConstructFSM()
    {
        //Get the list of points
        

    }

  



 
}
