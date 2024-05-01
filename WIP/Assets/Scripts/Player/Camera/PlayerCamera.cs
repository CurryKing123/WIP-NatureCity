using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using Cinemachine;

public class PlayerCamera : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vCam;
    public Transform player;


    private void Start()
    {
        if (isLocalPlayer)
        {
            vCam = CinemachineVirtualCamera.FindAnyObjectByType<CinemachineVirtualCamera>();
            vCam.LookAt = player.gameObject.transform;
            vCam.Follow = player.gameObject.transform;
        }
    }
}
