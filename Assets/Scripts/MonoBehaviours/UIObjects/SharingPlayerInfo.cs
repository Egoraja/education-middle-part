using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharingPlayerInfo 
{
    private float health;
    private float maxHealth;
    private int currentLevel;

    public float Health
    {
        get { return health; }
        set { health = value; }
    }
    public float MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }
    public int CurrentLevel
    {
        get { return currentLevel; }
        set { currentLevel = value; }
    }
}
