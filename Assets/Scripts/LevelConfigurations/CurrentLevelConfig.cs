using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentLevelConfig : MonoBehaviour
{
    [Header("Default Values")]
    [SerializeField] private int defaultNumbersOfEnemy = 1;
    [SerializeField] private int defaultNumbersOfBonus = 1;   
    [Space(5)]

    [Header("Current Values")]
    [SerializeField] private int currentNumbersOfEnemies;
    [SerializeField] private int currentNumbersOfBonuses;
    private void Start()
    {
        InitializeWithDefaults();
        if (LoadLevelConfig.Instance != null)
        {
           LoadLevelConfig.Instance.OnDataLoaded += ApplyLoadedData;
            LoadLevelConfig.Instance.OnDataLoadFailed += OnDataLoadFailed;

            if (LoadLevelConfig.Instance.IsDataLoaded)
            {
                ApplyLoadedData(LoadLevelConfig.Instance.CurrentConfig);
            }
        }
        else
        {
            Debug.LogWarning("SaveAndLoadData instance not found!");
        }
    }

    private void InitializeWithDefaults()
    {
        currentNumbersOfBonuses = defaultNumbersOfBonus;
        currentNumbersOfEnemies = defaultNumbersOfEnemy;        ;
        Debug.Log($"Initialized with defaults - Enemies: {currentNumbersOfEnemies}, Bonuses: {currentNumbersOfBonuses}");
    }

    private void ApplyLoadedData(LevelConfig levelConfig)
    {
        currentNumbersOfEnemies = levelConfig.NumbersOfEnemies;
        currentNumbersOfBonuses = levelConfig.NumbersOfBonuses;
        
        Debug.Log($"Data applied: Enemies={currentNumbersOfEnemies}, Bonuses={currentNumbersOfBonuses}");
    }

    private void OnDestroy()
    {
        if (LoadLevelConfig.Instance != null)
        {
            LoadLevelConfig.Instance.OnDataLoaded -= ApplyLoadedData;
            LoadLevelConfig.Instance.OnDataLoadFailed -= OnDataLoadFailed;
        }
    }

    private void OnDataLoadFailed(string error)
    {
        Debug.LogWarning($"Failed to load level config: {error}. Using default values.");
    }
}
