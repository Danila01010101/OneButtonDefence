using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [field: SerializeField] private List<GridUser> grids = new List<GridUser>();

    private void Awake()
    {
        new GameObject("ResourcesCounter").AddComponent<ResourcesCounter>();

        foreach (GridUser grid in grids)
        {
            var newGrid = new CellsGrid(gameData.GridSize);
            grid.SetupGrid(newGrid);
        }
    }
}