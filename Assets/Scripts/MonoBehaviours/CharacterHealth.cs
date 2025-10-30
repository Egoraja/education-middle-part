using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour, IReceiving
{
    private float health = 1;

    public void SetConfiguration(float health)
    {       
        this.health = health;       
    }
}
