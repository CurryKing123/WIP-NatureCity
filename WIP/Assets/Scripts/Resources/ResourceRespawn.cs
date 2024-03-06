using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class ResourceRespawn : MonoBehaviour
{
    public GameObject resourceNode;

    public void RespawnResource(float respawnTime)
    {
        StartCoroutine(RespawnRes(respawnTime));
    }
    public IEnumerator RespawnRes(float respawnTime)
    {
        Debug.Log($"Respawning Resource in {respawnTime} seconds");
        yield return new WaitForSeconds(respawnTime);
        Instantiate(resourceNode, transform.position, Quaternion.identity);
    }
}
