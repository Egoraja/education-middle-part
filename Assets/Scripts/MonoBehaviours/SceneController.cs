using Cinemachine;
using System;
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
    [SerializeField] private PlayerProgressManager progressManager;
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook;
    private float current_Y_AxisValue, current_X_AxisValue;

    private bool isInventoryOpened;

    private void Start()
    {
        current_X_AxisValue = cinemachineFreeLook.m_XAxis.m_MaxSpeed; 
        current_Y_AxisValue = cinemachineFreeLook.m_YAxis.m_MaxSpeed; 
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
        if (progressManager.IsAlive == false)                   
            return;
        
        if (isInventoryOpened == false)
        {
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = 0;
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = 0;
            progressManager.IsMoving = false;
            inventoryPanel.SetActive(true);
            craftButton.SetActive(true);
            isInventoryOpened = true;
        }
        else
        {
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = current_Y_AxisValue;
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = current_X_AxisValue;
            cinemachineFreeLook.enabled = true;
            progressManager.IsMoving = true;
            inventoryPanel.SetActive(false);           
            craftButton.SetActive(false);
            isInventoryOpened = false;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            InventoryButtonPressed();

        if (Input.GetKeyDown(KeyCode.R))
            RestartLevel();            
    }
}
