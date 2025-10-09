using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetState : EnvironmentInteractionState
{
    private float elapsedTime = 0;
    private float resetDuration = 2;
    private float lerpDuration = 10;
    private float rotationSpeed = 500;

    public ResetState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteraction estate) :
        base(context, estate)
    {
        EnvironmentInteractionContext Contex = context;
    }
    public override void EnterState()
    {
        Debug.Log("Enter Reset State");
        elapsedTime = 0;
        Context.ClosestPointOnColliderFromShoulder = Vector3.positiveInfinity;
        Context.CurrentInteractableCollider = null;
    }

    public override void ExitState()
    {}

    public override EnvironmentInteractionStateMachine.EEnvironmentInteraction GetNextState()
    {
        bool isMoving = Context.CharacterController.velocity != Vector3.zero;        
        if (elapsedTime >= resetDuration && isMoving)
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteraction.Search;
        }
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other)
    {}

    public override void OnTriggerExit(Collider other)
    {}

    public override void OnTriggerStay(Collider other)
    {}

    public override void UpdateState()
    {       
        elapsedTime += Time.deltaTime;        
        Context.InteractionPointYOffset = Mathf.Lerp(Context.InteractionPointYOffset, Context.ColliderCenterY, elapsedTime / lerpDuration);
        Context.CurrentIKConstraint.weight = Mathf.Lerp(Context.CurrentIKConstraint.weight, 0, elapsedTime / lerpDuration);
        Context.CurrentMultiRConstraint.weight = Mathf.Lerp(Context.CurrentMultiRConstraint.weight, 0, elapsedTime / lerpDuration);
        Context.CurrentIKTargetTransform.localPosition = Vector3.Lerp(Context.CurrentIKTargetTransform.localPosition, Context.CurrentOriginalTargetPosition, elapsedTime / lerpDuration);
        Context.CurrentIKTargetTransform.rotation = Quaternion.RotateTowards(Context.CurrentIKTargetTransform.rotation, Context.OriginalTargetRotation, rotationSpeed * Time.deltaTime);
    }
}
