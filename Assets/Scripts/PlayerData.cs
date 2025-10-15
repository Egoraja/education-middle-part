using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("Default Values")]
    [SerializeField] private float defaultSpeed = 1f;
    [SerializeField] private float defaultHealth = 10f;
    [Space(5)]

    [Header("Current Values")]
    [SerializeField] private float currentSpeed;
    [SerializeField] private float currentHealth;
    public float Speed { get { return currentSpeed; } private set { currentSpeed = value; } }
    public float Health { get { return currentHealth; } private set { currentHealth = value; } }


    private void Start()
    {
        InitializeWithDefaults();
        if (SaveAndLoadData.Instance != null)
        {
            SaveAndLoadData.Instance.OnDataLoaded += ApplyLoadedData;
            SaveAndLoadData.Instance.OnDataLoadFailed += OnDataLoadFailed;

            if (SaveAndLoadData.Instance.IsDataLoaded)
            {
                ApplyLoadedData(SaveAndLoadData.Instance.CurrentConfig);
            }
        }
        else
        {
            Debug.LogWarning("SaveAndLoadData instance not found!");
        }
    }

    private void InitializeWithDefaults()
    {        
        currentSpeed = defaultSpeed;
        currentHealth = defaultHealth;
        Debug.Log($"Initialized with defaults - Speed: {Speed}, Health: {Health}");
    }

    private void ApplyLoadedData(PlayerDataConfig config)
    {
        currentSpeed = config.speed;
        currentHealth = config.health;
        Debug.Log($"Data applied: Speed={Speed}, Health={Health}");
    }

    private void OnDestroy()
    {
        if (SaveAndLoadData.Instance != null)
        {
            SaveAndLoadData.Instance.OnDataLoaded -= ApplyLoadedData;
            SaveAndLoadData.Instance.OnDataLoadFailed -= OnDataLoadFailed;
        }
    }

    private void OnDataLoadFailed(string error)
    {
        Debug.LogWarning($"Failed to load player data: {error}. Using default values.");          
    }
}

