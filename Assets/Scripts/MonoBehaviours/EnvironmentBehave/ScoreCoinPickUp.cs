using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUpCoin : MonoBehaviour, IPickUpEffect
{
    [SerializeField] private float score = 10f;    

    public void ApplyEffect(PlayerProgressManager playerProgressManager)
    {
        playerProgressManager.AddNewScore(score);
        playerProgressManager.PrintInfo();
    }
}
