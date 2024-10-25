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
    private CellsGrid buildingsGrid;

    private void Awake()
    {
        SpawnResourceCounter();
        SpawnWorldGrid();
        InitializeBuildingSpawner();
        SpawnUpgradeCanvas();
        SetupStateMachine();
    }

    private void SpawnResourceCounter() => new GameObject("ResourcesCounter").AddComponent<ResourcesCounter>();

    private void SpawnWorldGrid()
    {
        buildingsGrid = new CellsGrid(worldGenerationData.GridSize, worldGenerationData.CellsInterval);
        worldCreator.SetupGrid(buildingsGrid, buildingSpawner);
    }

    private void InitializeBuildingSpawner() => buildingSpawner.Initialize(buildingsGrid, worldGenerationData.BuildingsData, gameData.UpgradeStateDuration);

    private void SpawnUpgradeCanvas()
    {
        upgradeCanvas = Instantiate(partManagerPrefab);
        upgradeCanvas.Initialize(worldGenerationData.startButtonsAmount);
    }

    private void SetupStateMachine()
    {
        GameStateMachineData gameStateMachineData = new GameStateMachineData
        (
            upgradeCanvas,
            gameData,
            worldCreator,
            buildingsGrid
        );
        gameStateMachine = new GameStateMachine(gameStateMachineData, enemiesData, gameData.EnemiesSpawnOffset, gameData.UpgradeStateDuration);
    }
}