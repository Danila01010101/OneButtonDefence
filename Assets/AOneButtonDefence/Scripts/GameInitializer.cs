using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private WorldCreator worldCreator;
    [SerializeField] private GridChanger changer;

    private void Awake()
    {
        new GameObject("ResourcesCounter").AddComponent<ResourcesCounter>();
        var newGrid = new CellsGrid(gameData.GridSize);
        worldCreator.SetupGrid(newGrid, changer);
    }
}