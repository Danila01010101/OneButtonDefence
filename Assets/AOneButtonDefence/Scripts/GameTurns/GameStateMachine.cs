using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : StateMachine, IStringStateChanger
{
    private Dictionary<string, IState> stringStates;

    public GameStateMachine(GameStateMachineData data, EnemiesData enemies, Vector3 enemySpawnOffset, float upgradeStateDuration)
    {
        GameBattleStateData battleStateData = new GameBattleStateData(this, data.CoroutineStarter, data.GameTurnsData.BattleWavesParameters, enemies, data.CellsGrid, enemySpawnOffset, data.EnemyTag);

        stringStates = new Dictionary<string, IState>()
        {
            { GameStateNames.StartDialog, new DialogState(this, data.GameTurnsData.StartDialogCanvas) },
            //{ GameStateNames.DragonDialog, new DialogState(this, gameData.EndTurnDialogCanvas) },
            { GameStateNames.BattleState, new GameBattleState(battleStateData) },
            { GameStateNames.Upgrade, new UpgradeState(this, data.UpgradeUIGameobject, upgradeStateDuration) }
        };

        ChangeStateWithString(GameStateNames.StartDialog);
    }
    
    public void ChangeStateWithString(string stateName) => ChangeState(stringStates[stateName]);

    public class GameStateMachineData
    {
        public PartManager UpgradeUIGameobject { get; private set; }
        public GameStartData GameTurnsData { get; private set; }
        public MonoBehaviour CoroutineStarter { get; private set; }
        public CellsGrid CellsGrid { get; private set; }
        public string EnemyTag { get; private set; }

        public GameStateMachineData(PartManager upgradeUIGameobject, GameStartData gameTurnsData, MonoBehaviour coroutineStarter, CellsGrid buildingsGrid, string enemyTag)
        {
            this.UpgradeUIGameobject = upgradeUIGameobject;
            this.GameTurnsData = gameTurnsData;
            this.CoroutineStarter = coroutineStarter;
            this.CellsGrid = buildingsGrid;
            this.EnemyTag = enemyTag;
        }
    }
}