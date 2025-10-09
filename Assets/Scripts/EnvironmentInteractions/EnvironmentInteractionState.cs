
using UnityEngine;
using UnityEngine.UIElements;

public abstract class EnvironmentInteractionState : BaseState<EnvironmentInteractionStateMachine.EEnvironmentInteraction>

{
    private float movingAwayOffset = 0.005f;
    private bool shouldReset;

    protected EnvironmentInteractionContext Context;

    public EnvironmentInteractionState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteraction stateKey) :
        base (stateKey)
    {
        Context = context;
    }   

    protected bool CheckShouldReset()
    {
        if (shouldReset)
        {
            Context.LowestDistance = Mathf.Infinity;
            shouldReset = false;
            return true;
        }

        bool isPlayerStoped = Context.CharacterController.velocity == Vector3.zero;
        bool isMovingAway = CheckIsMovingAway();
        bool isBadAngle = CheckIsBadAngle();
        bool isPlayerJumping = Mathf.Round(Context.CharacterController.velocity.y) >= 1;
        if (isPlayerStoped || isMovingAway|| isBadAngle|| isPlayerJumping)
        {
            Context.LowestDistance = Mathf.Infinity;
            return true;
        }
        return false;
    }

    protected bool CheckIsBadAngle()
    {
        if (Context.CurrentInteractableCollider == null)
        {
            return false;
        }
        Vector3 targetDirection = Context.ClosestPointOnColliderFromShoulder - Context.CurrentShoulderTransform.position;
        Vector3 shoulderDirection = Context.CurrentBodySide == EnvironmentInteractionContext.EBodySide.Right ?
            Context.RootTransform.right : -Context.RootTransform.right;
        float dotProduct = Vector3.Dot(shoulderDirection, targetDirection.normalized);
        bool isBadAngle = dotProduct < 0;
        return isBadAngle;
    }

    protected bool CheckIsMovingAway()
    {
        float currentDisnatceTarget = Vector3.Distance(Context.RootTransform.position, Context.ClosestPointOnColliderFromShoulder);

        bool isSearchingForNewInteraction = Context.CurrentInteractableCollider == null;
        if (isSearchingForNewInteraction)
        {
            return false;
        }
        bool isGettingCloserToTarget = currentDisnatceTarget <= Context.LowestDistance;
        if (isGettingCloserToTarget)
        {
            Context.LowestDistance = currentDisnatceTarget;
            return false;
        }
        bool isMovingAwayFromTarget = currentDisnatceTarget > Context.LowestDistance + movingAwayOffset;
        if (isMovingAwayFromTarget)
        {
            Context.LowestDistance = Mathf.Infinity;
            return true;
        }
        return false;
    }


    private Vector3 GetClosestPoinOnCollider(Collider intersectingCollider, Vector3 positionToCheck)
    {
        return intersectingCollider.ClosestPoint(positionToCheck);    
    }
    protected void StartIKTargetPositionTracking(Collider intersectingCollider)
    {

        if (((1 << intersectingCollider.gameObject.layer) & Context.InteractableLayer) != 0 && Context.CurrentInteractableCollider == null)
        {
            Context.CurrentInteractableCollider = intersectingCollider;
            Vector3 closestPointFromRoot = GetClosestPoinOnCollider(intersectingCollider, Context.RootTransform.position);
            Context.SetCurrentSide(closestPointFromRoot);
            SetIKTargetPosition();
        }
    }
    protected void UpdateIKTargetPosition(Collider intersectingCollider)
    {
        if (intersectingCollider == Context.CurrentInteractableCollider)
        {          
            SetIKTargetPosition();
        }    
    }
    protected void ResetIKTargetPositionTracking(Collider intersectingCollider)
    {
        if (intersectingCollider == Context.CurrentInteractableCollider)
        {           
            Context.CurrentInteractableCollider = null;
            Context.ClosestPointOnColliderFromShoulder = Vector3.positiveInfinity;
            shouldReset = true;
        }    
    }

    private void SetIKTargetPosition()
    {
        Context.ClosestPointOnColliderFromShoulder = GetClosestPoinOnCollider(Context.CurrentInteractableCollider,
        new Vector3(Context.CurrentShoulderTransform.position.x, Context.CharacterShoulderHeight, Context.CurrentShoulderTransform.position.z));

        Vector3 rayDirection = Context.CurrentShoulderTransform.position - Context.ClosestPointOnColliderFromShoulder;
        Vector3 normalixedRayDirection = rayDirection.normalized;
        float offsetDistance = 0.1f;
        Vector3 offset = normalixedRayDirection * offsetDistance;
        Vector3 offsetPosition = Context.ClosestPointOnColliderFromShoulder + offset;
        Context.CurrentIKTargetTransform.position = new Vector3(offsetPosition.x, Context.InteractionPointYOffset, offsetPosition.z);        
    }
}
   
