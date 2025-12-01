using Photon.Pun;
using System;
using UnityEngine;

public class DamageDealer : MonoBehaviourPunCallbacks
{
    [SerializeField] private float damageValue = 10f;
    private int shooterID;

    public int ShooterID
    { set { shooterID = value; } }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerProgressManager playerProgress) && collision.gameObject.TryGetComponent(out PhotonView photonViewTarget ))
        {                     
            photonViewTarget.RPC("ApplyDamage", RpcTarget.All, damageValue, shooterID);
        }
        photonView.RPC("DestroyBullet", RpcTarget.AllBuffered);        
    }

    [PunRPC]
    private void DestroyBullet()
    {
        if (photonView.IsMine)        
            PhotonNetwork.Destroy(gameObject);        
        else
            gameObject.SetActive(false);        
    }
}
