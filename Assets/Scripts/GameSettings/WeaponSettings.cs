using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSettings", menuName = "Game Data/Weapon Settings")]

public class WeaponSettings : ScriptableObject
{
    [Header("���� �������� ������")]
    [SerializeField] public float DamageValueMelee = 10f;

    [Header("���� ����������� ������")]
    [SerializeField] public float DamageValueDistance = 5f;    
}
