using System;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour, ICellPlacer
{
    private CellsGrid grid;
    private BuildingFactory buildingFacrory;

    public Action<CellPlaceCoordinates> CellFilled { get; set; }

    public void Initialize(CellsGrid grid, BuildingsData upgradeBuildings, float animationDuration)
    {
        buildingFacrory = new BuildingFactory(upgradeBuildings, animationDuration);
        this.grid = grid;
    }

    private void SetupStartBuildings()
    {
        SpawnBuilding(BasicBuildingData.Upgrades.Farm);
        SpawnBuilding(BasicBuildingData.Upgrades.SpiritBuilding);
        SpawnBuilding(BasicBuildingData.Upgrades.Factory);
        SpawnBuilding(BasicBuildingData.Upgrades.MilitaryCamp);
    }

    private void ActivateUpgrades(BasicBuildingData.Upgrades firstUpgrade)
    {
        ActivateUpgrade(firstUpgrade);
    }

    private void ActivateUpgrade(BasicBuildingData.Upgrades upgrade) => SpawnBuilding(upgrade);

    private void SpawnBuilding(BasicBuildingData.Upgrades upgradeType)
    {
        var building =  buildingFacrory.SpawnBuilding(upgradeType, SetupBuildingOnGrid(grid.GetBestCellCoordinates()));
    }

    private Vector3 SetupBuildingOnGrid(CellPlaceCoordinates placePosition) 
    {
        var position = grid.GetWorldPositionByCoordinates(placePosition.X, placePosition.Z);
        grid.Place(placePosition);
        CellFilled?.Invoke(placePosition);
        return position;
    }

    private void OnEnable()
    {
        GameInitializer.GameInitialized += SetupStartBuildings;
        UpgradeButton.UpgradeTypesChoosen += ActivateUpgrades;
    }

    private void OnDisable()
    {
        GameInitializer.GameInitialized -= SetupStartBuildings;
        UpgradeButton.UpgradeTypesChoosen -= ActivateUpgrades;
    }
}