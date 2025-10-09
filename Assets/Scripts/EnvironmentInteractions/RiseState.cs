using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseState : EnvironmentInteractionState
{
    private float elapsedTime = 0;
    private float resetDuration = 2;
    private float riseWeight = 1;
    private float lerpDuration = 5;
    private Quaternion expectedHandRotation;
    private float rotationSpeed = 100;
    private float maxDistance = 1f;    
    private float touchDistanceThreshold = 0.1f;
    private float touchTimeThreshold = 1;

    public RiseState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteraction estate) :
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
        if (CheckShouldReset())
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteraction.Reset;        
        }
        if (Vector3.Distance(Context.CurrentIKTargetTransform.position, Context.ClosestPointOnColliderFromShoulder) < touchDistanceThreshold
            && elapsedTime >= touchTimeThreshold)
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteraction.Touch;
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
        CalculateExpectedHandRotation();
        Context.InteractionPointYOffset = Mathf.Lerp(Context.InteractionPointYOffset,
            Context.ClosestPointOnColliderFromShoulder.y, elapsedTime / lerpDuration);
        Context.CurrentIKConstraint.weight = Mathf.Lerp(Context.CurrentIKConstraint.weight, riseWeight, elapsedTime / lerpDuration);
        Context.CurrentMultiRConstraint.weight = Mathf.Lerp(Context.CurrentMultiRConstraint.weight, riseWeight, elapsedTime / lerpDuration);
        Context.CurrentIKTargetTransform.rotation = Quaternion.RotateTowards(Context.CurrentIKTargetTransform.rotation, expectedHandRotation, rotationSpeed * Time.deltaTime);
        elapsedTime += Time.deltaTime;
    }

    private void CalculateExpectedHandRotation()
    {
        Vector3 startPos = Context.CurrentShoulderTransform.position;
        Vector3 endPos = Context.ClosestPointOnColliderFromShoulder;
        Vector3 direction = (endPos-startPos).normalized;
        RaycastHit hit;

        if (Physics.Raycast(startPos,direction,out hit, maxDistance, Context.InteractableLayer))
        {
            Vector3 surfaceNormal = hit.normal;
            Vector3 targetForward = surfaceNormal;
            expectedHandRotation = Quaternion.LookRotation(targetForward, Vector3.up);
        }    
    }
}
