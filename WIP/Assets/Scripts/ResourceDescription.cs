using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceDescription", menuName = "WIP/Resources")]
public class ResourceDescription : ScriptableObject
{
    [field: SerializeField] public string DisplayName { get; private set; }
}
