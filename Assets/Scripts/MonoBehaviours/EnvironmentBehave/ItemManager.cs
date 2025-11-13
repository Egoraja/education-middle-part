using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour, IPickUpAble
{
    private Transform targetTransform;
    private PlayerProgressManager playerProgressManager;
    public void Execute(PlayerProgressManager playerProgressManager)
    {       
        if (TryGetComponent(out IPickUpEffect pickUpEffect))
            pickUpEffect.ApplyEffect(playerProgressManager);      
        Destroy(gameObject, 0.01f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerProgressManager playerProgressManager))
        {
            targetTransform = other.transform;
            this.playerProgressManager = playerProgressManager;            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerProgressManager playerProgressManager))
        {
            targetTransform = null;
            this.playerProgressManager = null;            
        }
    }  

    private void Update()
    {
        if (targetTransform != null)
        {            
            float distance = Vector3.Distance(transform.position, targetTransform.position);
            if (distance < 1.5f)
            {
                Execute(playerProgressManager);
                targetTransform = null;
            }
           
        }
    }
}
