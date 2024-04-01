using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class MyNetworkPlayer : NetworkBehaviour
{
    private PlayerController player;

    [SerializeField] private TMP_Text displayNameText = null;

    [SyncVar]
    [SerializeField]
    private string displayName = "Missing Name";

    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }


}
