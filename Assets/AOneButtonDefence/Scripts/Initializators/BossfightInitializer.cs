using System;
using System.Collections;
using System.Collections.Generic;
using AOneButtonDefence.Scripts;
using UnityEngine;

public class BossfightInitializer : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private GameData gameData;
    [SerializeField] private EnemiesData enemiesData;
    [SerializeField] private SpellCastData spellCastData;
    [SerializeField] private CameraData cameraData;
    [SerializeField] private MusicData musicData;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private SpellCanvas spellCanvas;
    [SerializeField] private Vector3 bossSpawnPoint;

    [Header("Prefabs")]
    [SerializeField] private Cinemachine.CinemachineVirtualCamera virtualCameraPrefab;
    [SerializeField] private SpellCanvas spellCanvasPrefab;

    private IInput input;
    private IDisableableInput disableableInput;
    private SpellCanvas spellCanvasInstance;
    private IBackgroundMusicPlayer backgroundMusic;
    private IUpgradeEffectPlayer upgradeEffectPlayer;
    private object playerInitializerObj;
    private readonly List<IDisposable> disposables = new List<IDisposable>();
    public static Action OnBossSceneReady;
    private bool initialized;

    private void Awake()
    {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        var initializedObjectsParent = new GameObject("Spawned Objects").transform;
        initializedObjectsParent.position = Vector3.zero;
        
        CoroutineStarterInitializer coroutineStarterInit = new CoroutineStarterInitializer(initializedObjectsParent);
        yield return coroutineStarterInit.Initialize();
        
        var inputInit = new InputInitializer(gameData);
        yield return inputInit.Initialize();
        input = inputInit.Input;
        disableableInput = inputInit.DisableableInput;
        
        var musicInit = new MusicPlayerInitializer(transform, musicData);
        yield return musicInit.Initialize();
        backgroundMusic = musicInit.BackgroundPlayer;
        upgradeEffectPlayer = musicInit.UpgradeEffectPlayer;
        backgroundMusic?.StartLoadingMusic();
        
        var startAudioSources = new List<AudioSource>();
        
        var bgSource = backgroundMusic?.GetSource();
        if (bgSource != null) startAudioSources.Add(bgSource);
        
        VolumeChangerInitializer volumeChangerInit = new VolumeChangerInitializer(null, startAudioSources, musicData.StartValue);
        yield return volumeChangerInit.Initialize();

        var mediator = new MusicMediatorInitializer(backgroundMusic, upgradeEffectPlayer);
        yield return mediator.Initialize();
        if (mediator is IDisposable md) disposables.Add(md);

        SpellCanvasInitializer spellCanvasInit = new SpellCanvasInitializer(spellCanvas, input, spellCastData);
        disposables.Add(spellCanvasInit);

        var battleEvents = new BattleEventsForBossFight();

        var playerData = new PlayerControllerInitializer.PlayerControllerInitializerData(
            gameData.PlayerUnit,
            gameData.PlayerUnitData,
            Camera.main,
            playerSpawnPoint.transform.position,
            battleEvents
        );

        var playerInitializer = new PlayerControllerInitializer(playerData);
        playerInitializerObj = playerInitializer;
        yield return playerInitializer.Initialize(spellCanvasInit);
        disposables.Add(playerInitializer);
        yield return spellCanvasInit.Initialize();
        spellCanvasInstance = spellCanvasInit.Instance.GetComponent<SpellCanvas>();
        
        DialogCameraInitializer dialogCameraInit = new DialogCameraInitializer(cameraData);
        yield return dialogCameraInit.Initialize();
        
        CameraMovementInitializer cameraMovementInit = new CameraMovementInitializer(virtualCameraPrefab, initializedObjectsParent, input, cameraData);
        yield return cameraMovementInit.Initialize();
        
        IEnemyDetector gnomeDetector = null;
        gnomeDetector = new UnitDetector(gameData.WorldSize, LayerMask.GetMask(gameData.GnomeLayerName), 1f, gameData.DefaultStoppingDistance);

        var bossFightStateMachineData = new BossFightStateMachine.BossFightStateMachineData(gameData, input as IDisableableInput, spellCanvasInit.Instance, enemiesData, 
            gameData.EnemyTag, gnomeDetector, LayerMask.GetMask(gameData.EnemyLayerName), bossSpawnPoint);
        BossFightStateMachine stateMachine = new BossFightStateMachine(bossFightStateMachineData);

        initialized = true;
        OnBossSceneReady?.Invoke();
    }

    private void Update()
    {
        if (!initialized) return;
        input?.Update();
    }

    private void LateUpdate()
    {
        input?.LateUpdate();
    }

    private void OnDestroy()
    {
        foreach (var d in disposables)
        {
            try { d.Dispose(); } catch { }
        }

        if (playerInitializerObj is IDisposable pi) 
        {
            try { pi.Dispose(); } catch { }
        }
    }
}
