using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;

[BurstCompile]
public struct ControlSphereJob : IJobParallelForTransform
{
    [ReadOnly] public float3 PlayerPosition;
    [ReadOnly] public float DeltaTime;
    [ReadOnly] public float DistanceThreshold;
    [ReadOnly] public float Speed;
    [ReadOnly] public NativeArray<float3> StartPositions;

    public void Execute(int index, TransformAccess transform)
    {      

        float3 currentPosition = (float3)transform.position;
        float3 startPosition = StartPositions[index];

        float distanceToPlayer = math.distance(PlayerPosition, currentPosition);
        float distanceToStart = math.distance(currentPosition, startPosition);
        
        if (distanceToPlayer <= DistanceThreshold)
        {
            float3 direction = PlayerPosition - currentPosition;
            direction.y = 0;
            direction = math.normalize(direction);

            float3 newPosition = currentPosition - direction * DeltaTime * Speed;
            transform.position = newPosition;
        }
     
        else if (distanceToStart > 0.1f)
        {
            float3 directionToStart = startPosition - currentPosition;
            directionToStart.y = 0;
            directionToStart = math.normalize(directionToStart);

            float3 newPosition = currentPosition + directionToStart * DeltaTime * Speed;
            transform.position = newPosition;
        }
    }
}
