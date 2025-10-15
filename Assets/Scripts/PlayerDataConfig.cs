using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDataConfig
{
    public float speed;
    public float health;

    public PlayerDataConfig(float speed, float health)
    {
        this.speed = speed;
        this.health = health;
    }
}

