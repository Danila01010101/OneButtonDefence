using System;
using UnityEngine;

[Serializable]
public abstract class BasicBuildingData 
{
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public Building Prefab { get; private set; }
    [field: SerializeField] public Vector3 SpawnOffset { get; private set; }
    [field: SerializeField] public Vector3 SpawnRotation { get; private set; }
    [field: SerializeField] public ResourceChangeData[] buildResourceChange { get; private set; }
    [field: SerializeField] public ResourceChangeData[] resourcePerTurnChange { get; private set; }

    [System.Serializable]
    public class ResourceChangeData
    {
        public ResourceAmount resourceAmount;
        public string resourceDescription;
    }
}