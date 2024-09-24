using System.Collections.Generic;
using UnityEngine;

public class BuildingFactory
{
    private BuildingsData upgradeBuildings;
    private List<Building> buildingsList = new List<Building>();

    public BuildingFactory(BuildingsData buildings)
    {
        buildingsList = buildings.buildings;
    }

    public T SpawnBuilding<T>() where T : Building
    {
        for (int i = 0; i < buildingsList.Count; i++)
        {
            Building building = buildingsList[i];

            if (building is T)
            {
                Building spawnedBuilding = MonoBehaviour.Instantiate(building);
                spawnedBuilding.ActivateSpawnAction();
                return spawnedBuilding as T;
            }
        }

        throw new System.ArgumentException("Invalid type of building");
    }
}