using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviourPunCallbacks, IPickUpAble
{
    private Transform targetTransform;
    private bool isCollected = false;    
    private GameObject localPlayer;

    [PunRPC]
    public void Execute()
    {
        isCollected = true;

        if (localPlayer != null && localPlayer.TryGetComponent(out PlayerProgressManager playerProgress))
        {
            if (TryGetComponent(out IPickUpEffect pickUpEffect))
                pickUpEffect.ApplyEffect(playerProgress);
        }

        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(gameObject);
        else                    
            gameObject.SetActive(false);        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;
      
        if (other.TryGetComponent(out PlayerProgressManager playerProgressManager) && other.GetComponent<PhotonView>().IsMine)
        {
            localPlayer = other.gameObject;
            targetTransform = other.transform;                      
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerProgressManager playerProgressManager) && other.GetComponent<PhotonView>().IsMine)
        {
            localPlayer = null;
            targetTransform = null;            
        }
    }  

    private void Update()
    {
        if (targetTransform != null)
        {            
            float distance = Vector3.Distance(transform.position, targetTransform.position);
            if (distance < 1.5f)
            {
                if (photonView != null)
                    photonView.RPC("Execute", RpcTarget.AllBuffered);
                targetTransform = null;
            }           
        }
    }
}
