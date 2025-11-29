using System;
using System.Collections;
using System.Collections.Generic;
using AOneButtonDefence.Scripts;
using UnityEngine;

public class BossfightInitializer : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private GameData gameData;
    [SerializeField] private SpellCastData spellCastData;
    [SerializeField] private CameraData cameraData;
    [SerializeField] private MusicData musicData;
    [SerializeField] private Transform playerSpawnPoint;

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
        var inputInit = new InputInitializer(gameData);
        yield return inputInit.Initialize();
        input = inputInit.Input;
        disableableInput = inputInit.DisableableInput;
        
        var musicInit = new MusicPlayerInitializer(transform, musicData);
        yield return musicInit.Initialize();
        backgroundMusic = musicInit.BackgroundPlayer;
        upgradeEffectPlayer = musicInit.UpgradeEffectPlayer;
        backgroundMusic?.StartLoadingMusic();

        var mediator = new MusicMediatorInitializer(backgroundMusic, upgradeEffectPlayer);
        yield return mediator.Initialize();
        if (mediator is IDisposable md) disposables.Add(md);

        var spellInit = new SpellCanvasInitializer(spellCanvasPrefab, input, spellCastData);
        yield return spellInit.Initialize();
        spellCanvasInstance = spellInit.Instance.GetComponent<SpellCanvas>();

        var battleEvents = new BattleEvents(BossFightBattleState.BattleStarted, BossFightBattleState.EnemiesDefeated);
        disposables.Add(battleEvents);

        var playerData = new PlayerControllerInitializer.PlayerControllerInitializerData(
            gameData.PlayerUnit,
            gameData.PlayerUnitData,
            Camera.main,
            playerSpawnPoint.transform.position,
            battleEvents
        );

        var playerInitializer = new PlayerControllerInitializer(playerData);
        playerInitializerObj = playerInitializer;
        yield return playerInitializer.Initialize(spellInit);
        disposables.Add(playerInitializer);
        
        CameraMovementInitializer cameraMovementInit = new CameraMovementInitializer(virtualCameraPrefab, null, input, cameraData);
        yield return cameraMovementInit.Initialize();
        
        DialogCameraInitializer dialogCameraInit = new DialogCameraInitializer(cameraData);
        yield return dialogCameraInit.Initialize();

        var bossFightStateMachineData = new BossFightStateMachine.BossFightStateMachineData(gameData, input as IDisableableInput);
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
