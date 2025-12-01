using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject trapObjectPrefab;
    [SerializeField] private TrapActivator trapActivator;    
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform[] pathTransforms;    

    public void SpawnTrap()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject trapObject = PhotonNetwork.Instantiate(trapObjectPrefab.name, spawnPoint.position, Quaternion.identity);
            trapObject.TryGetComponent(out DeathTrapMover deathTrapMover);
            deathTrapMover.enabled = true;
            deathTrapMover.PathTransforms = pathTransforms;
            trapActivator.TrapObject = trapObject;                 
            photonView.RPC("SyncTrapObject", RpcTarget.AllBuffered, trapObject.GetPhotonView().ViewID);
        }       
    }

    [PunRPC]
    private void SyncTrapObject(int trapViewID)
    {        
        PhotonView trapView = PhotonView.Find(trapViewID);       

        if (trapView != null)
        {
            GameObject trapObject = trapView.gameObject;
            if (trapObject != null)
            {
            trapActivator.TrapObject = trapObject;
            }
        }
    }
}

