using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private int connectionTimeout = 30;

    private void Start()
    {      
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void  OnConnectedToMaster()
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
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " rooms name");
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount + " number of players");
        
    }
}
