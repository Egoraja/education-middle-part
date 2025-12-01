using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMovement : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{    
    [SerializeField, Range(1f, 5f)] private float rotationSpeed;
    private Vector3 currentPosition;

    private void Start()
    {       
       currentPosition = transform.position;      
    }

    private void Movement()
    {
        transform.Rotate(Vector3.up * 45 * rotationSpeed * Time.deltaTime);
    }   

    private void FixedUpdate()
    {
       
        if (PhotonNetwork.IsMasterClient)
        {
            Movement();
        }        
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        //////----------------------------------------------------------------////////
        object[] instantiationData = info.photonView.InstantiationData;
        if (instantiationData != null && instantiationData.Length > 0)
        {
           currentPosition = (Vector3)instantiationData[0];
        }

        if (PhotonNetwork.IsMasterClient == false)
        {            
            enabled = false;
        }
    }
}
