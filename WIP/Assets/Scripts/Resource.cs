using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ResourceType
{
    Stone,
    Wood
}

public class Resource : MonoBehaviour
{
    [field: SerializeField] public string DisplayName {get; private set;}

    
    public int resourceCount = 2;
    public float gatheringTime = 2.0f;
    private bool isBeingGathered = false;

    public void StartGathering(Transform player)
    {
        
        if (!isBeingGathered)
        {
            isBeingGathered = true;
            StartCoroutine(GatherResource(player));
        }
    }
    
    

    private IEnumerator GatherResource(Transform player)
    {
        while(resourceCount >= 0)
        {
            Debug.Log("Gathering...");
        // Play the gathering animation


        yield return new WaitForSeconds(gatheringTime);
        resourceCount--;

        if (resourceCount <= 0)
        {
            // Resource depleted
            Destroy(gameObject);
            Debug.Log("Gathering Finished");
        }

        isBeingGathered = false;
        }
    }
}