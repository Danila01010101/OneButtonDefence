using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : StateMachine, IStringStateChanger
{
    private Dictionary<string, IState> stringStates;

    public GameStateMachine(GameStateMachineData data, EnemiesData enemies, Vector3 enemySpawnOffset)
    {
        GameBattleStateData battleStateData = new GameBattleStateData(
            this, data.CoroutineStarter, data.GameTurnsData.BattleWavesParameters, enemies, data.CellsGrid, enemySpawnOffset, data.EnemyTag, data.GnomeTag);

        stringStates = new Dictionary<string, IState>()
        {
            { GameStateNames.StartDialog, new DialogState(this, data.GameTurnsData.StartDialogCanvas, GameStateNames.Upgrade, data.Input, true) },
            { GameStateNames.WinDialogue, new DialogState(this, data.GameTurnsData.EndTurnWinDialogCanvas, GameStateNames.Upgrade, data.Input) },
            { GameStateNames.BattleLoseDialogue, new DialogState(this, data.GameTurnsData.BattleLoseDialogCanvas, GameStateNames.Reload, data.Input) },
            { GameStateNames.SpiritLoseDialogue, new DialogState(this, data.GameTurnsData.SpiritLoseDialogCanvas, GameStateNames.Reload, data.Input) },
            { GameStateNames.ResourcesLoseDialogue, new DialogState(this, data.GameTurnsData.ResourceLoseDialogCanvas, GameStateNames.Reload, data.Input) },
            { GameStateNames.FoodLoseDialogue, new DialogState(this, data.GameTurnsData.FoodLoseDialogCanvas, GameStateNames.Reload, data.Input) },
            { GameStateNames.Reload, new ReloadingState() },
            //{ GameStateNames.DragonDialog, new DialogState(this, gameData.EndTurnDialogCanvas) },
            { GameStateNames.BattleState, new GameBattleState(battleStateData) },
            { GameStateNames.Upgrade, new UpgradeState(this, data.UpgradeUIGameObject, data.UpgradeStateDuration, data.UpgradeStateCompletionDelay) }
        };

        ChangeStateWithString(GameStateNames.StartDialog);
    }
    
    public void ChangeStateWithString(string stateName) => ChangeState(stringStates[stateName]);

    public class GameStateMachineData
    {
        public readonly PartManager UpgradeUIGameObject;
        public readonly GameData GameTurnsData;
        public readonly MonoBehaviour CoroutineStarter;
        public readonly CellsGrid CellsGrid;
        public readonly float UpgradeStateCompletionDelay;
        public readonly float UpgradeStateDuration;
        public readonly string EnemyTag;
        public readonly string GnomeTag;
        public readonly IDisableableInput Input;

        public GameStateMachineData(PartManager upgradeUIGameObject, GameData gameTurnsData, MonoBehaviour coroutineStarter, CellsGrid buildingsGrid,
            string enemyTag, string gnomeTag, IDisableableInput input, float upgradeStateDuration, float upgradeStateCompletionDelay)
        {
            UpgradeUIGameObject = upgradeUIGameObject;
            GameTurnsData = gameTurnsData;
            CoroutineStarter = coroutineStarter;
            CellsGrid = buildingsGrid;
            EnemyTag = enemyTag;
            GnomeTag = gnomeTag;
            Input = input;
            UpgradeStateDuration = upgradeStateDuration;
            UpgradeStateCompletionDelay = upgradeStateCompletionDelay;
        }
    }
}