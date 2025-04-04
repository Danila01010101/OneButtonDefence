using System;
using UnityEngine;

[System.Serializable]
public class BasicBuildingData 
{
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public Upgrades UpgradeType { get; private set; }
    [field: SerializeField] public Building Prefab { get; private set; }
    [field: SerializeField] public Vector3 SpawnOffset { get; private set; }
    [field: SerializeField] public Vector3 SpawnRotation { get; private set; }
    [field: SerializeField] public string BuildingLore { get; private set; }
    [field: SerializeField] public ResourceChangeInfo[] buildResourceChange { get; private set; }
    [field: SerializeField] public ResourceChangeInfo[] resourcePerTurnChange { get; private set; }

    [System.Serializable]
    public class ResourceChangeInfo
    {
        [field: SerializeField] public StartResourceAmount ResourceAmount { get; private set; }
        [field: SerializeField] public string ResourceDescription { get; private set; }
    }

    public enum Upgrades { Farm = 0, SpiritBuilding = 1, MilitaryCamp = 2, Factory = 3 }
}