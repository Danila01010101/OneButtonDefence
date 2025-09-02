using System;
using System.Collections;
using System.Collections.Generic;
using AOneButtonDefence.Scripts;
using AOneButtonDefence.Scripts.Initializators;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameInitializer : MonoBehaviour
{
    [FormerlySerializedAs("isTestBuild")] [SerializeField] private bool addCoinsOnStart;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera virtualCameraPrefab;
    [Header("Data")]
    [SerializeField] private GameData gameData;
    [SerializeField] private CameraData cameraData;
    [SerializeField] private MusicData musicData;
    [SerializeField] private EnemiesData enemiesData;
    [SerializeField] private WorldGenerationData worldGenerationData;
    [SerializeField] private SpellCastData spellCastData;
    [SerializeField] private WrightAngle.Waypoint.WaypointSettings waypointData;
    [Header("Canvas")]
    [SerializeField] private GameplayCanvas gameplayCanvasPrefab;
    [SerializeField] private Canvas loadingCanvas;
    [SerializeField] private GameObject debugCanvas;
    [SerializeField] private SpellCanvas spellCanvas;
    [SerializeField] private TutorialManager tutorialManagerPrefab;
    [SerializeField] private SpotlightTutorialMask tutorialMaskPrefab;
    [SerializeField] private SkinPanel shopSkinWindow;
    [SerializeField] private UIGameObjectShower uiGameObjectShowerPrefab;
    [SerializeField] private ClosableWindow infoCanvas;
    [SerializeField] private WrightAngle.Waypoint.WaypointUIManager waypointUIManager;

    private bool isSerializationCompleted;
    public static Action GameInitialized;

    private Vector3 positionForTestOrb;

    private IInput input;
    private IDisableableInput disableableInput;
    private GameStateMachine gameStateMachine;
    private readonly List<IDisposable> disposables = new List<IDisposable>();

    private void Awake()
    {
        StartCoroutine(InitializeGame());
    }

    private IEnumerator InitializeGame()
    {
        var parentInit = new ParentObjectsInitializer(transform);
        yield return parentInit.Initialize();
        Transform initializedObjectsParent = parentInit.InitializedParent;

        var loadingCanvasInit = new LoadingCanvasInitializer(loadingCanvas);
        yield return loadingCanvasInit.Initialize();
        var spawnedLoadingCanvas = loadingCanvasInit.Instance;

        var inputInit = new InputInitializer(gameData);
        yield return inputInit.Initialize();
        input = inputInit.Input;
        disableableInput = inputInit.DisableableInput;

        var coroutineStarterInit = new CoroutineStarterInitializer(initializedObjectsParent);
        yield return coroutineStarterInit.Initialize();

        var skinDetectorInit = new SkinDetectorInitializer();
        yield return skinDetectorInit.Initialize();
        var skinChangeDetector = skinDetectorInit.Instance;

        var musicPlayerInit = new MusicPlayerInitializer(initializedObjectsParent, musicData);
        yield return musicPlayerInit.Initialize();
        IBackgroundMusicPlayer backgroundMusicPlayer = musicPlayerInit.BackgroundPlayer;
        IUpgradeEffectPlayer upgradeEffectPlayer = musicPlayerInit.UpgradeEffectPlayer;
        backgroundMusicPlayer.StartLoadingMusic();

        var musicMediatorInit = new MusicMediatorInitializer(backgroundMusicPlayer, upgradeEffectPlayer);
        yield return musicMediatorInit.Initialize();
        var musicMediator = musicMediatorInit.Mediator;
        
        if (musicMediatorInit is IDisposable disposable)
            disposables.Add(disposable);

        var waveCounterInit = new WaveCounterInitializer(initializedObjectsParent);
        yield return waveCounterInit.Initialize();

        var battleNotifierInit = new BattleNotifierInitializer();
        yield return battleNotifierInit.Initialize();
        BattleNotifier battleNotifier = battleNotifierInit.Instance;

        yield return null;

        var resourceCounterInit = new ResourceCounterInitializer(initializedObjectsParent, gameData);
        yield return resourceCounterInit.Initialize();
        GameResourcesCounter gameResourcesCounter = resourceCounterInit.Instance;

        LayerMask gnomeLayerMask = LayerMask.GetMask(gameData.GnomeLayerName);
        IEnemyDetector knightDetector = new UnitDetector(gameData.WorldSize, LayerMask.GetMask(gameData.EnemyLayerName), 1f, gameData.DefaultStoppingDistance);
        var gnomesFactory = new UnitsFactory(new List<FightingUnit>() { gameData.GnomeUnit }, knightDetector, gnomeLayerMask, gameData.GnomeTag);

        var resourcesStatisticInit = new ResourcesStatisticInitializer(gameData, gameResourcesCounter, gnomesFactory);
        yield return resourcesStatisticInit.Initialize();
        var incomeDifferenceTextConverter = resourcesStatisticInit.IncomeConverter;

        var resourceChangeMediatorInit = new ResourceChangeMediatorInitializer(gameData);
        yield return resourceChangeMediatorInit.Initialize();
        var resourceChangeMediator = resourceChangeMediatorInit.Mediator;

        yield return null;

        var uiObjectShowerInit = new UIObjectShowerInitializer(uiGameObjectShowerPrefab);
        yield return uiObjectShowerInit.Initialize();

        var enemyDeathManagerInit = new EnemyDeathManagerInitializer(initializedObjectsParent);
        yield return enemyDeathManagerInit.Initialize();

        var dialogCameraInit = new DialogCameraInitializer(cameraData);
        yield return dialogCameraInit.Initialize();

        var cameraMovementInit = new CameraMovementInitializer(virtualCameraPrefab, initializedObjectsParent, input, cameraData);
        yield return cameraMovementInit.Initialize();

        yield return null;

        var buildingSpawnerInit = new BuildingSpawnerInitializer(initializedObjectsParent);
        yield return buildingSpawnerInit.Initialize();
        BuildingSpawner buildingSpawner = buildingSpawnerInit.Instance;

        var worldGridInit = new WorldGridInitializer(initializedObjectsParent, worldGenerationData, buildingSpawner, this);
        yield return worldGridInit.Initialize();
        yield return new WaitUntil(() => worldGridInit.IsWorldReady());
        GroundBlocksSpawner worldCreator = worldGridInit.WorldCreator;
        CellsGrid worldGrid = worldGridInit.Grid;

        buildingSpawner.Initialize(worldGrid, worldGenerationData.BuildingsData, gameData.UpgradeStateDuration);

        yield return null;
        yield return null;

        var upgradeCanvasInit = new UpgradeCanvasInitializer(gameplayCanvasPrefab, worldGenerationData);
        yield return upgradeCanvasInit.Initialize();
        GameplayCanvas upgradeCanvas = upgradeCanvasInit.CanvasInstance;
        upgradeCanvas.EnemiesCountIndicator.Initiallize(gameData.BattleWavesParameters);

        yield return null;

        var spellCanvasInit = new SpellCanvasInitializer(spellCanvas, input, spellCastData);
        yield return spellCanvasInit.Initialize();
        GameObject spellCanvasObj = spellCanvasInit.Instance;

        var debugCanvasInit = new DebugCanvasInitializer(debugCanvas, addCoinsOnStart);
        yield return debugCanvasInit.Initialize();

        var shopSkinInit = new ShopSkinWindowInitializer(shopSkinWindow, upgradeCanvas.transform, input);
        yield return shopSkinInit.Initialize();

        var battleCanvasInit = new BattleCanvasInitializer(waypointUIManager, waypointData, Camera.main);
        yield return battleCanvasInit.Initialize();

        List<AudioSource> upgradeSources = upgradeEffectPlayer.GetSources();
        var startAudioSources = new List<AudioSource>();
        startAudioSources.Add(backgroundMusicPlayer.GetSource());
        startAudioSources.AddRange(upgradeSources);

        var infoCanvasInit = new InfoCanvasInitializer(infoCanvas, upgradeCanvas);
        yield return infoCanvasInit.Initialize();

        var volumeChangerInit = new VolumeChangerInitializer(upgradeCanvas, startAudioSources, musicData.StartValue);
        yield return volumeChangerInit.Initialize();

        IEnemyDetector gnomeDetector = new UnitDetector(gameData.WorldSize, LayerMask.GetMask(gameData.GnomeLayerName), 1f, gameData.DefaultStoppingDistance);

        yield return null;

        var stateMachineInit = new StateMachineInitializer(gameData, enemiesData, upgradeCanvas, spellCanvasObj, worldCreator, worldGrid, disableableInput, gnomeDetector);
        yield return stateMachineInit.Initialize();
        gameStateMachine = stateMachineInit.Instance;
        
        int centerIndex = worldGrid.Size / 2 - 1;
        Vector3 playerSpawnPosition = worldGrid.GetWorldPositionByCoordinates(centerIndex, centerIndex) 
                                      + gameData.GnomeSpawnOffset;
        BattleEvents battleEvents = new BattleEvents();
        PlayerControllerInitializer.PlayerControllerInitializerData playerInitializeData =
            new PlayerControllerInitializer.PlayerControllerInitializerData(
                gameData.PlayerUnit,
                gameData.PlayerUnitData,
                Camera.main,
                playerSpawnPosition,
                battleEvents);
        PlayerControllerInitializer playerInitializer = new PlayerControllerInitializer(playerInitializeData);
        positionForTestOrb = playerSpawnPosition;
        yield return playerInitializer.Initialize();


        battleNotifier.Subscribe();

        var rewardSpawnerInit = new RewardSpawnerInitializer(initializedObjectsParent, gameData);
        yield return rewardSpawnerInit.Initialize();

        var tutorialInit = new TutorialInitializer(tutorialManagerPrefab, upgradeCanvas.GetComponent<Canvas>());
        yield return tutorialInit.Initialize();
        
        var tutorialSpotlightMask = new TutorialMaskInitializer(tutorialMaskPrefab, upgradeCanvas.GetComponent<Canvas>());
        yield return tutorialSpotlightMask.Initialize();

        yield return null;

        GameInitialized?.Invoke();
        isSerializationCompleted = true;
        Destroy(spawnedLoadingCanvas.gameObject);
    }

    private void Update()
    {
        if (!isSerializationCompleted) return;

        gameStateMachine?.Update();
        gameStateMachine?.HandleInput();
        input?.Update();
    }

    private void LateUpdate()
    {
        input?.LateUpdate();
        
        if (Input.GetKey(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void FixedUpdate()
    {
        if (!isSerializationCompleted) return;

        gameStateMachine?.PhysicsUpdate();
    }

    private void OnDrawGizmos()
    {
        if (positionForTestOrb != null)
            Gizmos.DrawCube(positionForTestOrb, Vector3.one * 10);
    }
    
    private void OnDestroy()
    {
        foreach (var disposable in disposables)
        {
            try
            {
                disposable.Dispose();
            }
            catch (Exception e)
            {
                Debug.LogError($"Dispose error: {disposable.GetType().Name} - {e}");
            }
        }
        disposables.Clear();
    }
}