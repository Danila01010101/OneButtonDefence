using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameBattleState : IState
{
    private IStateChanger stateMachine;
    private MonoBehaviour coroutineStarter;
    private BattleWavesParameters wavesParameters;
    private EnemieFactory enemieFactory;
    private Coroutine spawnCoroutine;
    private CellsGrid grid;
    private Vector3 enemiesSpawnOffset;
    private int spawnSpread = 2;
    private int currentWaveIndex = 0;

    public GameBattleState(GameBattleStateData data)
    {
        this.stateMachine = data.StateChanger;
        this.coroutineStarter = data.CoroutineStarter;
        this.wavesParameters = data.WavesParameters;
        this.enemiesSpawnOffset = data.EnemiesSpawnOffset;
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

        currentWaveIndex++;
    }
}