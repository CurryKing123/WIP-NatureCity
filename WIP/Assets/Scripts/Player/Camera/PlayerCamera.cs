using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class PlayerCamera : NetworkBehaviour
{
    public Transform player;
    public GameObject cameraHolder;
    public Vector3 offset;

    public override void OnStartAuthority()
    {
        cameraHolder.SetActive(true);
    }


    void Update()
    {
        if (isLocalPlayer)
        {
            cameraHolder.transform.position = player.transform.position + offset;
        }
    }
}
