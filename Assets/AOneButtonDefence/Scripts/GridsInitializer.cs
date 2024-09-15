using System.Collections.Generic;
using UnityEngine;

public class GridsInitializer : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [field: SerializeField] private List<GridUser> grids = new List<GridUser>();

    private void Awake()
    {
        foreach (GridUser grid in grids)
        {
            var newGrid = new CellsGrid(gameData.GridSize);
            grid.SetupGrid(newGrid);
        }
    }
}