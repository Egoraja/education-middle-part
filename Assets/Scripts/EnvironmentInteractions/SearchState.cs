using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : EnvironmentInteractionState
{
    private float approachDistanceThreshold = 2.0f;
    public SearchState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteraction estate) :
         base(context, estate)    {
        EnvironmentInteractionContext Contex = context;
    }

    public override void EnterState()
    {}

    public override void ExitState()
    {}

    public override EnvironmentInteractionStateMachine.EEnvironmentInteraction GetNextState()
    {
        if (CheckShouldReset())
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteraction.Reset;
        }

        bool isCloseToTarget = Vector3.Distance(Context.ClosestPointOnColliderFromShoulder,
            Context.RootTransform.position) < approachDistanceThreshold;

        bool isClosestPointOnColliderValid = Context.ClosestPointOnColliderFromShoulder != Vector3.positiveInfinity;
        if (isCloseToTarget && isClosestPointOnColliderValid)
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteraction.Approach;
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
    {}
}
