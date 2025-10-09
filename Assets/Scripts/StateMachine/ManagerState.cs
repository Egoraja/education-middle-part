using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManagerState<Estate>: MonoBehaviour where Estate: Enum
{   
    protected Dictionary<Estate, BaseState<Estate>> States = new Dictionary<Estate, BaseState<Estate>>();    
    public BaseState<Estate> CurrentState { get; protected set; }

    protected bool IsTransitionState = false;
    
    private void Start() 
    {
        CurrentState.EnterState();
    }

    private void Update() 
    {       
        Estate nextStateKey = CurrentState.GetNextState();        
        if (IsTransitionState == false && nextStateKey.Equals(CurrentState.StateKey))
        {
        CurrentState.UpdateState();
        }
        else if (IsTransitionState == false)
        {
            TransitionToState(nextStateKey);
        }
    }

    public void TransitionToState(Estate stateKey)
    {
        IsTransitionState = true;
        CurrentState.ExitState();
        CurrentState = States[stateKey];
        CurrentState.EnterState();
        IsTransitionState = false;    
    }

    private void OnTriggerEnter(Collider other)
    {        
        CurrentState.OnTriggerEnter(other);    
    }
    private void OnTriggerStay(Collider other)
    {
        CurrentState.OnTriggerStay(other);    
    }
    private void OnTriggerExit(Collider other)
    {
        CurrentState.OnTriggerExit(other);
    }
    private void OnCollisionEnter(Collision other) { }
    private void OnCollisionStay(Collision other) { }
    private void OnCollisionExit(Collision other) { }
}
