using System.Collections.Generic;
using UnityEngine;

public class BuildingFactory
{
    private List<BasicBuildingData> buildingsList;
    private float animationDuration;

    public BuildingFactory(BuildingsData buildingsData, float animationDuration)
    {
        buildingsList = buildingsData.Buildings;
        this.animationDuration = animationDuration;
    }

    public Building SpawnBuilding(BasicBuildingData.Upgrades upgradeType, Vector3 position)
    {
        foreach (var buildingData in buildingsList)
        {
            if (buildingData.UpgradeType == upgradeType)
            {
                Vector3 randomBuildingRotation = buildingData.SpawnRotations[Random.Range(0, buildingData.SpawnRotations.Count)];
                Building spawnedBuilding = Object.Instantiate(buildingData.Prefab, position, Quaternion.Euler(randomBuildingRotation));
                spawnedBuilding.Initialize(buildingData, position, animationDuration);
                return spawnedBuilding;
            }
        }

        throw new System.ArgumentException($"No building found for UpgradeType {upgradeType}");
    }
}