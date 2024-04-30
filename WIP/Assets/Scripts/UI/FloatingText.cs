using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public GameObject nameText;
    public Camera cam;


    void Start()
    {

    }

    void Update()
    {
        if (nameText != null)
        {
            nameText.transform.LookAt(cam.transform.position);
            nameText.transform.Rotate(0,180,0);
        }
    }
}
