using System.Collections.Generic;
using UnityEngine;

public class BossFightStateMachine : StateMachine, IStringStateChanger
{
    private Dictionary<string, IState> stringStates;

    public BossFightStateMachine(BossFightStateMachineData data)
    {
        BossFightBattleStateData bossFightBattleStateData = new BossFightBattleStateData(data.BattleCanvas, data.EnemiesData, data.EnemyLayerMask, data.EnemyDetector, data.EnemyLayer, data.DragonSpawnPosition);
        
        stringStates = new Dictionary<string, IState>()
        {
            { GameStateNames.StartDialog, new DialogState(this, null, data.GameTurnsData.StartDragonDialogCanvas, GameStateNames.BattleState, data.Input, null, false) },
            { GameStateNames.Reload, new ReloadingState() },
            { GameStateNames.BattleState, new BossFightBattleState(bossFightBattleStateData) },
        };
        
        ChangeStateWithString(GameStateNames.StartDialog);
    }
    
    public void ChangeStateWithString(string stateName) => ChangeState(stringStates[stateName]);

    public class BossFightStateMachineData
    {
        public readonly GameData GameTurnsData;
        public readonly EnemiesData EnemiesData;
        public readonly IDisableableInput Input;
        public readonly GameObject BattleCanvas;
        public readonly LayerMask EnemyLayer;
        public readonly string EnemyLayerMask;
        public readonly IEnemyDetector EnemyDetector;
        public readonly Vector3 DragonSpawnPosition;

        public BossFightStateMachineData(GameData gameTurnsData, IDisableableInput input, GameObject battleCanvas, EnemiesData emiesData,
            string enemyLayerMask, IEnemyDetector enemyDetector, LayerMask enemyLayer, Vector3 dragonSpawnPosition)
        {
            GameTurnsData = gameTurnsData;
            Input = input;
            BattleCanvas = battleCanvas;
            EnemiesData = emiesData;
            EnemyLayerMask = enemyLayerMask;
            EnemyDetector = enemyDetector;
            EnemyLayer = enemyLayer;
            DragonSpawnPosition = dragonSpawnPosition;
        }
    }
}
