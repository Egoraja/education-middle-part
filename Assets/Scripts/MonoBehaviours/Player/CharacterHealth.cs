using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour, IReceiving
{
    [SerializeField] private PlayerProgressManager progressManager;       
    [SerializeField] private float health = 1;
    private float maxHealth;   
    private bool isAlive = true;
    private bool hasShield = false;   
   
    public float Health
    { get { return health; } }

    public float MaxHealth 
        { get { return maxHealth; } }

    
   
    public void SetConfiguration(float health, int currentlevel)
    {                
        this.health = health;
        maxHealth = health;         
    }

    public void ApplyHealing(float healing)
    {        
        if (health > 0)
        {
            if (health + healing > maxHealth)
                health = maxHealth;
            else health += healing;
        }       
    }

    public void AddShield()
    {
        if (hasShield == true)
            return;
        hasShield = true;       
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
        CheckIsAlive();       
    }   

    private void CheckIsAlive()
    {
        isAlive = health > 0;
        health = isAlive ? health : 0;       
        if (isAlive == false)
            progressManager.PlayerIsDead();
    }   
}
