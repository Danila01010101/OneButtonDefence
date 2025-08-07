using System.Collections;
using UnityEngine;
using static GameStateMachine;

public class StateMachineInitializer : MonoBehaviour, IGameComponentInitializer
{
    [SerializeField] private GameData gameData;
    [SerializeField] private EnemiesData enemiesData;

    private GameStateMachine _stateMachine;

    public IEnumerator Initialize()
    {
        var gnomeDetector = new UnitDetector(gameData.WorldSize, LayerMask.GetMask(gameData.GnomeLayerName), 1f, gameData.DefaultStoppingDistance);

        var data = new GameStateMachineData
        (
            CanvasInitializer.GameplayCanvas,
            gameData,
            WorldInitializer.WorldCreator,
            WorldInitializer.Grid,
            SpellCanvasInitializer.SpellCanvas,
            gameData.EnemyTag,
            gameData.GnomeTag,
            InputInitializer.DisableableInput,
            gameData.UpgradeStateDuration,
            gameData.UpgradeStateCompletionDelay,
            gnomeDetector
        );

        _stateMachine = new GameStateMachine(data, enemiesData, gameData.EnemiesSpawnOffset, LayerMask.GetMask(gameData.EnemyLayerName));

        yield return null;
    }

    private void Update() => _stateMachine?.Update();
    private void FixedUpdate() => _stateMachine?.PhysicsUpdate();
    private void LateUpdate() => InputInitializer.Input?.LateUpdate();
}