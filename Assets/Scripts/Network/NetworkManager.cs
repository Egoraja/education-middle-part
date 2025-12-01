using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private bool useBestRegion = true;
    [SerializeField] private int connectionTimeout = 30;
    [Space(5)]
    [Header("PlayerSettings")]
    [SerializeField] private PlayerSpawner playerSpawner;
    [Space(5)]
    [Header("DeathTrapSettings")]    
    [SerializeField] private TrapSpawner trapSpawner;

    private void Start()
    {
        ConfigurePhotonForStableConnection();
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 3,            
            IsVisible = false
        };
        PhotonNetwork.JoinOrCreateRoom("MainRoom", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        var playerID = PhotonNetwork.LocalPlayer.ActorNumber;
        Debug.Log("Joined");
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " rooms name");
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount + " number of players");
        playerSpawner.SpawnPlayer(playerID);
        trapSpawner.SpawnTrap();        
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"Disconnected: {cause}");

        if (cause == DisconnectCause.ClientTimeout)
        {
            StartCoroutine(ReconnectAfterDelay(3f));
        }
    }

    private IEnumerator ReconnectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.ConnectUsingSettings();
    }



    public void SayHelloButton()
    {
        this.photonView.RPC("SendHello", RpcTarget.All, (byte)PhotonNetwork.LocalPlayer.ActorNumber);

    }

    [PunRPC]
    public void SendHello(byte playerID)
    {
        Debug.Log($"player {playerID} said {"Hello"}");
    }

    private void OnRegionsPinged(RegionHandler regionHandler)
    {
        Debug.Log($"Best region selected: {regionHandler.BestRegion.Code} with ping: {regionHandler.BestRegion.Ping}ms");
    }

    private void ConfigurePhotonForStableConnection()
    {       
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.EnableCloseConnection = true;
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 30000;
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.TimePingInterval = 3000;
    }
}
