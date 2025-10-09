using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchState : EnvironmentInteractionState
{
    private float elapsedTime = 0;
    private float resetThreshold = 1f;
    public TouchState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteraction estate) :
        base(context, estate)
    {
        EnvironmentInteractionContext Contex = context;
    }

    public override void EnterState()
    {
        elapsedTime = 0;        
    }

    public override void ExitState()
    {}

    public override EnvironmentInteractionStateMachine.EEnvironmentInteraction GetNextState()
    {        
        if (elapsedTime > resetThreshold || CheckShouldReset())
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteraction.Reset;
        }
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other)
    {
        StartIKTargetPositionTracking(other);
    }

    public override void OnTriggerExit(Collider other)
    {
        ResetIKTargetPositionTracking(other);
    }

    public override void OnTriggerStay(Collider other)
    {
        UpdateIKTargetPosition(other);
    }

    public override void UpdateState()
    {
        elapsedTime += Time.deltaTime;
    }
}
