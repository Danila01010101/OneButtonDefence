using Cinemachine;
using UnityEngine;
using static GameStateMachine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private CameraData cameraData;
    [SerializeField] private EnemiesData enemiesData;
    [SerializeField] private WorldGenerationData worldGenerationData;
    [SerializeField] private GroundBlocksSpawner worldCreator;
    [SerializeField] private BuildingSpawner buildingSpawner;
    [SerializeField] private PartManager partManagerPrefab;
    [SerializeField] private CinemachineVirtualCamera virtualCameraPrefab;

    private GameStateMachine gameStateMachine;
    private PartManager upgradeCanvas;
    private CellsGrid buildingsGrid;
    private IInput input;

    private void Awake()
    {
        InitializeInput();
        InitializeCameraMovementComponent();
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
            buildingsGrid,
            gameData.EnemyTag
        );
        gameStateMachine = new GameStateMachine(gameStateMachineData, enemiesData, gameData.EnemiesSpawnOffset, gameData.UpgradeStateDuration);
    }
}