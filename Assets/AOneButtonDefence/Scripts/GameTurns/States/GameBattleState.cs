using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameBattleState : IState
{
    private IStringStateChanger stateMachine;
    private MonoBehaviour coroutineStarter;
    private BattleWavesParameters wavesParameters;
    private EnemieFactory enemieFactory;
    private Coroutine spawnCoroutine;
    private CellsGrid grid;
    private Vector3 enemiesSpawnOffset;
    private int spawnSpread = 2;
    private int currentWaveIndex = 0;
    private string enemyTag;
    private string gnomeTag;

    public GameBattleState(GameBattleStateData data)
    {
        this.stateMachine = data.StateChanger;
        this.coroutineStarter = data.CoroutineStarter;
        this.wavesParameters = data.WavesParameters;
        this.enemiesSpawnOffset = data.EnemiesSpawnOffset;
        enemyTag = data.EnemyTag;
        gnomeTag = data.GnomeTag;
        grid = data.CellsGrid;
        enemieFactory = new EnemieFactory(data.EnemiesData);
    }

    public void Enter() => StartWave();

    public void Exit()
    {
        if (spawnCoroutine != null)
        {
            coroutineStarter.StopCoroutine(spawnCoroutine);
        }
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
        for (int i = 0; i < waveData.amountOfEnemySpawns; i++)
        {
            yield return new WaitForSeconds(waveData.spawnInterval);

            for (int enemiesAmount = 0; enemiesAmount < waveData.amountOfEnemySpawns; enemiesAmount++)
            {
                enemieFactory.SpawnEnemy<FightingUnit>(grid.GetRandomEmptyCellPosition(spawnSpread) + enemiesSpawnOffset);
            }
        }

        coroutineStarter.StartCoroutine(EndStateChecking());
        currentWaveIndex++;
    }

    private IEnumerator EndStateChecking()
    {
        List<GameObject> knightUnits = GameObject.FindGameObjectsWithTag(enemyTag).ToList();
        List<GameObject> gnomeUnits = GameObject.FindGameObjectsWithTag(gnomeTag).ToList();

        while (knightUnits.Count > 0 || knightUnits.Count > 0)
        {
            yield return new WaitForEndOfFrame();

            if (knightUnits.Count > 0)
            {
                CheckAndClearUnits(knightUnits);
            }
            else
            {
                stateMachine.ChangeStateWithString(GameStateNames.WinDialogue);
            }

            if (gnomeUnits.Count > 0)
            {
                CheckAndClearUnits(gnomeUnits);
            }
            else
            {
                stateMachine.ChangeStateWithString(GameStateNames.LoseDialogue);
            }
        }
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