using UnityEngine;
using System.Collections;

public class StateMachineInitializer : IGameInitializerStep
{
    private GameData _gameData;
    private EnemiesData _enemiesData;
    private GameplayCanvas _canvas;
    private GameObject _battleStateCanvas;
    private GroundBlocksSpawner _worldCreator;
    private CellsGrid _grid;
    private IDisableableInput _input;
    private IEnemyDetector _detector;
    public GameStateMachine Instance { get; private set; }

    public StateMachineInitializer(GameData gameData, EnemiesData enemiesData, GameplayCanvas canvas, GameObject battleCanvas, GroundBlocksSpawner worldCreator, CellsGrid grid, IDisableableInput input, IEnemyDetector detector)
    {
        _gameData = gameData;
        _enemiesData = enemiesData;
        _canvas = canvas;
        _battleStateCanvas = battleCanvas;
        _worldCreator = worldCreator;
        _grid = grid;
        _input = input;
        _detector = detector;
    }

    public IEnumerator Initialize()
    {
        GameStateMachine.GameStateMachineData stateData = new GameStateMachine.GameStateMachineData(
            _canvas,
            _gameData,
            _worldCreator,
            _grid,
            _battleStateCanvas,
            _gameData.EnemyTag,
            _gameData.GnomeTag,
            _input,
            _gameData.UpgradeStateDuration,
            _gameData.UpgradeStateCompletionDelay,
            _detector
        );
        Instance = new GameStateMachine(stateData, _enemiesData, _gameData.EnemiesSpawnOffset, LayerMask.GetMask(_gameData.EnemyLayerName));
        
        yield break;
    }
}