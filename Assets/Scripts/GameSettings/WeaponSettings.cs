using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSettings", menuName = "Game Data/Weapon Settings")]

public class WeaponSettings : ScriptableObject
{
    [Header("”рон ближнего оружи€")]
    [SerializeField] public float DamageValueMelee = 10f;

    [Header("”рон стрел€ющего оружи€")]
    [SerializeField] public float DamageValueDistance = 5f;    
}
