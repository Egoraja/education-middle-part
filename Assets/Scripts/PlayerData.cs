using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class PlayerData
{      
    [SerializeField] private float score;
    [SerializeField] private int level;
    [SerializeField] private float scoreToNextLevel;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float currentHealth;    
        
    public float Speed { get { return currentSpeed; } set { currentSpeed = value; } }
    public float Health { get { return currentHealth; } set { currentHealth = value; } }
    public float Score { get { return score; } set { score = value; } }
    public float ScoreToNextLevel { get { return scoreToNextLevel; } set { scoreToNextLevel = value; } }
    public int Level { get { return level; } set { level = value; } }
        
    public PlayerData()
    {
    }

    public PlayerData(PlayerDefaultSettings playerDefaultSettings)
    {
        if (playerDefaultSettings == null)
        {
            Debug.LogError("PlayerDefaultSettings is null!");
            return;
        }
        this.score = playerDefaultSettings.DefaultScore;
        this.level = playerDefaultSettings.DefaultLevel;
        this.scoreToNextLevel = playerDefaultSettings.DefaultScoreToNextLevel;
        this.currentSpeed = playerDefaultSettings.DefaultSpeed;
        this.currentHealth = playerDefaultSettings.DefaultHealth;  
        Debug.Log("PlayerData created with default settings");       
    }

    public PlayerData(float score, int currentLevel, float scoreToNextLevel, float currentSpeed, float currentHealth)
    {     
        this.score = score;
        this.level = currentLevel;
        this.scoreToNextLevel = scoreToNextLevel;
        this.currentSpeed = currentSpeed;
        this.currentHealth = currentHealth;         
    }
}

