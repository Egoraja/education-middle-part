using UnityEngine;

internal interface IItemAbility
{
    void AddTarget(GameObject targetm);

    void UseItemFromInventory();

    bool IsReadyToCraft { get; set; }
}