using Photon.Pun;
using UnityEngine;

public class TrapActivator : MonoBehaviourPunCallbacks
{
    [SerializeField] private float timer = 2f;   

    private GameObject trapObject;
    private GameObject localPlayer;
    private bool isCollected = false;
    
    public GameObject TrapObject
    { set { trapObject = value; } }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected == true || trapObject == null)
        {
            Debug.Log("return");
            return;
        }
        if (other.transform.TryGetComponent(out TrapInteraction trapInteraction) && other.GetComponent<PhotonView>().IsMine)
        {            
            localPlayer = other.gameObject;
            if (photonView != null)            
                photonView.RPC("StartInteraction", RpcTarget.All);            
        }
    }

    [PunRPC]
    private void StartInteraction()
    {
        if (localPlayer != null && localPlayer.TryGetComponent(out TrapInteraction trapInteraction) && trapObject != null)
        {
            isCollected = true;
            trapInteraction.StartFirstPartTrapInteraction(trapObject.transform, timer);
            localPlayer = null;
        }
    }    
}
