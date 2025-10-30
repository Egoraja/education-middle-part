using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDefaultSettings", menuName = "Game Data/PlayerDefault Settings")]

public class PlayerDefaultSettings : ScriptableObject
{
    [Header("Default Values")]
    [SerializeField] public int DefaultLevel = 1;
    [SerializeField] public float DefaultScore = 0f;
    [SerializeField] public float DefaultScoreToNextLevel = 10f;
    [SerializeField] public float DefaultSpeed = 1f;
    [SerializeField] public float DefaultHealth = 10f;   
}

  
