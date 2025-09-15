using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameBattleState : IState
{
    private IStringStateChanger stateMachine;
    private MonoBehaviour coroutineStarter;
    private WaveGenerator.RuntimeWavesParameters runtimeWavesParameters;
    private UnitsFactory unitsFactory;
    private Coroutine endTurnCheckCoroutine;
    private Coroutine spawnCoroutine;
    private CellsGrid grid;
    private GameObject spellCanvas;
    private Vector3 enemiesSpawnOffset;
    private int spawnSpread = 2;
    private int currentWaveIndex = 0;
    private string enemyTag;
    private string gnomeTag;

    public static Action BattleStarted;
    public static Action EnemiesDefeated;
    public static Action BattleWon;
    public static Action BattleLost;
    
    public GameBattleState(GameBattleStateData data)
    {
        stateMachine = data.StateChanger;
        coroutineStarter = data.CoroutineStarter;
        enemiesSpawnOffset = data.EnemiesSpawnOffset;
        enemyTag = data.EnemyTag;
        gnomeTag = data.GnomeTag;
        grid = data.CellsGrid;
        unitsFactory = new UnitsFactory(data.EnemiesData.enemies, data.Detector, data.EnemyLayer, data.EnemyTag);
        spellCanvas = data.SpellCanvas;

        List<BattleWavesParameters.EnemyGenerationData> enemyConfigs = (data.WavesParameters.waves.Count > 0) 
            ? data.WavesParameters.waves[0].enemiesToGenerate 
            : new List<BattleWavesParameters.EnemyGenerationData>();

        AsyncHelper.Instance.RunAsyncWithResult(
            () => WaveGenerator.GenerateWaves(data.WavesParameters, 100),
            result =>
            {
                runtimeWavesParameters = result;

                for (int i = 0; i < Mathf.Min(5, runtimeWavesParameters.Waves.Count); i++)
                {
                    var wave = runtimeWavesParameters.Waves[i];
                    Debug.Log($"Wave {i + 1}, interval: {wave.SpawnInterval}");
                    foreach (var enemy in wave.EnemiesToSpawn)
                    {
                        Debug.Log($"Enemy: {enemy.EnemyPrefab.name}, Amount: {enemy.Amount}, Interval: {enemy.SpawnInterval}");
                    }
                }
            },
            error =>
            {
                Debug.LogError("Ошибка генерации волн: " + error);
            }
        );
    }

    public void Enter()
    {
        BattleStarted?.Invoke();
        StartWave();
        spellCanvas.SetActive(true);
        PlayerController.PlayerDead += LoseBattle;
    }

    public void Exit()
    {
        spellCanvas.SetActive(false);
        
        if (spawnCoroutine != null)
        {
            coroutineStarter.StopCoroutine(spawnCoroutine);
        }

        if (endTurnCheckCoroutine != null)
        {
            coroutineStarter.StopCoroutine(endTurnCheckCoroutine);
        }
        
        PlayerController.PlayerDead -= LoseBattle;
    }

    public void HandleInput() { }

    public void OnAnimationEnterEvent() { }

    public void OnAnimationExitEvent() { }

    public void OnAnimationTransitionEvent() { }

    public void OnTriggerEnter(Collider collider) { }

    public void OnTriggerExit(Collider collider) { }

    public void PhysicsUpdate() { }

    public void Update() { }

    private void StartWave()
    {
        WaveGenerator.RuntimeWaveData waveParameters = runtimeWavesParameters.Waves[currentWaveIndex];
        spawnCoroutine = coroutineStarter.StartCoroutine(StartEnemiesSpawn(waveParameters));
    }
    
    private IEnumerator StartEnemiesSpawn(WaveGenerator.RuntimeWaveData waveData)
    {
        foreach (var enemyData in waveData.EnemiesToSpawn)
        {
            for (int i = 0; i < enemyData.Amount; i++)
            {
                yield return new WaitForSeconds(waveData.SpawnInterval);
                unitsFactory.SpawnSpecificUnit(enemyData.EnemyPrefab, grid.GetRandomEmptyCellPosition(spawnSpread) + enemiesSpawnOffset);
            }
        }

        endTurnCheckCoroutine = coroutineStarter.StartCoroutine(EndStateChecking());
        currentWaveIndex++;
    }


    private IEnumerator EndStateChecking()
    {
        List<GameObject> knightUnits = GameObject.FindGameObjectsWithTag(enemyTag).ToList();
        List<GameObject> gnomeUnits = GameObject.FindGameObjectsWithTag(gnomeTag).ToList();

        while (true)
        {
            yield return new WaitForEndOfFrame();
            EnemiesDefeated?.Invoke();
            yield return new WaitForSeconds(1.75f);

            if (knightUnits.Count > 0)
            {
                CheckAndClearUnits(knightUnits);
            }
            else
            {
                WinBattle();
            }

            if (gnomeUnits.Count > 0)
            {
                CheckAndClearUnits(gnomeUnits);
            }
            else
            {
                LoseBattle();
            }
        }
    }

    private void LoseBattle()
    {
        BattleLost?.Invoke();
        stateMachine.ChangeStateWithString(GameStateNames.BattleLoseDialogue);
    }

    private void WinBattle()
    {
        WaveCounter.Instance.EndWave();
        BattleWon?.Invoke();
        stateMachine.ChangeStateWithString(GameStateNames.WinDialogue);
    }

    private void CheckAndClearUnits(List<GameObject> units)
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] == null)
            {
                units.Remove(units[i]);
            }
        }
    }
}