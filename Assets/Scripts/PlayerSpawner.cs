using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;


public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private CharacterDataSaver characterDataSaver;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject bullet;
    [SerializeField] private CinemachineFreeLook freeLookCamera;

    public void SpawnPlayer(int playerID)
    {
        if (playerID > spawnPoints.Count)
            Debug.Log("No spawn points");
        else
        {
            GameObject player = PhotonNetwork.Instantiate(playerObject.name, spawnPoints[playerID-1].position, Quaternion.identity);
            if (player.TryGetComponent(out PlayerProgressManager playerProgress) && player.TryGetComponent(out ShootingAbility shootingAbility)
                && player.TryGetComponent(out PlayerController playerController) && player.GetPhotonView().IsMine)
            {
                playerProgress.enabled = true;
                shootingAbility.enabled = true;
                playerProgress.InventoryManager = inventoryManager;
                characterDataSaver.LoadPlayerDataStart(playerProgress);
                freeLookCamera.Follow = playerProgress.CamAncor;
                freeLookCamera.LookAt = playerProgress.CamAncor;
                playerController.ThirdPersonCamera = mainCamera.transform;
                sceneController.AddSettings(playerProgress, freeLookCamera);
                shootingAbility.Bullet = bullet;
            }
        }
    }
}
