using Cinemachine;
using UnityEngine;
using static GameStateMachine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private GameStartData gameData;
    [SerializeField] private CameraData cameraData;
    [SerializeField] private EnemiesData enemiesData;
    [SerializeField] private WorldGenerationData worldGenerationData;
    [SerializeField] private BuildingSpawner buildingSpawner;
    [SerializeField] private PartManager partManagerPrefab;
    [SerializeField] private CinemachineVirtualCamera virtualCameraPrefab;

    private GroundBlocksSpawner worldCreator;
    private GameStateMachine gameStateMachine;
    private PartManager upgradeCanvas;
    private GroundBlocksFactory blocksFactory;
    private CellsGrid cellsGrid;
    private GameSaver gameSaver;
    private Data data;
    private IInput input;

    private void Awake()
    {
        InitializeGameSaver();
        InitializeInput();
        InitializeCameraMovementComponent();
        InitializeBlocksFactory();
        InitializeGroundBlockSpawner();
        SpawnWorldGrid();
        SpawnResourceCounter();
        InitializeBuildingSpawner();
        SpawnUpgradeCanvas();
        SetupStateMachine();
    }

    private void Update()
    {
        gameStateMachine.Update();
        gameStateMachine.HandleInput();
        input.LateUpdate();
    }

    private void FixedUpdate() => gameStateMachine.PhysicsUpdate();

    private void InitializeGameSaver()
    {
        gameSaver = new GameSaver();
        gameSaver.Load();
    }

    private void InitializeInput()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            throw new System.NotImplementedException();
        }
        else
        {
            input = new DesctopInput(gameData.SwipeDeadZone);
        }
    }

    private void SpawnResourceCounter() => new GameObject("ResourcesCounter").AddComponent<ResourcesCounter>();

    private void InitializeCameraMovementComponent()
    {
        CameraMovement cameraMovement = Instantiate(virtualCameraPrefab).gameObject.AddComponent<CameraMovement>();
        cameraMovement.gameObject.name = "CameraMovement";
        cameraMovement.Initialize(input, cameraData);
    }

    private void InitializeBlocksFactory() => blocksFactory = new GroundBlocksFactory(worldGenerationData);

    private void InitializeGroundBlockSpawner() => worldCreator = new GroundBlocksSpawner();

    private void SpawnWorldGrid()
    {
        if (GameSaver.Instance.Data.CellsGrid == null)
        {
            cellsGrid = new CellsGrid(worldGenerationData.GridSize, worldGenerationData.CellsInterval);
            GameSaver.Instance.Data.InitializeEmptyData(cellsGrid);
            worldCreator.SetupGrid(cellsGrid, buildingSpawner, blocksFactory);
            worldCreator.GenerateNewWorld();
        }
        else
        {
            cellsGrid = GameSaver.Instance.Data.CellsGrid;
            worldCreator.SetupGrid(cellsGrid, buildingSpawner, blocksFactory);
            worldCreator.GenerateExistingWorld();
        }
    }

    private void InitializeBuildingSpawner() => buildingSpawner.Initialize(cellsGrid, worldGenerationData.BuildingsData, gameData.UpgradeStateDuration);

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
            this,
            cellsGrid,
            gameData.EnemyTag
        );
        gameStateMachine = new GameStateMachine(gameStateMachineData, enemiesData, gameData.EnemiesSpawnOffset, gameData.UpgradeStateDuration);
    }
}