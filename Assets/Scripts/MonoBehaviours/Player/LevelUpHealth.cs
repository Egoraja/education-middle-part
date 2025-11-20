using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelUpHealth : MonoBehaviour, ILevelUp
{
    private float[] healthStages;
    private CharacterHealth characterHealth;
    private int currentLevel;

    private void Start()
    {
        characterHealth = GetComponent<CharacterHealth>();       
    }

    public void LevelUp(int level, int maxLevel)
    {
        if (healthStages == null)        
            SetHealthStages(maxLevel);
        currentLevel = level;
        characterHealth.SetConfiguration(healthStages[level - 1], currentLevel);        
    }

    public void SetConfiguration(int level, int maxLevel)
    {     
        if (healthStages == null)        
            SetHealthStages(maxLevel);
        currentLevel = level;
        characterHealth.SetConfiguration(healthStages[level - 1], currentLevel);
    }

    private void SetHealthStages(int maxLevel)
    {
        healthStages = new float[maxLevel];
        for (int i = 0; i < healthStages.Length; i++)
        {
            healthStages[i] = 100 + i * 10;
        }
    }
}
