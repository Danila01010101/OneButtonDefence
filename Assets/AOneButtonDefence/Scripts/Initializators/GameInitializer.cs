using System;
using System.Collections;
using System.Collections.Generic;
using AOneButtonDefence.Scripts.Initializators;
using UnityEngine;
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

    private IInput input;
    private IDisableableInput disableableInput;
    private GameStateMachine gameStateMachine;

    private void Awake()
    {
        StartCoroutine(InitializeGame());
    }

    private IEnumerator InitializeGame()
    {
        // Parent objects
        var parentInit = new ParentObjectsInitializer(transform);
        yield return parentInit.Initialize();
        Transform initializedObjectsParent = parentInit.InitializedParent;

        // Loading canvas
        var loadingCanvasInit = new LoadingCanvasInitializer(loadingCanvas);
        yield return loadingCanvasInit.Initialize();
        var spawnedLoadingCanvas = loadingCanvasInit.Instance;

        // Input
        var inputInit = new InputInitializer(gameData);
        yield return inputInit.Initialize();
        input = inputInit.Input;
        disableableInput = inputInit.DisableableInput;

        // Coroutine starter
        var coroutineStarterInit = new CoroutineStarterInitializer(initializedObjectsParent);
        yield return coroutineStarterInit.Initialize();

        // Skin detector
        var skinDetectorInit = new SkinDetectorInitializer();
        yield return skinDetectorInit.Initialize();
        var skinChangeDetector = skinDetectorInit.Instance;

        // Music player
        var musicPlayerInit = new MusicPlayerInitializer(initializedObjectsParent, musicData);
        yield return musicPlayerInit.Initialize();
        IBackgroundMusicPlayer backgroundMusicPlayer = musicPlayerInit.BackgroundPlayer;
        IUpgradeEffectPlayer upgradeEffectPlayer = musicPlayerInit.UpgradeEffectPlayer;
        backgroundMusicPlayer.StartLoadingMusic();

        // Music mediator
        var musicMediatorInit = new MusicMediatorInitializer(backgroundMusicPlayer, upgradeEffectPlayer);
        yield return musicMediatorInit.Initialize();
        var musicMediator = musicMediatorInit.Mediator;

        // Wave counter
        var waveCounterInit = new WaveCounterInitializer(initializedObjectsParent);
        yield return waveCounterInit.Initialize();

        // Battle notifier
        var battleNotifierInit = new BattleNotifierInitializer();
        yield return battleNotifierInit.Initialize();
        BattleNotifier battleNotifier = battleNotifierInit.Instance;

        yield return null;

        // Resources counter
        var resourceCounterInit = new ResourceCounterInitializer(initializedObjectsParent, gameData);
        yield return resourceCounterInit.Initialize();
        GameResourcesCounter gameResourcesCounter = resourceCounterInit.Instance;

        // Enemy detectors and factories (use original logic)
        LayerMask gnomeLayerMask = LayerMask.GetMask(gameData.GnomeLayerName);
        IEnemyDetector knightDetector = new UnitDetector(gameData.WorldSize, LayerMask.GetMask(gameData.EnemyLayerName), 1f, gameData.DefaultStoppingDistance);
        var gnomesFactory = new UnitsFactory(new List<FightingUnit>() { gameData.GnomeUnit }, knightDetector, gnomeLayerMask, gameData.GnomeTag);

        // Resources statistics
        var resourcesStatisticInit = new ResourcesStatisticInitializer(gameData, gameResourcesCounter, gnomesFactory);
        yield return resourcesStatisticInit.Initialize();
        var incomeDifferenceTextConverter = resourcesStatisticInit.IncomeConverter;

        // Resource change mediator
        var resourceChangeMediatorInit = new ResourceChangeMediatorInitializer(gameData);
        yield return resourceChangeMediatorInit.Initialize();
        var resourceChangeMediator = resourceChangeMediatorInit.Mediator;

        yield return null;

        // UI Object shower
        var uiObjectShowerInit = new UIObjectShowerInitializer(uiGameObjectShowerPrefab);
        yield return uiObjectShowerInit.Initialize();

        // Enemy death manager
        var enemyDeathManagerInit = new EnemyDeathManagerInitializer(initializedObjectsParent);
        yield return enemyDeathManagerInit.Initialize();

        // Dialog camera
        var dialogCameraInit = new DialogCameraInitializer(cameraData);
        yield return dialogCameraInit.Initialize();

        // Camera movement
        var cameraMovementInit = new CameraMovementInitializer(virtualCameraPrefab, initializedObjectsParent, input, cameraData);
        yield return cameraMovementInit.Initialize();

        yield return null;

        // Building spawner
        var buildingSpawnerInit = new BuildingSpawnerInitializer(initializedObjectsParent);
        yield return buildingSpawnerInit.Initialize();
        BuildingSpawner buildingSpawner = buildingSpawnerInit.Instance;

        // World grid
        var worldGridInit = new WorldGridInitializer(initializedObjectsParent, worldGenerationData, buildingSpawner, this);
        yield return worldGridInit.Initialize();
        yield return new WaitUntil(() => worldGridInit.IsWorldReady());
        GroundBlocksSpawner worldCreator = worldGridInit.WorldCreator;
        CellsGrid worldGrid = worldGridInit.Grid;

        // Initialize building spawner with grid
        buildingSpawner.Initialize(worldGrid, worldGenerationData.BuildingsData, gameData.UpgradeStateDuration);

        yield return null;
        yield return null;

        // Upgrade canvas
        var upgradeCanvasInit = new UpgradeCanvasInitializer(gameplayCanvasPrefab, worldGenerationData);
        yield return upgradeCanvasInit.Initialize();
        GameplayCanvas upgradeCanvas = upgradeCanvasInit.CanvasInstance;
        upgradeCanvas.EnemiesCountIndicator.Initiallize(gameData.BattleWavesParameters);

        yield return null;

        // Spell canvas
        var spellCanvasInit = new SpellCanvasInitializer(spellCanvas, input, spellCastData);
        yield return spellCanvasInit.Initialize();
        GameObject spellCanvasObj = spellCanvasInit.Instance;

        // Debug canvas
        var debugCanvasInit = new DebugCanvasInitializer(debugCanvas, addCoinsOnStart);
        yield return debugCanvasInit.Initialize();

        // Shop skin window
        var shopSkinInit = new ShopSkinWindowInitializer(shopSkinWindow, upgradeCanvas.transform, input);
        yield return shopSkinInit.Initialize();

        // Battle canvas (waypoints)
        var battleCanvasInit = new BattleCanvasInitializer(waypointUIManager, waypointData, Camera.main);
        yield return battleCanvasInit.Initialize();

        // Audio sources for volume control
        List<AudioSource> upgradeSources = upgradeEffectPlayer.GetSources();
        var startAudioSources = new List<AudioSource>();
        startAudioSources.Add(backgroundMusicPlayer.GetSource());
        startAudioSources.AddRange(upgradeSources);

        // Info canvas
        var infoCanvasInit = new InfoCanvasInitializer(infoCanvas, upgradeCanvas);
        yield return infoCanvasInit.Initialize();

        // Volume changer
        var volumeChangerInit = new VolumeChangerInitializer(upgradeCanvas, startAudioSources, musicData.StartValue);
        yield return volumeChangerInit.Initialize();

        // Gnome detector
        IEnemyDetector gnomeDetector = new UnitDetector(gameData.WorldSize, LayerMask.GetMask(gameData.GnomeLayerName), 1f, gameData.DefaultStoppingDistance);

        yield return null;

        // State machine
        var stateMachineInit = new StateMachineInitializer(gameData, enemiesData, upgradeCanvas, spellCanvasObj, worldCreator, worldGrid, disableableInput, gnomeDetector);
        yield return stateMachineInit.Initialize();
        gameStateMachine = stateMachineInit.Instance;

        // Subscribe battle notifier
        battleNotifier.Subscribe();

        // Reward spawner
        var rewardSpawnerInit = new RewardSpawnerInitializer(initializedObjectsParent, gameData);
        yield return rewardSpawnerInit.Initialize();

        // Tutorial
        var tutorialInit = new TutorialInitializer(tutorialManagerPrefab, upgradeCanvas.GetComponent<Canvas>());
        yield return tutorialInit.Initialize();
        
        //Tutorial mask
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
    }

    private void FixedUpdate()
    {
        if (!isSerializationCompleted) return;

        gameStateMachine?.PhysicsUpdate();
    }
}