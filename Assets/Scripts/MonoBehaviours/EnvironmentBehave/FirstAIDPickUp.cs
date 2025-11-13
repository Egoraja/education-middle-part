using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAIDPickUp : MonoBehaviour, IPickUpEffect, IItem
{
    [SerializeField] private GameObject uIFirstAIDObject;      

    public GameObject UIItem => uIFirstAIDObject;

    public void ApplyEffect(PlayerProgressManager playerProgressManager)
    {
        playerProgressManager.AddNewItem(UIItem);
    }
}
