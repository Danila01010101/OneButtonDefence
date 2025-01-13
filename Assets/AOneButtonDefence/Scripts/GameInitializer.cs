using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using static GameStateMachine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private CameraData cameraData;
    [SerializeField] private MusicData musicData;
    [SerializeField] private EnemiesData enemiesData;
    [SerializeField] private WorldGenerationData worldGenerationData;
    [SerializeField] private GroundBlocksSpawner worldCreator;
    [SerializeField] private BuildingSpawner buildingSpawner;
    [SerializeField] private PartManager partManagerPrefab;
    [SerializeField] private CinemachineVirtualCamera virtualCameraPrefab;
    [SerializeField] private Canvas loadingCanvas;

    private bool isSerializationCompleted;
    private GameStateMachine gameStateMachine;
    private IInput input;
    private IDisableableInput disableableInput;

    private void Awake()
    {
        StartCoroutine(StartInitialization());
    }

    private IEnumerator StartInitialization()
    {
        SetupLoadingCanvas();
        InitializeInput();
        Tuple<IBackgroundMusicPlayer, IUpgradeEffectPlayer> players = InitializeMusicPlayer();
        IBackgroundMusicPlayer backgroundMusicPlayer = players.Item1;
        IUpgradeEffectPlayer upgradeEffectPlayer = players.Item2;
        InitializeMusicMediator(backgroundMusicPlayer, upgradeEffectPlayer);
        yield return null;
        SpawnResourceCounter();
        yield return null;
        InitializeDialogCamera();
        InitializeCameraMovementComponent();
        yield return null;
        var worldGrid = SpawnWorldGrid();
        yield return new WaitUntil(() => worldCreator.IsWorldReady);
        InitializeBuildingSpawner(worldGrid, worldGenerationData.BuildingsData, gameData.UpgradeStateDuration);
        yield return null;
        PartManager upgradeCanvas = SpawnUpgradeCanvas();
        yield return null;
        SetupStateMachine(upgradeCanvas, worldCreator, worldGrid, disableableInput);
        yield return null;
        isSerializationCompleted = true;
        Destroy(loadingCanvas.gameObject);
    }

    private void Update()
    {
        if (isSerializationCompleted == false)
            return;
        
        gameStateMachine.Update();
        gameStateMachine.HandleInput();
        input.LateUpdate();
    }

    private void FixedUpdate()
    {
        if (isSerializationCompleted == false)
            return;
            
        gameStateMachine.PhysicsUpdate();
    }

    private void SetupLoadingCanvas() => loadingCanvas = Instantiate(loadingCanvas);

    private Tuple<IBackgroundMusicPlayer, IUpgradeEffectPlayer> InitializeMusicPlayer()
    {
        var musicPlayerGameObject = new GameObject("MusicPlayer");
        var backgroundPlayer = musicPlayerGameObject.AddComponent<AudioSource>();
        var firstUpgradePlayer = musicPlayerGameObject.AddComponent<AudioSource>();
        var secondUpgradePlayer = musicPlayerGameObject.AddComponent<AudioSource>();
        var musicPlayer = new GameMusicPlayer(musicData, backgroundPlayer, firstUpgradePlayer, secondUpgradePlayer);
        var allMusicPlayers = new Tuple<IBackgroundMusicPlayer, IUpgradeEffectPlayer>(musicPlayer, musicPlayer);
        return allMusicPlayers;
    }

    private void InitializeMusicMediator(IBackgroundMusicPlayer backgroundMusicPlayer, IUpgradeEffectPlayer upgradeEffectPlayer)
    {
        var musicMediator = new MusicPlayerMediator(backgroundMusicPlayer, upgradeEffectPlayer);
    }

    private void InitializeInput()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            throw new System.NotImplementedException();
        }
        else
        {
            var initializedInput = new DesctopInput(gameData.SwipeDeadZone);
            input = initializedInput;
            disableableInput = initializedInput;
        }
    }

    private void SpawnResourceCounter()
    {
        ResourcesCounter resourcesCounter = new GameObject("ResourcesCounter").AddComponent<ResourcesCounter>();
        resourcesCounter.SetStartValues(gameData.StartFoodAmount, gameData.StartMaterialsAmount, gameData.StartSpiritAmount);
    }

    private void InitializeDialogCamera()
    {
        var dialogCamera = Instantiate(cameraData.DialogCameraPrefab);
        dialogCamera.transform.position = cameraData.DialogCameraPosition;
        dialogCamera.transform.eulerAngles = cameraData.DialogCameraEulerAngles;
    }

    private void InitializeCameraMovementComponent()
    {
        CameraMovement cameraMovement = Instantiate(virtualCameraPrefab).gameObject.AddComponent<CameraMovement>();
        cameraMovement.gameObject.name = "CameraMovement";
        cameraMovement.Initialize(input, cameraData);
    }

    private CellsGrid SpawnWorldGrid()
    {
        var buildingsGrid = new CellsGrid(worldGenerationData.GridSize, worldGenerationData.CellsInterval);
        worldCreator.SetupGrid(worldGenerationData, buildingsGrid, buildingSpawner, this);
        return buildingsGrid;
    }

    private void InitializeBuildingSpawner(CellsGrid grid, 
        BuildingsData upgradeBuildings, 
        float animationDuration) => buildingSpawner.Initialize(grid, upgradeBuildings, animationDuration);

    private PartManager SpawnUpgradeCanvas()
    {
        PartManager upgradeCanvas = Instantiate(partManagerPrefab);
        upgradeCanvas.Initialize(worldGenerationData.startButtonsAmount);
        return upgradeCanvas;
    }

    private void SetupStateMachine(PartManager gameplayCanvas, GroundBlocksSpawner worldCreator, CellsGrid grid, IDisableableInput inputForDialogueState)
    {
        GameStateMachineData gameStateMachineData = new GameStateMachineData 
        (
            gameplayCanvas,
            gameData,
            worldCreator,
            grid,
            gameData.EnemyTag,
            gameData.GnomeTag,
            inputForDialogueState
        );
        gameStateMachine = new GameStateMachine(gameStateMachineData, enemiesData, gameData.EnemiesSpawnOffset, gameData.UpgradeStateDuration);
    }
}