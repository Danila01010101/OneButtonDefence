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

    private void ActivateUpgrades(UpgradeButton.Upgrades firstUpgrade, UpgradeButton.Upgrades secondUpgrade)
    {
        ActivateUpgrade(firstUpgrade);
        ActivateUpgrade(secondUpgrade);
    }

    private void ActivateUpgrade(UpgradeButton.Upgrades upgrade)
    {
        switch (upgrade)
        {
            case UpgradeButton.Upgrades.Farm:
                SpawnBuilding<Farm>();
                break;
            case UpgradeButton.Upgrades.SpiritBuilding:
                SpawnBuilding<SpiritBuilding>();
                break;
            case UpgradeButton.Upgrades.MilitaryCamp:
                var camp = SpawnBuilding<MilitaryCamp>();
                camp.SetupFactory(detector);
                break;
            case UpgradeButton.Upgrades.ResourcesCenter:
                SpawnBuilding<Factory>();
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private T SpawnBuilding<T>() where T : Building
    {
        T building = buildingFacrory.SpawnBuilding<T>();
        SetupBuildingPosition(building, grid.GetBestCellCoordinates());
        return building;
    }

    private void SetupBuildingPosition(Building building, CellPlaceCoordinates placePosition) 
    {
        building.transform.position = grid.GetWorldPositionByCoordinates(placePosition.X, placePosition.Z) + building.BuildingOffset;
        grid.Place(placePosition);
        CellFilled?.Invoke(placePosition);
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