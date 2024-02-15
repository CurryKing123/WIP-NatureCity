using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ResourceManager : MonoBehaviour
{
    
    
    public int resourceCount = 2;
    public float gatheringTime = 2.0f;
    private bool isBeingGathered = false;

    //public void StartGathering(Transform player)
    //{
    //    
    //    if (!isBeingGathered)
    //    {
    //        isBeingGathered = true;
    //        StartCoroutine(GatherResource(player));
    //    }
    //}

    
    
//Gathering Resources
    //private IEnumerator GatherResource(Transform player)
    //{
    //    ResourceCollection resCol = player.GetComponent<ResourceCollection>();
//
    //    //while(resCol.myResourceList.resources[resourceAmount] > 0 )
    //    while(resourceCount > 0)
    //    {
    //        Debug.Log("Gathering...");
    //    // Play the gathering animation
//
//
    //    yield return new WaitForSeconds(gatheringTime);
    //    resourceCount--;
    //    Inventory.AddResources(ResourceType, 1);
//
    //    if (resourceCount <= 0)
    //    {
    //        // Resource depleted
    //        Destroy(gameObject);
    //        Debug.Log("Gathering Finished");
    //    }
//
    //    isBeingGathered = false;
    //    }
    //}
}