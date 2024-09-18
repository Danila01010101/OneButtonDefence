using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private GroundBlocksSpawner worldCreator;
    [SerializeField] private BuildingSpawner changer;

    private void Awake()
    {
        new GameObject("ResourcesCounter").AddComponent<ResourcesCounter>();
        var newGrid = new CellsGrid(gameData.GridSize, gameData.CellsInterval);
        worldCreator.SetupGrid(newGrid, changer);
    }
}