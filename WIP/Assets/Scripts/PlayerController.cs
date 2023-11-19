using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{

    private NavMeshAgent agent;

    private bool isMoving = false;
    private bool isMovingToResource = false;
    private Transform targetResource;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            agent.speed = 3.5f;
            

            if (Physics.Raycast(ray, out hit))
            {
                

                if (hit.collider.CompareTag("Resource"))
                {
                    ResourceManager resource = hit.collider.GetComponent<ResourceManager>();
                    targetResource = hit.transform;
                    
                    
                    if (resource != null)
                    {
                        MoveToResource(targetResource);
                        isMovingToResource = true;
                        Debug.Log("Moving to resource");
                    }
                }
                else
                {
                    // Move the player to the clicked position
                    MovePlayer(hit.point);
                    isMovingToResource = false;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("In Resource Area");
        
        if (other.CompareTag("Resource") && (isMovingToResource))
        {
            agent.speed = 0f;
            ResourceManager resource = other.GetComponent<ResourceManager>();

            if (resource.resourceCount > 0)
            {
                // Start gathering animation and process
                resource.StartGathering(transform);
            }
        }
    }
    



    
    private void MoveToResource(Transform resource)
    {
        agent.SetDestination(resource.position);
    }
    void MovePlayer(Vector3 destination)
    {
        agent.SetDestination(destination);
        isMoving = true;
    }

}