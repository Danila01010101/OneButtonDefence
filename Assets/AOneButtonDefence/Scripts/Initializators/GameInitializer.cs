using System;
using System.Collections;
using System.Collections.Generic;
using AOneButtonDefence.Scripts;
using AOneButtonDefence.Scripts.Initializators;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
    private RendererDisabler rendererDisabler;
    private readonly List<IDisposable> disposables = new List<IDisposable>();

    private IEnumerator SafeStep(string name, Func<IEnumerator> step, Action onComplete = null)
    {
        if (step == null) yield break;

        IEnumerator routine = null;
        try
        {
            routine = step();
        }
        catch (Exception e)
        {
            Debug.LogError($"[Initializer Error] {name} during start: {e}");
        }

        if (routine != null)
        {
            while (true)
            {
                object current = null;
                try
                {
                    if (!routine.MoveNext()) break;
                    current = routine.Current;
                }
                catch (Exception e)
                {
                    Debug.LogError($"[Initializer Error] {name} during execution: {e}");
                    break;
                }
                yield return current;
            }
        }

        try
        {
            onComplete?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError($"[Initializer Error] {name} onComplete: {e}");
        }
    }

    private IEnumerator SafeWaitUntil(Func<bool> predicate, float timeoutSeconds, string stepName)
    {
        float t = 0f;
        while (t < timeoutSeconds)
        {
            bool ok = false;
            try { ok = predicate(); }
            catch (Exception e)
            {
                Debug.LogError($"[Initializer Error] {stepName} predicate: {e}");
                break;
            }
            if (ok) yield break;
            t += Time.deltaTime;
            yield return null;
        }
        Debug.LogWarning($"[Initializer Warning] {stepName} timed out after {timeoutSeconds} seconds.");
    }

    private void Awake() => StartCoroutine(InitializeGameSafe());

    private IEnumerator InitializeGameSafe()
    {
        Transform initializedObjectsParent = null;
        Canvas spawnedLoadingCanvas = null;
        IBackgroundMusicPlayer backgroundMusicPlayer = null;
        IUpgradeEffectPlayer upgradeEffectPlayer = null;
        BuildingSpawner buildingSpawner = null;
        GroundBlocksSpawner worldCreator = null;
        CellsGrid worldGrid = null;
        GameResourcesCounter gameResourcesCounter = null;
        UnitsFactory gnomesFactory = null;
        IncomeDifferenceTextConverter incomeDifferenceTextConverter = null;
        ResourceChangeMediator resourceChangeMediator = null;
        GameplayCanvas upgradeCanvas = null;
        GameObject spellCanvasObj = null;
        BattleNotifier battleNotifier = null;

        ParentObjectsInitializer parentInit = new ParentObjectsInitializer(transform);
        yield return SafeStep("ParentObjectsInitializer", () => parentInit.Initialize(), () =>
        {
            try { initializedObjectsParent = parentInit.InitializedParent; } catch { initializedObjectsParent = null; }
        });

        LoadingCanvasInitializer loadingCanvasInit = new LoadingCanvasInitializer(loadingCanvas);
        yield return SafeStep("LoadingCanvasInitializer", () => loadingCanvasInit.Initialize(), () =>
        {
            try { spawnedLoadingCanvas = loadingCanvasInit.Instance; } catch { spawnedLoadingCanvas = null; }
        });

        InputInitializer inputInit = new InputInitializer(gameData);
        yield return SafeStep("InputInitializer", () => inputInit.Initialize(), () =>
        {
            try
            {
                input = inputInit.Input;
                disableableInput = inputInit.DisableableInput;
            }
            catch { input = null; disableableInput = null; }
        });

        CoroutineStarterInitializer coroutineStarterInit = new CoroutineStarterInitializer(initializedObjectsParent);
        yield return SafeStep("CoroutineStarterInitializer", () => coroutineStarterInit.Initialize());

        disposables.Add(SkinChangeDetector.Instance);
        
        rendererDisabler = new RendererDisabler();
        yield return SafeStep("RendererDisabler", () => rendererDisabler.Initialize());
        
        disposables.Add(rendererDisabler);

        MusicPlayerInitializer musicPlayerInit = new MusicPlayerInitializer(initializedObjectsParent, musicData);
        yield return SafeStep("MusicPlayerInitializer", () => musicPlayerInit.Initialize(), () =>
        {
            try
            {
                backgroundMusicPlayer = musicPlayerInit.BackgroundPlayer;
                upgradeEffectPlayer = musicPlayerInit.UpgradeEffectPlayer;
                backgroundMusicPlayer?.StartLoadingMusic();
            }
            catch { backgroundMusicPlayer = null; upgradeEffectPlayer = null; }
        });

        MusicMediatorInitializer musicMediatorInit = new MusicMediatorInitializer(backgroundMusicPlayer, upgradeEffectPlayer);
        yield return SafeStep("MusicMediatorInitializer", () => musicMediatorInit.Initialize(), () =>
        {
            try { if (musicMediatorInit is IDisposable d) disposables.Add(d); } catch { }
        });

        WaveCounterInitializer waveCounterInit = new WaveCounterInitializer(initializedObjectsParent);
        yield return SafeStep("WaveCounterInitializer", () => waveCounterInit.Initialize());

        BattleNotifierInitializer battleNotifierInit = new BattleNotifierInitializer();
        yield return SafeStep("BattleNotifierInitializer", () => battleNotifierInit.Initialize(), () =>
        {
            try { battleNotifier = battleNotifierInit.Instance; } catch { battleNotifier = null; }
        });

        yield return null;

        ResourceCounterInitializer resourceCounterInit = new ResourceCounterInitializer(initializedObjectsParent, gameData);
        yield return SafeStep("ResourceCounterInitializer", () => resourceCounterInit.Initialize(), () =>
        {
            try { gameResourcesCounter = resourceCounterInit.Instance; } catch { gameResourcesCounter = null; }
        });

        try
        {
            LayerMask gnomeLayerMask = LayerMask.GetMask(gameData.GnomeLayerName);
            IEnemyDetector knightDetector = new UnitDetector(gameData.WorldSize, LayerMask.GetMask(gameData.EnemyLayerName), 1f, gameData.DefaultStoppingDistance);
            gnomesFactory = new UnitsFactory(new List<FightingUnit>() { gameData.GnomeUnit }, knightDetector, gnomeLayerMask, gameData.GnomeTag);
        }
        catch { gnomesFactory = null; }

        ResourcesStatisticInitializer resourcesStatisticInit = new ResourcesStatisticInitializer(gameData, gameResourcesCounter, gnomesFactory);
        yield return SafeStep("ResourcesStatisticInitializer", () => resourcesStatisticInit.Initialize(), () =>
        {
            try { incomeDifferenceTextConverter = resourcesStatisticInit.IncomeConverter; } catch { incomeDifferenceTextConverter = null; }
        });

        ResourceChangeMediatorInitializer resourceChangeMediatorInit = new ResourceChangeMediatorInitializer(gameData);
        yield return SafeStep("ResourceChangeMediatorInitializer", () => resourceChangeMediatorInit.Initialize(), () =>
        {
            try { resourceChangeMediator = resourceChangeMediatorInit.Mediator; disposables.Add(resourceChangeMediatorInit); } catch { resourceChangeMediator = null; }
        });

        yield return null;

        UIObjectShowerInitializer uiObjectShowerInit = new UIObjectShowerInitializer(uiGameObjectShowerPrefab);
        yield return SafeStep("UIObjectShowerInitializer", () => uiObjectShowerInit.Initialize());

        EnemyDeathManagerInitializer enemyDeathManagerInit = new EnemyDeathManagerInitializer(initializedObjectsParent);
        yield return SafeStep("EnemyDeathManagerInitializer", () => enemyDeathManagerInit.Initialize());

        DialogCameraInitializer dialogCameraInit = new DialogCameraInitializer(cameraData);
        yield return SafeStep("DialogCameraInitializer", () => dialogCameraInit.Initialize());

        CameraMovementInitializer cameraMovementInit = new CameraMovementInitializer(virtualCameraPrefab, initializedObjectsParent, input, cameraData);
        yield return SafeStep("CameraMovementInitializer", () => cameraMovementInit.Initialize());

        yield return null;

        BuildingSpawnerInitializer buildingSpawnerInit = new BuildingSpawnerInitializer(initializedObjectsParent);
        yield return SafeStep("BuildingSpawnerInitializer", () => buildingSpawnerInit.Initialize(), () =>
        {
            try { buildingSpawner = buildingSpawnerInit.Instance; } catch { buildingSpawner = null; }
        });

        WorldGridInitializer worldGridInit = new WorldGridInitializer(initializedObjectsParent, worldGenerationData, buildingSpawner, this);
        yield return SafeStep("WorldGridInitializer", () => worldGridInit.Initialize(), () =>
        {
            try { worldCreator = worldGridInit.WorldCreator; worldGrid = worldGridInit.Grid; } catch { worldCreator = null; worldGrid = null; }
        });

        if (worldGridInit != null)
            yield return SafeWaitUntil(() => worldGridInit.IsWorldReady(), 20f, "WorldGridReady");

        if (buildingSpawner != null && worldGrid != null && worldGenerationData?.BuildingsData != null)
        {
            try { buildingSpawner.Initialize(worldGrid, worldGenerationData.BuildingsData, gameData.UpgradeStateDuration); }
            catch (Exception e) { Debug.LogError($"buildingSpawner.Initialize failed: {e}"); }
        }

        yield return null;

        UpgradeCanvasInitializer upgradeCanvasInit = new UpgradeCanvasInitializer(gameplayCanvasPrefab, worldGenerationData);
        yield return SafeStep("UpgradeCanvasInitializer", () => upgradeCanvasInit.Initialize(), () =>
        {
            try { upgradeCanvas = upgradeCanvasInit.CanvasInstance; } catch { upgradeCanvas = null; }
        });

        try
        {
            if (upgradeCanvas != null)
                upgradeCanvas.EnemiesCountIndicator.Initiallize(gameData.BattleWavesParameters);
        }
        catch (Exception e) { Debug.LogError($"EnemiesCountIndicator initialization failed: {e}"); }

        SpellCanvasInitializer spellCanvasInit = new SpellCanvasInitializer(spellCanvas, input, spellCastData);
        yield return SafeStep("SpellCanvasInitializer", () => spellCanvasInit.Initialize(), () =>
        {
            try { spellCanvasObj = spellCanvasInit.Instance; } catch { spellCanvasObj = null; }
        });

        DebugCanvasInitializer debugCanvasInit = new DebugCanvasInitializer(debugCanvas, addCoinsOnStart, gameResourcesCounter, gameData.GemsResource);
        yield return SafeStep("DebugCanvasInitializer", () => debugCanvasInit.Initialize());

        BattleCanvasInitializer battleCanvasInit = new BattleCanvasInitializer(waypointUIManager, waypointData, Camera.main);
        yield return SafeStep("BattleCanvasInitializer", () => battleCanvasInit.Initialize());

        List<AudioSource> upgradeSources = new List<AudioSource>();
        try { upgradeSources = upgradeEffectPlayer?.GetSources() ?? new List<AudioSource>(); } catch { }
        var startAudioSources = new List<AudioSource>();
        try
        {
            var bgSource = backgroundMusicPlayer?.GetSource();
            if (bgSource != null) startAudioSources.Add(bgSource);
            if (upgradeSources != null) startAudioSources.AddRange(upgradeSources.FindAll(s => s != null));
        }
        catch { }

        InfoCanvasInitializer infoCanvasInit = new InfoCanvasInitializer(infoCanvas, upgradeCanvas);
        yield return SafeStep("InfoCanvasInitializer", () => infoCanvasInit.Initialize());

        VolumeChangerInitializer volumeChangerInit = new VolumeChangerInitializer(upgradeCanvas, startAudioSources, musicData.StartValue);
        yield return SafeStep("VolumeChangerInitializer", () => volumeChangerInit.Initialize());

        IEnemyDetector gnomeDetector = null;
        try { gnomeDetector = new UnitDetector(gameData.WorldSize, LayerMask.GetMask(gameData.GnomeLayerName), 1f, gameData.DefaultStoppingDistance); } catch { gnomeDetector = null; }

        yield return null;

        StateMachineInitializer stateMachineInit = new StateMachineInitializer(gameData, enemiesData, upgradeCanvas, spellCanvasObj, worldCreator, worldGrid, disableableInput, gnomeDetector);
        yield return SafeStep("StateMachineInitializer", () => stateMachineInit.Initialize(), () =>
        {
            try { gameStateMachine = stateMachineInit.Instance; } catch { gameStateMachine = null; }
        });

        Vector3 playerSpawnPosition = Vector3.zero;
        try
        {
            int centerIndex = worldGrid != null ? (worldGrid.Size / 2 - 1) : 0;
            playerSpawnPosition = (worldGrid != null ? worldGrid.GetWorldPositionByCoordinates(Mathf.Max(0, centerIndex), Mathf.Max(0, centerIndex)) : Vector3.zero) + (gameData != null ? gameData.GnomeSpawnOffset : Vector3.zero);
        }
        catch { playerSpawnPosition = Vector3.zero; }

        BattleEvents battleEvents = new BattleEvents();
        PlayerControllerInitializer.PlayerControllerInitializerData playerInitializeData =
            new PlayerControllerInitializer.PlayerControllerInitializerData(
                gameData.PlayerUnit,
                gameData.PlayerUnitData,
                Camera.main,
                playerSpawnPosition,
                battleEvents);
        PlayerControllerInitializer playerInitializer = new PlayerControllerInitializer(playerInitializeData);
        yield return SafeStep("PlayerControllerInitializer", () => playerInitializer.Initialize(), () =>
        {
            try { positionForTestOrb = playerSpawnPosition; } catch { }
        });
        disposables.Add(playerInitializer);

        try { battleNotifier?.Subscribe(); } catch { }

        RewardSpawnerInitializer rewardSpawnerInit = new RewardSpawnerInitializer(initializedObjectsParent, gameData);
        yield return SafeStep("RewardSpawnerInitializer", () => rewardSpawnerInit.Initialize());

        TutorialInitializer tutorialInit = new TutorialInitializer(tutorialManagerPrefab, upgradeCanvas != null ? upgradeCanvas.GetComponent<Canvas>() : null);
        yield return SafeStep("TutorialInitializer", () => tutorialInit.Initialize());

        TutorialMaskInitializer tutorialSpotlightMask = new TutorialMaskInitializer(tutorialMaskPrefab, upgradeCanvas != null ? upgradeCanvas.GetComponent<Canvas>() : null);
        yield return SafeStep("TutorialMaskInitializer", () => tutorialSpotlightMask.Initialize());

        yield return null;
        
        ShopSkinWindowInitializer shopSkinInit = new ShopSkinWindowInitializer(shopSkinWindow, upgradeCanvas != null ? upgradeCanvas.transform : null, input);
        yield return SafeStep("ShopSkinWindowInitializer", () => shopSkinInit.Initialize());
        shopSkinInit.SetWindowsConnection(upgradeCanvasInit.CanvasInstance);

        yield return null;

        try { GameInitialized?.Invoke(); } catch { }
        isSerializationCompleted = true;
        
        if (spawnedLoadingCanvas != null)
        {
            try { Destroy(spawnedLoadingCanvas.gameObject); } catch { Destroy(spawnedLoadingCanvas); }
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R) && addCoinsOnStart) SceneManager.LoadScene(0);
        if (!isSerializationCompleted) return;
        try { gameStateMachine?.Update(); } catch { }
        try { gameStateMachine?.HandleInput(); } catch { }
        try { input?.Update(); } catch { }
    }

    private void LateUpdate()
    {
        try { rendererDisabler?.LateUpdate(); } catch { }
        try { input?.LateUpdate(); } catch { }
    }

    private void FixedUpdate()
    {
        if (!isSerializationCompleted) return;
        try { gameStateMachine?.PhysicsUpdate(); } catch { }
    }

    private void OnDestroy()
    {
        foreach (var disposable in disposables)
        {
            try { disposable.Dispose(); } catch (Exception e) { Debug.LogError($"Dispose error: {disposable.GetType().Name} - {e}"); }
        }
        disposables.Clear();
    }
}