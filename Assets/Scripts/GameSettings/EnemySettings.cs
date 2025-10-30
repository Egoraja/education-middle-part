using UnityEngine;

[CreateAssetMenu (fileName = "EnemySettings", menuName = "Game Data/Enemy Settings")]

public class EnemySettings : ScriptableObject
{
    [Header("�������� ����������")]
    [SerializeField, Min(1)] public float EnemyHealth = 100;

    [Header("������ ������")]
    [SerializeField] public bool IsFlying;

    [Header("�������� ����������")]
    [SerializeField] public string EnemyItem;
}