using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelUpHealth : MonoBehaviour, ILevelUp
{
    private float[] healthStages;
    [SerializeField] private CharacterHealth characterHealth;

    public void LevelUp(int level, int maxLevel)
    {
        if (healthStages == null)
        {
            SetHealthStages(maxLevel);
        }
        characterHealth.SetConfiguration(healthStages[level - 1]);        
    }

    public void SetCongiguration(int level, int maxLevel)
    {
        if (healthStages == null)
        {
            SetHealthStages(maxLevel);
        }
        characterHealth.SetConfiguration(healthStages[level - 1]);
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
