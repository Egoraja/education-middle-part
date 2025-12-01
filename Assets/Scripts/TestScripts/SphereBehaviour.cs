using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Jobs;
using UnityEngine.Jobs;

public class SphereBehaviour : MonoBehaviour
{
    private float speed;
    private float distanceThreshold;

    private Transform playerTransform;    

    public void SetConfig(Transform player, float speed, float distanceThreshold)
    {
        playerTransform = player;            
        this.speed = speed;
        this.distanceThreshold = distanceThreshold;
    }

    private void FixedUpdate()
    {
        if (playerTransform != null)
        {            
            float distance = Vector3.Distance(playerTransform.position, transform.position);
            if (distance <= distanceThreshold)
            {
                Vector3 direction = (playerTransform.position - transform.position);
                direction.y = 0f;
                direction = direction.normalized;
                transform.position += -direction * Time.deltaTime * speed;
            }     
        }
    }    
}
