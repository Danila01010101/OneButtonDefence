using System;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour, ICellPlacer
{
    private CellsGrid grid;
    private BuildingFactory buildingFacrory;
    private IEnemyDetector detector;

    public Action<CellPlaceCoordinates> CellFilled { get; set; }

    public void Initialize(CellsGrid grid, BuildingsData upgradeBuildings, float animationDuration, IEnemyDetector detector)
    {
        buildingFacrory = new BuildingFactory(upgradeBuildings, animationDuration);
        this.grid = grid;
        this.detector = detector;
        SetupStartBuildings();
    }

    public void SetupStartBuildings()
    {
        SpawnBuilding(BasicBuildingData.Upgrades.Farm);
        SpawnBuilding(BasicBuildingData.Upgrades.SpiritBuilding);
        SpawnBuilding(BasicBuildingData.Upgrades.Factory);
        SpawnBuilding(BasicBuildingData.Upgrades.MilitaryCamp);
    }

    private void ActivateUpgrades(BasicBuildingData.Upgrades firstUpgrade, BasicBuildingData.Upgrades secondUpgrade)
    {
        ActivateUpgrade(firstUpgrade);
        ActivateUpgrade(secondUpgrade);
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
        UpgradeButton.UpgradeTypesChoosen += ActivateUpgrades;
    }

    private void OnDisable()
    {
        UpgradeButton.UpgradeTypesChoosen -= ActivateUpgrades;
    }
}