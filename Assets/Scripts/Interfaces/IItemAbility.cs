using System.Collections.Generic;
using UnityEngine;

internal interface IItemAbility
{
    void AddTarget(GameObject targetm);

    List<GameObject> Targets { get; set; }

    void UseItemFromInventory();

    bool IsReadyToCraft { get; set; }
}