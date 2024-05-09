using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class MyNetworkManager : NetworkManager
{
    [SerializeField] private GameObject player;
    private string playerName;


    public override void OnClientConnect()
    {
        base.OnClientConnect();

        Debug.Log("Connected to server!");
    }


    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        MyNetworkPlayer netPlayer = conn.identity.GetComponent<MyNetworkPlayer>();

        WaitForName(conn);
        
        Debug.Log($"{numPlayers} Players in server");
    }

    async void WaitForName(NetworkConnectionToClient conn)
    {
        MyNetworkPlayer netPlayer = conn.identity.GetComponent<MyNetworkPlayer>();
        await Task.Delay(1000);
        player = GameObject.Find("Player");
        playerName = player.GetComponent<PlayerController>().userName;
        netPlayer.SetDisplayName(playerName);
    }
}
