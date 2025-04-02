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
        SpawnBuilding<Farm>();
        SpawnBuilding<SpiritBuilding>();
        SpawnBuilding<Factory>();
        var camp = SpawnBuilding<MilitaryCamp>();
        camp.SetupFactory(detector);
    }

    private void ActivateUpgrades(BasicBuildingData.Upgrades firstUpgrade, BasicBuildingData.Upgrades secondUpgrade)
    {
        ActivateUpgrade(firstUpgrade);
        ActivateUpgrade(secondUpgrade);
    }

    private void ActivateUpgrade(BasicBuildingData.Upgrades upgrade)
    {
        SpawnBuilding(upgrade);
        switch (upgrade)
        {
            case BasicBuildingData.Upgrades.Farm:
                SpawnBuilding();
                break;
            case BasicBuildingData.Upgrades.SpiritBuilding:
                SpawnBuilding();
                break;
            case BasicBuildingData.Upgrades.MilitaryCamp:
                var camp = SpawnBuilding();
                camp.SetupFactory(detector);
                break;
            case BasicBuildingData.Upgrades.ResourcesCenter:
                SpawnBuilding<Factory>();
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private Building SpawnBuilding(BasicBuildingData.Upgrades upgradeType)
    {
        var building =  buildingFacrory.SpawnBuilding(upgradeType, SetupBuildingOnGrid(grid.GetBestCellCoordinates()));
        building.SetupFacroty(detector);
        return building;
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