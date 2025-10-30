using UnityEngine;

[CreateAssetMenu (fileName = "EnemySettings", menuName = "Game Data/Enemy Settings")]

public class EnemySettings : ScriptableObject
{
    [Header("Здоровье противника")]
    [SerializeField, Min(1)] public float EnemyHealth = 100;

    [Header("Умение летать")]
    [SerializeField] public bool IsFlying;

    [Header("Предметы противника")]
    [SerializeField] public string EnemyItem;
}