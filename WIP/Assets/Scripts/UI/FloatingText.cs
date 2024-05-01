using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public GameObject nameText;
    public Transform player;
    public Vector3 offset;
    private Vector3 rotOffset = new Vector3(0, 180, 0);
    public Camera cam;


    void Update()
    {
        if (nameText != null)
        {
            nameText.transform.position = player.position + offset;
        }
    }
}
