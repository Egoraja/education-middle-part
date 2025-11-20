using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAIDAbility : MonoBehaviour, IItemAbility, ICraftAble
{
    [SerializeField] private float healtUpVolume = 10;
    [SerializeField] private string nameability = "Health";
    public string Name => nameability;
    private bool isReadyToCraft = false;
    private List<GameObject> targets = new List<GameObject>();

    public bool IsReadyToCraft
    {
        get { return isReadyToCraft; }
        set { isReadyToCraft = value; }
    }

    public List<GameObject> Targets
    {
        get { return targets; }
        set { targets = value; }
    }   

    public void AddTarget(GameObject target)
    {
        targets.Add(target);       
    }  
    
    public void UseItemFromInventory()
    {
        if (isReadyToCraft == false)
        {
            foreach (GameObject target in targets)
            {
                if (target.TryGetComponent(out PlayerProgressManager playerProgressManager))
                { 
                    if (playerProgressManager.IsAlive)                    
                        playerProgressManager.AddHealth(healtUpVolume);                    
                }
            }
            Destroy(gameObject, 0.01f);
        }
    }    
}

