using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : StateMachine, IStringStateChanger
{
    private Dictionary<string, IState> stringStates;

    public GameStateMachine(GameStateMachineData data, EnemiesData enemies, Vector3 enemySpawnOffset, float upgradeStateDuration)
    {
        GameBattleStateData battleStateData = new GameBattleStateData(
            this, data.CoroutineStarter, data.GameTurnsData.BattleWavesParameters, enemies, data.CellsGrid, enemySpawnOffset, data.EnemyTag, data.GnomeTag);

        stringStates = new Dictionary<string, IState>()
        {
            { GameStateNames.StartDialog, new DialogState(this, data.GameTurnsData.StartDialogCanvas, GameStateNames.Upgrade) },
            { GameStateNames.WinDialogue, new DialogState(this, data.GameTurnsData.EndTurnWinDialogCanvas, GameStateNames.Upgrade) },
            { GameStateNames.BattleLoseDialogue, new DialogState(this, data.GameTurnsData.BattleLoseDialogCanvas, GameStateNames.Reload) },
            { GameStateNames.SpiritLoseDialogue, new DialogState(this, data.GameTurnsData.SpiritLoseDialogCanvas, GameStateNames.Reload) },
            { GameStateNames.ResourcesLoseDialogue, new DialogState(this, data.GameTurnsData.ResourceLoseDialogCanvas, GameStateNames.Reload) },
            { GameStateNames.Reload, new ReloadingState() },
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
        public GameData GameTurnsData { get; private set; }
        public MonoBehaviour CoroutineStarter { get; private set; }
        public CellsGrid CellsGrid { get; private set; }
        public string EnemyTag { get; private set; }
        public string GnomeTag { get; private set; }

        public GameStateMachineData(PartManager upgradeUIGameobject, GameData gameTurnsData, MonoBehaviour coroutineStarter, CellsGrid buildingsGrid, string enemyTag, string gnomeTag)
        {
            this.UpgradeUIGameobject = upgradeUIGameobject;
            this.GameTurnsData = gameTurnsData;
            this.CoroutineStarter = coroutineStarter;
            this.CellsGrid = buildingsGrid;
            this.EnemyTag = enemyTag;
            this.GnomeTag = gnomeTag;
        }
    }
}