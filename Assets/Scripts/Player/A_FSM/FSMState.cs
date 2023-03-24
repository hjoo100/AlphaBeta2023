using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class FSMState
{

    protected Dictionary<Transition, FSMStateID> map = new Dictionary<Transition, FSMStateID>();
    protected FSMStateID stateID;
    public FSMStateID ID { get { return stateID; } }

    protected float time;
    protected float fixedtime;
    protected float latetime;

    public void AddTransition(Transition transition, FSMStateID id)
    {
        //Since this is a Deterministc FSM,
        //Check if the current transition was already inside the map
        if (map.ContainsKey(transition))
        {
            Debug.LogWarning("FSMState ERROR: transition is already inside the map");
            return;
        }

        map.Add(transition, id);
        Debug.Log("Added : " + transition + " with ID : " + id);

    }

    /// <summary>
    /// This method deletes a pair transition-state from this state´s map.    
    /// </summary>
    public void DeleteTransition(Transition trans)
    {
        // Check if the pair is inside the map before deleting
        if (map.ContainsKey(trans))
        {
            map.Remove(trans);
            return;
        }
        Debug.LogError("FSMState ERROR: Transition passed was not on this State List");
    }


    /// <summary>
    /// This method returns the new state the FSM should be if
    ///    this state receives a transition  
    /// </summary>
    public FSMStateID GetOutputState(Transition trans)
    {
        return map[trans];
    }

}


   
