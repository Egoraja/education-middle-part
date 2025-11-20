using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPickUp : MonoBehaviour, IPickUpEffect
{
    [SerializeField] private float damage;
    public void ApplyEffect(PlayerProgressManager playerProgressManager)
    {
        playerProgressManager.ApplyDamage(damage);
    }
}
