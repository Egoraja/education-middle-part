using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Transform inventory;
    private Transform[] inventorySlots;    

    private void Start()
    {       
        SetInventory();           
    }

    public Transform GetPosition()
    {       
        Transform temp = null;
        for (int i = 0; i < inventorySlots.Length; i++)
        {           
            if (inventorySlots[i].childCount == 0)
            {
                temp = inventorySlots[i];
                break;
            }           
        }
        return temp;
    }

    public void AddNewItem(GameObject uIItem, bool isReadyToCraft, GameObject target)
    {        
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            GameObject newItem;
            if (inventorySlots[i].childCount == 0)
            {
                if (isReadyToCraft == true)
                    newItem = uIItem;                                  
                else                     
                    newItem = Instantiate(uIItem, inventorySlots[i]);                    
                
                newItem.TryGetComponent(out IItemAbility itemAbility);
                itemAbility.AddTarget(target);
                itemAbility.IsReadyToCraft = isReadyToCraft;
                break;
            }
        }    
    }

    private void SetInventory()
    {
        inventorySlots = new Transform[inventory.childCount];
        int temp = 0;
        foreach (Transform t in inventory)
        {
            inventorySlots[temp] = t;
            temp++;
        }
    }   
}
