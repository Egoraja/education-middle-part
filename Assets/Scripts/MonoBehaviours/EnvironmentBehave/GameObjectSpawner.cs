using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Collections;

[BurstCompile]
public class GameObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private int gameObjectCount;
    [SerializeField] private float spawnRadius;
    [SerializeField] private Transform playerTransform;

    [Header("GameObjectOptions")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float distanceThreshold = 5f;

    private bool isComplete = false;

    private TransformAccessArray accessArray;
    private Transform[] gameObjectTransforms;

    private Vector3[] startPositions;
    private NativeArray<float3> nativeArrayStartPosition;

    private List<GameObject> gameObjects = new List<GameObject>();

    private void Awake()
    {
        gameObjectTransforms = new Transform[gameObjectCount];
        startPositions = new Vector3[gameObjectCount];

        for (int i = 0; i < gameObjectCount; i++)
        {
            Vector2 rndPosition = UnityEngine.Random.insideUnitCircle * spawnRadius + new Vector2(transform.position.x, transform.position.z);
            Vector3 spawnPosition = new Vector3(rndPosition.x, 1, rndPosition.y);
            GameObject newGameObject = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
            newGameObject.transform.parent = transform;

            if (newGameObject.TryGetComponent<SphereBehaviour>(out SphereBehaviour sphereBehaviour))
                sphereBehaviour.SetConfig(playerTransform, speed, distanceThreshold);

            gameObjects.Add(newGameObject);
            gameObjectTransforms[i] = newGameObject.transform;
            startPositions[i] = spawnPosition;
        }

        nativeArrayStartPosition = new NativeArray<float3>(gameObjectCount, Allocator.Persistent);
        for (int i = 0; i < gameObjectCount; i++)
        {
            nativeArrayStartPosition[i] = startPositions[i];
        }

        accessArray = new TransformAccessArray(gameObjectTransforms);
        isComplete = true;
    }

    private void Update()
    {
        if (isComplete)
        {
            ControlSphereJob job = new ControlSphereJob()
            {
                PlayerPosition = playerTransform.position,
                DeltaTime = Time.deltaTime,
                DistanceThreshold = distanceThreshold,
                Speed = speed,
                StartPositions = nativeArrayStartPosition
            };

            JobHandle jobHandle = job.Schedule(accessArray);
            jobHandle.Complete();
        }
    }
    private void OnDestroy()
    {
        if (accessArray.isCreated)
            accessArray.Dispose();

        if (nativeArrayStartPosition.IsCreated)
            nativeArrayStartPosition.Dispose();
    }
}

