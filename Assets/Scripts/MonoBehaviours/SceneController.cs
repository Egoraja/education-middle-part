using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField] private Button inventoryButton;
    [SerializeField] private GameObject craftButton;
    [SerializeField] private GameObject inventoryPanel;

    private bool isInventoryOpened;

    private void Start()
    {      
        isInventoryOpened = false;
        inventoryPanel.SetActive(false);
        craftButton.SetActive(false);
    }
    public void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void InventoryButtonPressed()
    {
        if (isInventoryOpened == false)
        {
            inventoryPanel.SetActive(true);
            craftButton.SetActive(true);
            isInventoryOpened = true;
        }
        else
        {
            inventoryPanel.SetActive(false);            
            craftButton.SetActive(false);
            isInventoryOpened = false;
        }
    }
}
