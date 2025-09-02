using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameBattleState : IState
{
    private IStringStateChanger stateMachine;
    private MonoBehaviour coroutineStarter;
    private BattleWavesParameters wavesParameters;
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
        wavesParameters = data.WavesParameters;
        enemiesSpawnOffset = data.EnemiesSpawnOffset;
        enemyTag = data.EnemyTag;
        gnomeTag = data.GnomeTag;
        grid = data.CellsGrid;
        unitsFactory = new UnitsFactory(data.EnemiesData.enemies, data.Detector, data.EnemyLayer, data.EnemyTag);
        spellCanvas = data.SpellCanvas;
        AsyncHelper.Instance.RunAsyncWithResult(() => WaveGenerator.GenerateWaves(data.WavesParameters, 100), result => wavesParameters = result);
        //CoroutineStarter.StartCoroutine(LevelGenerationClass.GenerateNewLevels(data.WavesParameters, 100, out newParameters));
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
        BattleWavesParameters.WaveData waveParameters = wavesParameters.waves[currentWaveIndex];
        spawnCoroutine = coroutineStarter.StartCoroutine(StartEnemiesSpawn(waveParameters));
    }
    
    private IEnumerator StartEnemiesSpawn(BattleWavesParameters.WaveData waveData)
    {
        foreach (var enemyData in waveData.enemiesToSpawn)
        {
            for (int i = 0; i < enemyData.Amount; i++)
            {
                yield return new WaitForSeconds(waveData.spawnInterval);
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