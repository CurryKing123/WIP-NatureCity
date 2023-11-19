using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollection : MonoBehaviour
{
    public TextAsset textJSON;

    [System.Serializable]

    public class Resource
    {
        public int resourcesid;
        public string resourceName;
        public int resourceAmount;
        public float gatheringTime;
        
        public int lightCost;
        public string resourceType;
    }

    [System.Serializable]
    public class ResourceList
    {
        public Resource[] resources;
    }

    public ResourceList myResourceList = new ResourceList();

    void Start()
    {
        myResourceList = JsonUtility.FromJson<ResourceList>(textJSON.text);
    }
}
