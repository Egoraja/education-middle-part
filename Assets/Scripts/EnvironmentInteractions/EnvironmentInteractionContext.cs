using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnvironmentInteractionContext : MonoBehaviour
{
    private TwoBoneIKConstraint leftHandIKConstraint;
    private TwoBoneIKConstraint rightHandIKConstraint;
    private MultiRotationConstraint leftHandMultiRConstraint;
    private MultiRotationConstraint rightHandMultiRConstraint;
    private CharacterController characterController;
    private Transform rootTransform;
    private LayerMask interactableLayer;
    private Collider currentInteractableCollider;
    private float characterShoulderHeight;
    private Vector3 leftOriginalTargetPosition;
    private Vector3 rightOriginalTargetPositon;
    

    public TwoBoneIKConstraint LeftHandIKConstraint => leftHandIKConstraint;
    public TwoBoneIKConstraint RightHandIKConstraint => rightHandIKConstraint;
    public MultiRotationConstraint LeftHandMultiRConstraint => leftHandMultiRConstraint;
    public MultiRotationConstraint RightHandMultiRConstraint => rightHandMultiRConstraint;
    public CharacterController CharacterController => characterController;
    public LayerMask InteractableLayer => interactableLayer;
    public Transform RootTransform => rootTransform;
    public float CharacterShoulderHeight => characterShoulderHeight;
    public Collider CurrentInteractableCollider { get { return currentInteractableCollider; } set { currentInteractableCollider = value; } }
    public TwoBoneIKConstraint CurrentIKConstraint { get; private set; }
    public MultiRotationConstraint CurrentMultiRConstraint { get; private set; }
    public Transform CurrentIKTargetTransform { get; private set; }
    public Transform CurrentShoulderTransform { get; private set; }    
    public EBodySide CurrentBodySide { get; private set; }
    public Vector3 ClosestPointOnColliderFromShoulder { get; set; } = Vector3.positiveInfinity;
    public float InteractionPointYOffset { get; set; } = 0;
    public float ColliderCenterY { get; set; }
    public Vector3 CurrentOriginalTargetPosition { get; private set; }
    public Quaternion OriginalTargetRotation { get; private set; }
    public float LowestDistance { get; set; } = Mathf.Infinity;
    public enum EBodySide
    { 
    Right,
    Left
    }

    public EnvironmentInteractionContext(TwoBoneIKConstraint leftHandIKConstraint,
        TwoBoneIKConstraint rightHandIKConstraint, MultiRotationConstraint leftHandMultiRConstraint,
        MultiRotationConstraint rightHandMultiRConstraint, CharacterController characterController, Transform rootTransform, LayerMask layerMask)
    {
        this.leftHandIKConstraint = leftHandIKConstraint;
        this.rightHandIKConstraint = rightHandIKConstraint;
        this.leftHandMultiRConstraint = leftHandMultiRConstraint;
        this.rightHandMultiRConstraint = rightHandMultiRConstraint;
        this.characterController = characterController;
        this.rootTransform = rootTransform;
        interactableLayer = layerMask;
        leftOriginalTargetPosition = leftHandIKConstraint.data.target.transform.localPosition;
        rightOriginalTargetPositon = rightHandIKConstraint.data.target.transform.localPosition;
        OriginalTargetRotation = LeftHandIKConstraint.data.target.rotation;
    }

    public void SetCurrentSide(Vector3 positionToCheck)
    {
        Vector3 leftShoulder = leftHandIKConstraint.data.root.transform.position;
        Vector3 righttShoulder = rightHandIKConstraint.data.root.transform.position;
        bool isLeftCloser = Vector3.Distance(positionToCheck, leftShoulder) < Vector3.Distance(positionToCheck, righttShoulder);      
        if (isLeftCloser == true)
        {
            CurrentBodySide = EBodySide.Left;
            CurrentIKConstraint = leftHandIKConstraint;
            CurrentMultiRConstraint = leftHandMultiRConstraint;
            CurrentOriginalTargetPosition = leftOriginalTargetPosition;
        }
        else
        {
            CurrentBodySide = EBodySide.Right;
            CurrentIKConstraint = rightHandIKConstraint;
            CurrentMultiRConstraint = rightHandMultiRConstraint;
            CurrentOriginalTargetPosition = rightOriginalTargetPositon;
        }        
        characterShoulderHeight = CurrentIKConstraint.data.root.transform.position.y;
        CurrentShoulderTransform = CurrentIKConstraint.data.root.transform;
        CurrentIKTargetTransform = CurrentIKConstraint.data.target.transform;        
    }
}
