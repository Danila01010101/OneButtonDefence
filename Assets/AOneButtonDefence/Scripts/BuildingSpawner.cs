using System;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour, ICellPlacer
{
    private CellsGrid grid;
    private BuildingFactory buildingFacrory;

    public Action<CellPlacePosition> CellFilled { get; set; }

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
                Farm farm = SpawnFarm();
                SetupBuildingPosition(farm, grid.GetBestCellPlace());
                break;
            case UpgradeButton.Upgrades.SpiritBuilding:
                SpiritBuilding spiritBuilding = SpawnSpiritBuilding();
                SetupBuildingPosition(spiritBuilding, grid.GetBestCellPlace());
                break;
            case UpgradeButton.Upgrades.MilitaryCamp:
                MilitaryCamp militaryCamp = SpawnMilitaryCamp();
                SetupBuildingPosition(SpawnMilitaryCamp(), grid.GetBestCellPlace());
                break;
            case UpgradeButton.Upgrades.ResourcesCenter:
                Factory resourcesCenter = SpawnFactory();
                SetupBuildingPosition(resourcesCenter, grid.GetBestCellPlace());
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
        CellFilled?.Invoke(placePosition);
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