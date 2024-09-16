using System.Collections.Generic;

public class BuildingFacrory
{
    private UpgradeBuildings upgradeBuildings;
    private List<Building> buildingsList = new List<Building>();

    public BuildingFacrory(UpgradeBuildings buildings)
    {
        buildingsList = buildings.buildings;
    }

    public T GetBuilding<T>() where T : Building
    {
        for (int i = 0; i < buildingsList.Count; i++)
        {
            if (buildingsList[i] is T)
            {
                return buildingsList[i] as T;
            }
        }

        throw new System.ArgumentException("Invalid type of building");
    }
}