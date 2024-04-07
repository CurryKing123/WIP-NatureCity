using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class PlayerCamera : NetworkBehaviour
{
    public Transform player;
    public GameObject cameraHolder;

    public override void OnStartAuthority()
    {
        cameraHolder.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(-10, 15, -10);
    }
}
