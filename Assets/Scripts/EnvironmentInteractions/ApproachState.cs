using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachState : EnvironmentInteractionState
{
    private float elapsedTime = 0.0f;
    private float lerpDuration = 5;
    private float approachDuration = 2f;
    private float approachWeight = 0.5f;
    private float approachRotationWeight = 0.75f;
    private float rotationSpeed = 500f;
    private float riseDistanceThreshold = 0.5f;
    public ApproachState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteraction estate) :
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
        bool isOverStateLifeDuration = elapsedTime >= approachDuration;
        if (isOverStateLifeDuration || CheckShouldReset())
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteraction.Reset;
        }
        bool isWithinArmsReach =
            Vector3.Distance(Context.ClosestPointOnColliderFromShoulder, Context.CurrentShoulderTransform.position) < riseDistanceThreshold;
        bool isClosestPointOnColliderReal = Context.ClosestPointOnColliderFromShoulder != Vector3.positiveInfinity;

        if (isWithinArmsReach && isClosestPointOnColliderReal)
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteraction.Rise;
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
        Quaternion expectedGroundRotation = Quaternion.LookRotation(-Vector3.up, Context.RootTransform.forward);
        elapsedTime += Time.deltaTime;

        Context.CurrentIKTargetTransform.rotation =
            Quaternion.RotateTowards(Context.CurrentIKTargetTransform.rotation, expectedGroundRotation, rotationSpeed * Time.deltaTime);

        Context.CurrentIKConstraint.weight = 
            Mathf.Lerp(Context.CurrentIKConstraint.weight, approachWeight, elapsedTime / lerpDuration);

        Context.CurrentMultiRConstraint.weight =
            Mathf.Lerp(Context.CurrentMultiRConstraint.weight, approachRotationWeight, elapsedTime / lerpDuration);
    }
}
