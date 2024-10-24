using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : StateMachine, IStringStateChanger
{
    private Dictionary<string, IState> stringStates;

    public GameStateMachine(GameStateMachineData data, EnemiesData enemies, Vector3 enemySpawnOffset)
    {
        GameBattleStateData battleStateData = new GameBattleStateData(this, data.CoroutineStarter, data.GameTurnsData.BattleWavesParameters, enemies, data.CellsGrid, enemySpawnOffset);

        stringStates = new Dictionary<string, IState>()
        {
            { GameStateNames.StartDialog, new DialogState(this, data.GameTurnsData.StartDialogCanvas) },
            //{ GameStateNames.DragonDialog, new DialogState(this, gameData.EndTurnDialogCanvas) },
            { GameStateNames.BattleState, new GameBattleState(battleStateData) },
            { GameStateNames.Upgrade, new UpgradeState(this, data.UpgradeUIGameobject) }
        };

        ChangeStateWithString(GameStateNames.StartDialog);
    }
    
    public void ChangeStateWithString(string stateName) => ChangeState(stringStates[stateName]);

    public class GameStateMachineData
    {
        public PartManager UpgradeUIGameobject { get; private set; }
        public GameData GameTurnsData { get; private set; }
        public MonoBehaviour CoroutineStarter { get; private set; }
        public CellsGrid CellsGrid { get; private set; }

        public GameStateMachineData(PartManager upgradeUIGameobject, GameData gameTurnsData, MonoBehaviour coroutineStarter, CellsGrid buildingsGrid)
        {
            this.UpgradeUIGameobject = upgradeUIGameobject;
            this.GameTurnsData = gameTurnsData;
            this.CoroutineStarter = coroutineStarter;
            this.CellsGrid = buildingsGrid;
        }
    }
}