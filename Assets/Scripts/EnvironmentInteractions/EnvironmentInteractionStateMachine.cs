using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Assertions;


public class EnvironmentInteractionStateMachine : ManagerState<EnvironmentInteractionStateMachine.EEnvironmentInteraction>
{
    [SerializeField] private TwoBoneIKConstraint _leftHandIKConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightHandIKConstraint;

    [SerializeField] private MultiRotationConstraint _leftHandMultiRConstraint;
    [SerializeField] private MultiRotationConstraint _rightHandMultiRConstraint;

    [SerializeField] private CharacterController _characterController;
    [SerializeField] private LayerMask interactableLayer;    

    private EnvironmentInteractionContext context;

    private void Awake()
    {
        context = new EnvironmentInteractionContext(_leftHandIKConstraint, _rightHandIKConstraint,
            _leftHandMultiRConstraint, _rightHandMultiRConstraint, _characterController, transform.root, interactableLayer);
        ValidateConstraints();
        InitializeState();
        ConstructEnvironmentDetectionCollider();
    }   
   
    private void ValidateConstraints()
    {
        Assert.IsNotNull(_leftHandIKConstraint, "Left IK constraint is mot assigned.");
        Assert.IsNotNull(_rightHandIKConstraint, "Right IK constraint is mot assigned.");
        Assert.IsNotNull(_leftHandMultiRConstraint, "Left MultiR constraint is mot assigned.");
        Assert.IsNotNull(_rightHandMultiRConstraint, "Right MultiR constraint is mot assigned.");
        Assert.IsNotNull(_characterController, "CharacterController is mot assigned.");
    }

    private void InitializeState()
    {
        States.Add(EEnvironmentInteraction.Reset, new ResetState(context, EEnvironmentInteraction.Reset));
        States.Add(EEnvironmentInteraction.Search, new SearchState(context, EEnvironmentInteraction.Search));
        States.Add(EEnvironmentInteraction.Approach, new ApproachState(context, EEnvironmentInteraction.Approach));
        States.Add(EEnvironmentInteraction.Rise, new RiseState(context, EEnvironmentInteraction.Rise));
        States.Add(EEnvironmentInteraction.Touch, new TouchState(context, EEnvironmentInteraction.Touch));
        CurrentState = States[EEnvironmentInteraction.Search];
    }

    private void ConstructEnvironmentDetectionCollider()
    {    
        context.ColliderCenterY = _characterController.center.y;
    }

    public enum EEnvironmentInteraction
    { 
        Search,
        Approach,
        Rise,
        Touch,
        Reset
    }
}
