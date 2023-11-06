using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(-3, 3, -3);
    }
}
