using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [field: SerializeField] public SerializableDictionary<ResourceDescription, int> Resources{get; private set;}

    public int GetResourceCount(ResourceDescription type)
    {
        if(Resources.TryGetValue(type, out int currentCount))
        {
            return currentCount;
        }
        else
        {
            return 0;
        }
    }

    public int AddResources(ResourceDescription type, int count)
    {
        if (Resources.TryGetValue(type, out int currentCount))
        {
            return Resources[type] += count;
        }
        else 
        {
            Resources.Add(type, count);
            return count;
        }
    }
}
