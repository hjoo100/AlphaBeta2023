using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected float time { get; set; }
    protected float fixedtime { get; set; }
    protected float latetime { get; set; }

    public StateMachine stateMachine;

    public Scr_PlayerCtrl playerCtrl;
    public virtual void OnEnter(StateMachine _stateMachine)
    {
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<Scr_PlayerCtrl>();
        if(playerCtrl == null)
        {
            Debug.Log("Failed to find player scr");
        }
        stateMachine = _stateMachine;
    }


    public virtual void OnUpdate()
    {
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<Scr_PlayerCtrl>();
        if (playerCtrl == null)
        {
            Debug.Log("Failed to find player scr");
        }
        time += Time.deltaTime;
    }

    public virtual void OnFixedUpdate()
    {
        fixedtime += Time.deltaTime;
    }
    public virtual void OnLateUpdate()
    {
        latetime += Time.deltaTime;
    }

    public virtual void OnExit()
    {

    }

  

 
    protected static void Destroy(UnityEngine.Object obj)
    {
        UnityEngine.Object.Destroy(obj);
    }

   
    // Returns the component of type T if the game object has one attached, null if it doesn't.
    protected T GetComponent<T>() where T : Component { return stateMachine.GetComponent<T>(); }

  
    // Returns the component of Type <paramref name="type"/> if the game object has one attached, null if it doesn't.
    protected Component GetComponent(System.Type type) { return stateMachine.GetComponent(type); }

 
    // Returns the component with name <paramref name="type"/> if the game object has one attached, null if it doesn't.
    protected Component GetComponent(string type) { return stateMachine.GetComponent(type); }
    
}
