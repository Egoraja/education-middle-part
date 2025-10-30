using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class LevelConfig 
{
    private int numberOfEnemies;
    private int numberOfBonuses;   

    public int NumbersOfEnemies { get { return numberOfEnemies; } }
    public int NumbersOfBonuses { get { return numberOfBonuses; } }

    public LevelConfig(int numberOfenemies, int numberOfbonuses)
    {
        this.numberOfEnemies = numberOfenemies;
        this.numberOfBonuses = numberOfbonuses;
    }
}
