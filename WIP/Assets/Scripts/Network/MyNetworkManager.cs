using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class MyNetworkManager : NetworkManager
{
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

        //netPlayer.SetDisplayName(playerName);
        
        Debug.Log($"{numPlayers} Players in server");
    }


}
