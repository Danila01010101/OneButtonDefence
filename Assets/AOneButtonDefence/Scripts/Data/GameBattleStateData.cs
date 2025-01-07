using UnityEngine;

public class GameBattleStateData
{
    public IStringStateChanger StateChanger { get; private set; }
    public MonoBehaviour CoroutineStarter { get; private set; }
    public BattleWavesParameters WavesParameters { get; private set; }
    public EnemiesData EnemiesData { get; private set; }
    public CellsGrid CellsGrid { get; private set; }
    public Vector3 EnemiesSpawnOffset { get; private set; }
    public string EnemyTag { get; private set; }
    public string GnomeTag { get; private set; }

    public GameBattleStateData(IStringStateChanger stateChanger, MonoBehaviour coroutineStarter, BattleWavesParameters wavesParameters, EnemiesData data, 
        CellsGrid cellsGrid, Vector3 enemiesSpawnOffset, string enemyLayerMask, string gnomeLayerMask)
    {
        this.StateChanger = stateChanger;
        this.CoroutineStarter = coroutineStarter;
        this.WavesParameters = wavesParameters;
        this.EnemiesData = data;
        this.CellsGrid = cellsGrid;
        this.EnemiesSpawnOffset = enemiesSpawnOffset;
        EnemyTag = enemyLayerMask;
        GnomeTag = gnomeLayerMask;
    }
}