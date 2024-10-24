using UnityEngine;

public class GameBattleStateData
{
    public IStateChanger StateChanger { get; private set; }
    public MonoBehaviour CoroutineStarter { get; private set; }
    public BattleWavesParameters WavesParameters { get; private set; }
    public EnemiesData EnemiesData { get; private set; }
    public CellsGrid CellsGrid { get; private set; }
    public Vector3 EnemiesSpawnOffset { get; private set; }

    public GameBattleStateData(IStateChanger stateChanger, MonoBehaviour coroutineStarter, BattleWavesParameters wavesParameters, EnemiesData data, CellsGrid cellsGrid, Vector3 enemiesSpawnOffset)
    {
        this.StateChanger = stateChanger;
        this.CoroutineStarter = coroutineStarter;
        this.WavesParameters = wavesParameters;
        this.EnemiesData = data;
        this.CellsGrid = cellsGrid;
        this.EnemiesSpawnOffset = enemiesSpawnOffset;
    }
}