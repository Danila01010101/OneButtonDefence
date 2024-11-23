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

    private void Start()
    {
        SetupStartBuildings();
    }

    public void SetupStartBuildings()
    {
        SpawnBuilding<MainBuilding>(Building.BuildingType.MainCastle);
        SpawnBuilding<Factory>(Building.BuildingType.Factory);
        SpawnBuilding<Farm>(Building.BuildingType.Farm);
        SpawnBuilding<SpiritBuilding>(Building.BuildingType.SpiritBuilding);
        SpawnBuilding<MilitaryCamp>(Building.BuildingType.MilitaryCamp);
    }

    private void ActivateUpgrade(Building.BuildingType upgrade)
    {
        switch (upgrade)
        {
            case Building.BuildingType.Farm:
                SpawnBuilding<Farm>(upgrade);
                break;
            case Building.BuildingType.SpiritBuilding:
                SpawnBuilding<SpiritBuilding>(upgrade);
                break;
            case Building.BuildingType.MilitaryCamp:
                SpawnBuilding<MilitaryCamp>(upgrade);
                break;
            case Building.BuildingType.Factory:
                SpawnBuilding<Factory>(upgrade);
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void SpawnBuilding<T>(Building.BuildingType type) where T : Building
    {
        T building = buildingFacrory.SpawnBuilding<T>();
        CellPlaceCoordinates cellPlaceCoordinates = grid.GetBestCellCoordinates();
        building.transform.position = grid.GetWorldPositionByCoordinates(cellPlaceCoordinates.X, cellPlaceCoordinates.Z) + building.BuildingOffset;
        grid.PlaceBuilding(cellPlaceCoordinates, type);
        CellFilled?.Invoke(cellPlaceCoordinates);
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