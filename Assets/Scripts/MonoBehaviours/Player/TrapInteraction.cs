using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class TrapInteraction : MonoBehaviour
{
    [SerializeField] private MultiAimConstraint headTracking;
    [SerializeField] private ChainIKConstraint leftHandChainIKconstrat;
    [SerializeField] private MultiAimConstraint leftHandTracking;
    [SerializeField] private float heightOffset = 0.1f;

    private Vector3 offset;
    private Transform trapGameObject;   
    private bool trapIsActive = false;
    private bool isTouching = false;
    private float timerAnimation = float.MinValue;
    private float baseWeight = 0;
    private PlayerProgressManager progressManager;   

    private void Start()
    {
        progressManager = GetComponent<PlayerProgressManager>();
        offset = new Vector3(0, heightOffset, 0);
    }

    private void Update()
    {
        if (trapIsActive == true && timerAnimation > 0)
            StartSecondPartInteraction();
        if (trapIsActive == false && timerAnimation > 0)
            StartThirdPartInteraction();
        if (isTouching)
            StartFourthPartInteraction();
        if (trapGameObject != null && isTouching == false)
            headTracking.data.sourceObjects[0].transform.position = trapGameObject.transform.position;
    }

    public void StartFirstPartTrapInteraction(Transform trapTransform, float timer)
    {
        progressManager.IsMoving = false;  
        trapGameObject = trapTransform;
        timerAnimation = timer;       
        SetConstraintData();
    }

    private void SetConstraintData()
    {
        trapIsActive = true;
        headTracking.weight = 1f;
    }
    
    private void StartSecondPartInteraction()
    {       
        timerAnimation -= Time.deltaTime;
        if (timerAnimation <= 0)
        {            
            trapIsActive = false;            
            trapGameObject.GetComponent<DeathTrapMover>().AttackMode(transform);
            timerAnimation = trapGameObject.GetComponent<DeathTrapMover>().AttackModeDuration;
        }
    }

    private void StartThirdPartInteraction()
    {
        timerAnimation -= Time.deltaTime;
        if (timerAnimation <= 0)
        {
            isTouching = true;
            timerAnimation = float.MinValue;
        }
    }

    private void StartFourthPartInteraction()
    {
        leftHandChainIKconstrat.data.target.position = trapGameObject.transform.position - offset;
        leftHandTracking.data.sourceObjects[0].transform.position = trapGameObject.transform.position;
        headTracking.data.sourceObjects[0].transform.position = trapGameObject.transform.position - offset;

        leftHandTracking.weight = baseWeight;
        leftHandChainIKconstrat.weight = baseWeight;
        baseWeight += Time.deltaTime;
        if (baseWeight >= 1)
        {
            isTouching = false;
            Destroy(trapGameObject.gameObject);
            progressManager.PlayerIsDead();
        }
    }
}
