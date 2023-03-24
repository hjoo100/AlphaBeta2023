using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class is adapted and modified from the FSM implementation class available on UnifyCommunity website
/// The license for the code is Creative Commons Attribution Share Alike.
/// It's originally the port of C++ FSM implementation mentioned in Chapter01 of Game Programming Gems 1
/// You're free to use, modify and distribute the code in any projects including commercial ones.
/// Please read the link to know more about CCA license @http://creativecommons.org/licenses/by-sa/3.0/
/// </summary>

public enum Transition
{    
    Idle = 0,
    IdleToFirst,
    FirstToSec,
    SecToThird,
    ThirdToFourth,
    FourthToFifth
}

public enum FSMStateID
{    
    IdleState = 0,
    FirstHit,
    SecondHit,
    ThirdHit,
    FourthHit,
    FinalHit
}

public class AdvancedFSM : FSM 
{
    private List<FSMState> fsmStates;

    public Scr_PlayerCtrl playerScr;

    //The fsmStates are not changing directly but updated by using transitions
    private FSMStateID currentStateID;
    public FSMStateID CurrentStateID { get { return currentStateID; } }

    private FSMState currentState;
    public FSMState CurrentState { get { return currentState; } }

    public AdvancedFSM()
    {
        fsmStates = new List<FSMState>();
    }

    private void Awake()
    {
        playerScr = GameObject.FindGameObjectWithTag("Player").GetComponent<Scr_PlayerCtrl>();
    }

    /// Add New State into the list

    public void AddFSMState(FSMState fsmState)
    {
        // Check for Null reference before deleting
        if (fsmState == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
        }

        // First State inserted is also the Initial state
        //   the state the machine is in when the simulation begins
        if (fsmStates.Count == 0)
        {
            fsmStates.Add(fsmState);
            currentState = fsmState;
            currentStateID = fsmState.ID;
            return;
        }

        // Add the state to the List if it is not inside it
        foreach (FSMState state in fsmStates)
        {
            if (state.ID == fsmState.ID)
            {
                Debug.LogError("FSM ERROR: Trying to add a state that was already inside the list");
                return;
            }
        }

        //If no state in the current then add the state to the list
        fsmStates.Add(fsmState);
    }

    
    //This method delete a state from the FSM List if it exists,     
    public void DeleteState(FSMStateID fsmState)
    {
        // Search the List and delete the state if it�s inside it
        foreach (FSMState state in fsmStates)
        {
            if (state.ID == fsmState)
            {
                fsmStates.Remove(state);
                return;
            }
        }
        Debug.LogError("FSM ERROR: The state passed was not on the list. Impossible to delete it");
    }

    
    /// This method tries to change the state the FSM is in based on
    /// the current state and the transition passed. 
   
    public void PerformTransition(Transition trans)
    {  
        // Check if the currentState has the transition passed as argument
        FSMStateID id = currentState.GetOutputState(trans);        

        // Update the currentStateID and currentState		
        currentStateID = id;
        foreach (FSMState state in fsmStates)
        {
            if (state.ID == currentStateID)
            {
                currentState = state;
                break;
            }
        }
    }
}
