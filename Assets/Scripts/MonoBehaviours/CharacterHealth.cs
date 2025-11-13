using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour, IReceiving
{
    [SerializeField] private float health = 1;
    private float maxHealth;
    private bool isAlive = true;
    private bool hasShield = false;
    public bool IsAlive { get { return isAlive; } }

    public void SetConfiguration(float health)
    {       
        this.health = health;
        maxHealth = health;       
    }

    public void ApplyHealing(float healing)
    {        
        if (health > 0)
        {
            if (health + healing > maxHealth)
                health = healing;
            else health += healing;
        }
    }

    public void AddShield()
    {
        if (hasShield == true)
            return;
        hasShield = true;
        Debug.Log("shield added");
    }

    public void ApplyDamage(float damage)
    {
        if (hasShield)
        {
            hasShield = false;
            return;
        }
        else if (health > 0)
        {
            health -= damage;
            CheckIsAlive();
        }    
    }
    public void KillCharacter()
    {
        health = 0;
        isAlive = false;    
    }

    private void CheckIsAlive()
    {
        isAlive = health > 0 ? true : false;    
    }

    private void UpdateUIInfo()
    {

    }
}
