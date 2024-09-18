using System;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    private CellsGrid grid;
    private BuildingFactory buildingFacrory;

    public Action<CellPlacePosition> BuildingSpawned;

    public void Initialize(CellsGrid grid, UpgradeBuildings upgradeBuildings)
    {
        buildingFacrory = new BuildingFactory(upgradeBuildings);
        this.grid = grid;
        SetupStartBuildings();
    }

    public void SetupStartBuildings()
    {
        SetupBuildingPosition(SpawnFarm(), grid.GetBestCellPlace());
        SetupBuildingPosition(SpawnFactory(), grid.GetBestCellPlace());
        SetupBuildingPosition(SpawnSpiritBuilding(), grid.GetBestCellPlace());
        SetupBuildingPosition(SpawnMilitaryCamp(), grid.GetBestCellPlace());
    }

    private Farm SpawnFarm() => buildingFacrory.SpawnBuilding<Farm>();

    private SpiritBuilding SpawnSpiritBuilding() => buildingFacrory.SpawnBuilding<SpiritBuilding>();

    private Factory SpawnFactory() => buildingFacrory.SpawnBuilding<Factory>();

    private MilitaryCamp SpawnMilitaryCamp() => buildingFacrory.SpawnBuilding<MilitaryCamp>();

    private void SetupBuildingPosition(Building building, CellPlacePosition placePosition) 
    {
        building.transform.position = grid.GetWorldPositionByCoordinates(placePosition.X, placePosition.Z) + building.Offset;
        grid.Place(placePosition);
        BuildingSpawned?.Invoke(placePosition);
    }
}