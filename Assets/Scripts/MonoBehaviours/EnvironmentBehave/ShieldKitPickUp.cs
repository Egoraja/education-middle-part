using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldKitPickUp : MonoBehaviour, IPickUpEffect, IItem
{
    [SerializeField] private GameObject uIShieldKitObject; 

    public GameObject UIItem => uIShieldKitObject;

    public void ApplyEffect(PlayerProgressManager playerProgressManager)
    {
        playerProgressManager.AddNewItem(UIItem);
    }
}
