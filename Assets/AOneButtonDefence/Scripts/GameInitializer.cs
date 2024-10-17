using UnityEngine;
using static GameStateMachine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private EnemiesData enemiesData;
    [SerializeField] private WorldGenerationData worldGenerationData;
    [SerializeField] private GroundBlocksSpawner worldCreator;
    [SerializeField] private BuildingSpawner buildingSpawner;
    [SerializeField] private PartManager partManagerPrefab;

    private GameStateMachine gameStateMachine;
    private PartManager upgradeCanvas;

    private void Awake()
    {
        SpawnResourceCounter();
        SpawnWorldGrid();
        SetupBorderRememberer();
        SpawnUpgradeCanvas();
        SetupStateMachine();
    }

    private void SpawnResourceCounter() => new GameObject("ResourcesCounter").AddComponent<ResourcesCounter>();

    private void SpawnWorldGrid()
    {
        var newGrid = new CellsGrid(worldGenerationData.GridSize, worldGenerationData.CellsInterval);
        worldCreator.SetupGrid(newGrid, buildingSpawner);
    }

    private void SetupBorderRememberer()
    {
        BorderRememberer borderRememberer = new GameObject("BorderRememberer").AddComponent<BorderRememberer>();
        borderRememberer.Initialize(buildingSpawner, worldGenerationData.CellSize, gameData.EnemiesSpawnSpread);
    }

    private void SpawnUpgradeCanvas()
    {
        upgradeCanvas = Instantiate(partManagerPrefab);
        upgradeCanvas.Initialize(worldGenerationData.startButtonsAmount);
    }

    private void SetupStateMachine()
    {
        GameStateMachineData gameStateMachineData = new GameStateMachineData
        (
            upgradeCanvas.gameObject,
            gameData,
            worldCreator
        );
        gameStateMachine = new GameStateMachine(gameStateMachineData, enemiesData);
    }
}