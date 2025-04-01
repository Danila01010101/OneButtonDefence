using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using static GameStateMachine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private CameraData cameraData;
    [SerializeField] private MusicData musicData;
    [SerializeField] private EnemiesData enemiesData;
    [SerializeField] private WorldGenerationData worldGenerationData;
    [SerializeField] private SpellCastData spellCastData;
    [FormerlySerializedAs("partManagerPrefab")] [SerializeField] private GameplayCanvas gameplayCanvasPrefab;
    [SerializeField] private CinemachineVirtualCamera virtualCameraPrefab;
    [SerializeField] private Canvas loadingCanvas;
    [SerializeField] private GameObject debugCanvas;
    [SerializeField] private SpellCanvas spellCanvas;
    [SerializeField] private SkinPanel shopSkinWindow;
    [SerializeField] private UIGameObjectShower uiGameObjectShowerPrefab;

    private BattleNotifier battleNotifier;
    private GameResourcesCounter gameResourcesCounter;
    private Transform initializedObjectsParent;
    private BuildingSpawner buildingSpawner;
    private GroundBlocksSpawner worldCreator;
    private GameStateMachine gameStateMachine;
    private MusicPlayerMediator musicMediator;
    private SkinChangeDetector skinChangeDetector;
    private RewardSpawner rewardSpawner;
    private IInput input;
    private IDisableableInput disableableInput;
    private bool isSerializationCompleted;
    
    public static Action GameInitialized;

    private void Awake()
    {
        StartCoroutine(StartInitialization());
    }

    private IEnumerator StartInitialization()
    {
        SetupInitializedPartParent();
        SetupLoadingCanvas();
        InitializeInput();
        SetupCoroutineStarter();
        InitializeSkinDetector();
        Tuple<IBackgroundMusicPlayer, IUpgradeEffectPlayer> players = InitializeMusicPlayer();
        IBackgroundMusicPlayer backgroundMusicPlayer = players.Item1;
        IUpgradeEffectPlayer upgradeEffectPlayer = players.Item2;
        backgroundMusicPlayer.StartLoadingMusic();
        InitializeMusicMediator(backgroundMusicPlayer, upgradeEffectPlayer);
        InstantiateWaveCounter();
        SetupBattleNotifier();
        yield return null;
        SpawnResourceCounter();
        SetupResourcesStatistic();
        yield return null;
        SetupUIObjectShower();
        SetupEnemyDeathManager();
        InitializeDialogCamera();
        InitializeCameraMovementComponent();
        yield return null;
        CreateBuildingSpawner();
        var worldGrid = SpawnWorldGrid();
        yield return new WaitUntil(() => worldCreator.IsWorldReady);
        IEnemyDetector knightDetector = SetupEnemyDetector(LayerMask.GetMask(gameData.EnemyLayerName));
        InitializeBuildingSpawner(worldGrid, worldGenerationData.BuildingsData, gameData.UpgradeStateDuration, knightDetector);
        yield return null;
        yield return null;
        GameplayCanvas upgradeCanvas = SpawnUpgradeCanvas();
        yield return null;
        var spellCanvas = SetupSpellCanvas();
        SetupDebugCanvas();
        SetupShopSkinWindow(upgradeCanvas.transform);
        IEnemyDetector gnomeDetector = SetupEnemyDetector(LayerMask.GetMask(gameData.GnomeLayerName));
        yield return null;
        SetupStateMachine(upgradeCanvas, spellCanvas, worldCreator, worldGrid, disableableInput, gnomeDetector);
        battleNotifier.Subscribe();
        SetupRewardSpawner(GemsView.Instance.GemsTextTransform);
        yield return null;
        GameInitialized?.Invoke();
        isSerializationCompleted = true;
        Destroy(loadingCanvas.gameObject);
    }

    private void Update()
    {
        if (isSerializationCompleted == false)
            return;
        
        gameStateMachine.Update();
        gameStateMachine.HandleInput();
        input.Update();
    }

    private void LateUpdate()
    {
        input.LateUpdate();
    }

    private void FixedUpdate()
    {
        if (isSerializationCompleted == false)
            return;
            
        gameStateMachine.PhysicsUpdate();
    }

    private void SetupInitializedPartParent()
    {
        initializedObjectsParent = new GameObject("InitializedObjects").transform;
        initializedObjectsParent.SetParent(transform);
    }

    private void InitializeSkinDetector() => skinChangeDetector = new SkinChangeDetector();

    private void SetupLoadingCanvas() => loadingCanvas = Instantiate(loadingCanvas);
    
    private void SetupUIObjectShower() => Instantiate(uiGameObjectShowerPrefab, Vector3.up * 100, Quaternion.identity);

    private void SetupCoroutineStarter()
    {
        var coroutineStarterTransform = new GameObject("CoroutineStarter").AddComponent<CoroutineStarter>().transform;
        coroutineStarterTransform.SetParent(initializedObjectsParent);
    }

    private Tuple<IBackgroundMusicPlayer, IUpgradeEffectPlayer> InitializeMusicPlayer()
    {
        var musicPlayerGameObject = new GameObject("MusicPlayer");
        musicPlayerGameObject.transform.SetParent(initializedObjectsParent);
        var backgroundPlayer = musicPlayerGameObject.AddComponent<AudioSource>();
        backgroundPlayer.volume = 0.6f;
        var firstUpgradePlayer = musicPlayerGameObject.AddComponent<AudioSource>();
        firstUpgradePlayer.volume = 0.8f;
        var secondUpgradePlayer = musicPlayerGameObject.AddComponent<AudioSource>();
        secondUpgradePlayer.volume = 0.8f;
        var musicPlayer = new GameMusicPlayer(musicData, backgroundPlayer, firstUpgradePlayer, secondUpgradePlayer);
        var allMusicPlayers = new Tuple<IBackgroundMusicPlayer, IUpgradeEffectPlayer>(musicPlayer, musicPlayer);
        return allMusicPlayers;
    }

    private void InitializeMusicMediator(IBackgroundMusicPlayer backgroundMusicPlayer, IUpgradeEffectPlayer upgradeEffectPlayer)
    {
        musicMediator = new MusicPlayerMediator(backgroundMusicPlayer, upgradeEffectPlayer);
        musicMediator.Subscribe();
    }

    private void SetupEnemyDeathManager()
    {
        var enemyDeathManager = new GameObject("EnemyDeathManager").AddComponent<EnemyDeathManager>();
        enemyDeathManager.Initialize();
        enemyDeathManager.transform.SetParent(initializedObjectsParent);
    }

    private void InitializeInput()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            var mobileInput = new MobileInput(gameData.SwipeDeadZone, gameData.ClickMaxTime);
            input = mobileInput;
            disableableInput = mobileInput;
        }
        else
        {
            var initializedInput = new DesctopInput(gameData.SwipeDeadZone, gameData.ClickMaxTime);
            input = initializedInput;
            disableableInput = initializedInput;
        }
    }

    private void SpawnResourceCounter()
    {
        gameResourcesCounter = new GameObject("ResourcesCounter").AddComponent<GameResourcesCounter>();
        gameResourcesCounter.transform.SetParent(initializedObjectsParent);
        gameResourcesCounter.Initialize(gameData.StartFoodAmount, gameData.StartMaterialsAmount, gameData.StartSpiritAmount);
        gameResourcesCounter.SetGnomeDeathFine(gameData.GnomeDeathSpiritFine);
    }

    private void SetupResourcesStatistic()
    {
        new ResourceIncomeCounter(gameResourcesCounter);
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
        cameraMovement.transform.SetParent(initializedObjectsParent);
        cameraMovement.gameObject.name = "CameraMovement";
        cameraMovement.Initialize(input, cameraData);
    }

    private void CreateBuildingSpawner()
    {
        buildingSpawner = new GameObject("BuildingSpawner").AddComponent<BuildingSpawner>();
        buildingSpawner.transform.SetParent(initializedObjectsParent);
    }

    private CellsGrid SpawnWorldGrid()
    {
        worldCreator = new GameObject("WorldCreator").AddComponent<GroundBlocksSpawner>();
        worldCreator.transform.SetParent(initializedObjectsParent);
        var buildingsGrid = new CellsGrid(worldGenerationData.GridSize, worldGenerationData.CellsInterval);
        worldCreator.SetupGrid(worldGenerationData, buildingsGrid, buildingSpawner, this);
        return buildingsGrid;
    }

    private void InitializeBuildingSpawner(CellsGrid grid,  BuildingsData upgradeBuildings, float animationDuration, IEnemyDetector detector)
    {
        buildingSpawner.Initialize(grid, upgradeBuildings, animationDuration, detector);
    }

    private GameplayCanvas SpawnUpgradeCanvas()
    {
        GameplayCanvas upgradeCanvas = Instantiate(gameplayCanvasPrefab);
        upgradeCanvas.Initialize(4, 
            worldGenerationData.BuildingsData.FarmData,
            worldGenerationData.BuildingsData.SpiritBuildingData,
            worldGenerationData.BuildingsData.MilitaryCampData,
            worldGenerationData.BuildingsData.FactoryData
            );
        return upgradeCanvas;
    }
    
    private SkinPanel SetupShopSkinWindow(Transform canvasTransform)
    {
        var shopWindow = Instantiate(shopSkinWindow, canvasTransform);
        shopWindow.Initialize(input);
        return shopWindow;
    }

    private GameObject SetupSpellCanvas()
    {
        var spellCanvasWindow = Instantiate(spellCanvas);
        var spellCastScript = new SpellCast(input, spellCanvasWindow, spellCastData);
        spellCanvasWindow.gameObject.SetActive(false);
        return spellCanvasWindow.gameObject;
    }

    private GameObject SetupDebugCanvas()
    {
        var debugCanvasWidnow = Instantiate(debugCanvas);
        return debugCanvasWidnow.gameObject;
    }
    
    private void InstantiateWaveCounter() => new GameObject("WaveCounter").AddComponent<WaveCounter>().transform.SetParent(initializedObjectsParent);
        
    private void SetupStateMachine(GameplayCanvas gameplayCanvas, GameObject battleStateCanvas, GroundBlocksSpawner worldCreator, 
        CellsGrid grid, IDisableableInput inputForDialogueState, IEnemyDetector detector)
    {
        GameStateMachineData gameStateMachineData = new GameStateMachineData 
        (
            gameplayCanvas,
            gameData,
            worldCreator,
            grid,
            battleStateCanvas,
            gameData.EnemyTag,
            gameData.GnomeTag,
            inputForDialogueState,
            gameData.UpgradeStateDuration,
            gameData.UpgradeStateCompletionDelay,
            detector
        );
        gameStateMachine = new GameStateMachine(gameStateMachineData, enemiesData, gameData.EnemiesSpawnOffset);
    }

    private void SetupBattleNotifier() => battleNotifier = new BattleNotifier();

    private IEnemyDetector SetupEnemyDetector(LayerMask enemyMask) => 
        new UnitDetector(gameData.WorldSize, enemyMask, 1f);

    private void SetupRewardSpawner(RectTransform uiTarget)
    {
        rewardSpawner = new GameObject("RewardSpawner").AddComponent<RewardSpawner>();
        rewardSpawner.transform.SetParent(initializedObjectsParent);
        rewardSpawner.Initialize(gameData.EnemyRewardPrefab, uiTarget, new RewardSpawner.RewardAnimationSettings(1, 1), gameResourcesCounter);
    }

    private void OnDestroy()
    {
        ResourceIncomeCounter.Instance.Unsubscribe();
        skinChangeDetector.Unsubscribe();
        musicMediator.Unsubscribe();
        battleNotifier.Unsubscribe();
    }
}