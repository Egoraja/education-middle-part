using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForUIEnemy : MonoBehaviour
{
    private float currentHealth = 100;
    private float maxHealth = 100;
    private int currentLevel = 2;


    public void ApplyDamage(float damageValue)
    {
        currentHealth -= damageValue;    
    }

    public SharingPlayerInfo GetInfo()
    {
        SharingPlayerInfo playerInfo = new SharingPlayerInfo
        {
            CurrentLevel = this.currentLevel,
            Health = this.currentHealth,
            MaxHealth = this.maxHealth
        };
        return playerInfo;
    }
}
