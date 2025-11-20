using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Unity.VisualScripting;

public class CraftController : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private Transform inventory;
    [SerializeField] private CraftSettings craftSettings;
    [SerializeField] private Button craftButton;
    [SerializeField] private Text craftButtonText;
    [SerializeField] private string craftButtonText1 = "Craft";
    [SerializeField] private string craftButtonText2 = "Crafting";
    private List<GameObject> targets = new List<GameObject>();
    private Image craftButtonImage;
    private List<Button> buttons = new List<Button>();
    private List<GameObject> selected = new List<GameObject>();
    private List<GameObject> orderToInstantiate;
    private bool isCrafting = false;
    private float delay = 0;

    public void EnterCraftMode()
    {
        craftButtonImage = craftButton.GetComponent<Image>();
        if (isCrafting == false)
        {
            buttons = FindCraftAbleObjects();
            if (buttons.Count == 0)
                return;

            craftButtonText.text = craftButtonText2;
            craftButtonImage.color = Color.red;
            isCrafting = true;
            selected.Clear();
            foreach (var item in buttons)
            {
                item.onClick.AddListener(() => Select(item.gameObject));
                item.GetComponent<ICraftAble>().IsReadyToCraft = true;
            }
        }

        else
        {
            craftButtonText.text = craftButtonText1;
            craftButtonImage.color = new Color(1, 1, 1, 1);
            isCrafting = false;
            selected.Clear();
            buttons = FindCraftAbleObjects();

            foreach (var item in buttons)
                item.onClick.RemoveAllListeners();
            CraftCanceled(buttons);
        }       
    }
    public void Select(GameObject gameObject)
    {       
        if (selected.Contains(gameObject))
        {
            selected.Remove(gameObject);
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        else
        {
            selected.Add(gameObject);
            gameObject.GetComponent<Image>().color = new Color(1, 0.5f, 0.5f, 0.7f);
        }
        CheckCombination();
    }

    private List<Button> FindCraftAbleObjects()
    {
        List<Button> currentList = new List<Button>();
        foreach (Transform item in inventory)
        {
            if (item.childCount > 0)
            {
                var temp = item.GetChild(0);

                if (temp.TryGetComponent(out ICraftAble craftAble) && temp.GetComponent<Button>())                                    
                    currentList.Add(temp.GetComponent<Button>());                
            }
        }
        return currentList;
    }
    
    private void CraftCanceled(List<Button> buttons)
    {
        foreach (var button in buttons)
        {
            if(button.gameObject.TryGetComponent(out ICraftAble craftAble))
                craftAble.IsReadyToCraft = false;           
            button.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        buttons.Clear();
    }

    private void CheckCombination()
    {
        List<string> selectedNames = new List<string>();       
        List<GameObject> orderToDel = new List<GameObject>();
        orderToInstantiate = new List<GameObject>();

        foreach (var name in selected)        
            selectedNames.Add(name.GetComponent<ICraftAble>().Name);        
        selectedNames.Sort();

        foreach (var combination in craftSettings.CraftCombinations)
        {
            combination.Sources.Sort();
            if (combination.Sources.SequenceEqual(selectedNames))
            {
                targets.Clear();
                orderToDel.AddRange(selected);            
                orderToInstantiate.AddRange(combination.Results);
                selected.Clear();
                selectedNames.Clear();                   
            }
        }
        
        foreach (var item in orderToDel)
        {
            if (targets.Count == 0)
                targets = item.GetComponent<IItemAbility>().Targets;
            Destroy(item);
        }
        if (orderToInstantiate.Count > 0)
            delay = 0.1f;     
    }

    private void UpdateInventory(List<GameObject> orderToInstantiate)
    {        
        if (orderToInstantiate.Count == 0)
            return;
        else
        {            
            foreach (var item in orderToInstantiate)
            {
                Transform position = inventoryManager.GetPosition();
                Debug.Log(position == null);
                if (position != null)
                {
                    GameObject newItem = Instantiate(item, inventoryManager.GetPosition());
                    newItem.GetComponent<IItemAbility>().Targets.AddRange(targets);
                }
            }            
        }
        foreach (var item in buttons)
            item.onClick.RemoveAllListeners();
        isCrafting = false;         
        EnterCraftMode();    
    }

    private void Update()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
            if (delay < 0)
            {
                UpdateInventory(orderToInstantiate);
                delay = 0;            
            }        
        }
    }
}
