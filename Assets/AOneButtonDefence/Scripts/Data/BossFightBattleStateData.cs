using UnityEngine;

public class BossFightBattleStateData
{
    public EnemiesData EnemiesData { get; private set; }
    public LayerMask EnemyLayer { get; private set; }
    public IEnemyDetector Detector { get; private set; }
    public string EnemyTag { get; private set; }
    public GameObject BattleCanvas { get; private set; }
    public Vector3 DragonSpawnPosition { get; private set; }

    public BossFightBattleStateData(GameObject battleCanvas, EnemiesData data, string enemyLayerMask, IEnemyDetector gnomeDetector, LayerMask enemyLayer, Vector3 dragonSpawnPosition)
    {
        BattleCanvas = battleCanvas;
        EnemyTag = enemyLayerMask;
        Detector = gnomeDetector;
        EnemyLayer = enemyLayer;
        EnemiesData = data;
        DragonSpawnPosition = dragonSpawnPosition;
    }
}
