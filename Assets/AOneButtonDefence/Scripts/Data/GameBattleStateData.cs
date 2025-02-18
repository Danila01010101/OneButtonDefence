using UnityEngine;

public class GameBattleStateData
{
    public IStringStateChanger StateChanger { get; private set; }
    public MonoBehaviour CoroutineStarter { get; private set; }
    public BattleWavesParameters WavesParameters { get; private set; }
    public EnemiesData EnemiesData { get; private set; }
    public CellsGrid CellsGrid { get; private set; }
    public GameObject SpellCanvas { get; private set; }
    public Vector3 EnemiesSpawnOffset { get; private set; }
    public string EnemyTag { get; private set; }
    public string GnomeTag { get; private set; }
    public IEnemyDetector Detector { get; private set; }

    public GameBattleStateData(IStringStateChanger stateChanger, MonoBehaviour coroutineStarter, BattleWavesParameters wavesParameters, EnemiesData data, 
        CellsGrid cellsGrid, GameObject spellCanvas, Vector3 enemiesSpawnOffset, string enemyLayerMask, string gnomeLayerMask, IEnemyDetector gnomeDetector)
    {
        StateChanger = stateChanger;
        CoroutineStarter = coroutineStarter;
        WavesParameters = wavesParameters;
        EnemiesData = data;
        CellsGrid = cellsGrid;
        SpellCanvas = spellCanvas;
        EnemiesSpawnOffset = enemiesSpawnOffset;
        EnemyTag = enemyLayerMask;
        GnomeTag = gnomeLayerMask;
        Detector = gnomeDetector;
    }
}