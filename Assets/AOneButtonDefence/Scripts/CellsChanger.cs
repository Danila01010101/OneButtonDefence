using System;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    private CellsGrid grid;
    private BuildingFactory buildingFacrory;

    public Action<CellPlacePosition> BuildingSpawned;

    public void Initialize(CellsGrid grid, BuildingsData upgradeBuildings)
    {
        buildingFacrory = new BuildingFactory(upgradeBuildings);
        this.grid = grid;
    }

    private void Start()
    {
        SetupStartBuildings();
    }

    public void SetupStartBuildings()
    {
        SetupBuildingPosition(SpawnFarm(), grid.GetBestCellPlace());
        SetupBuildingPosition(SpawnFactory(), grid.GetBestCellPlace());
        SetupBuildingPosition(SpawnSpiritBuilding(), grid.GetBestCellPlace());
        SetupBuildingPosition(SpawnMilitaryCamp(), grid.GetBestCellPlace());
    }

    private void ActivateUpgrade(UpgradeButton.Upgrades upgrade)
    {
        switch (upgrade)
        {
            case UpgradeButton.Upgrades.Farm:
                SetupBuildingPosition(SpawnFarm(), grid.GetBestCellPlace());
                break;
            case UpgradeButton.Upgrades.SpiritBuilding:
                SetupBuildingPosition(SpawnSpiritBuilding(), grid.GetBestCellPlace());
                break;
            case UpgradeButton.Upgrades.MilitaryCamp:
                SetupBuildingPosition(SpawnMilitaryCamp(), grid.GetBestCellPlace());
                break;
            case UpgradeButton.Upgrades.ResourcesCenter:
                SetupBuildingPosition(SpawnFactory(), grid.GetBestCellPlace());
                break;
            default:
                throw new NotImplementedException();
        }
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

    private void OnEnable()
    {
        UpgradeButton.Upgrade += ActivateUpgrade;
    }

    private void OnDisable()
    {
        UpgradeButton.Upgrade -= ActivateUpgrade;
    }
}