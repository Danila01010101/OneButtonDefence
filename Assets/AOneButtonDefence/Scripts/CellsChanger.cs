using UnityEngine;

public class GridChanger : MonoBehaviour
{
    private CellsGrid grid;
    private BuildingFacrory buildingFacrory;

    public void Initialize(CellsGrid grid, UpgradeBuildings upgradeBuildings)
    {
        buildingFacrory = new BuildingFacrory(upgradeBuildings);
        this.grid = grid;
    }

    public void SetupStartBuildings()
    {

    }
}