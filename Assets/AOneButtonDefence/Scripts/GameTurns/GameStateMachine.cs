using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : StateMachine, IStringStateChanger
{
    private Dictionary<string, IState> stringStates;

    public GameStateMachine(GameStateMachineData data, EnemiesData enemies, Vector3 enemySpawnOffset, LayerMask enemyLayer)
    {
        GameBattleStateData battleStateData = new GameBattleStateData( this, data.CoroutineStarter, data.GameTurnsData.BattleWavesParameters, 
            enemies, data.CellsGrid, data.SpellCanvas, enemySpawnOffset, data.EnemyTag, data.GnomeTag, data.Detector, enemyLayer);

        stringStates = new Dictionary<string, IState>()
        {
            { GameStateNames.StartDialog, new DialogState(this, data.UpgradeUIGameObject.ResourceInfo, data.GameTurnsData.StartDialogCanvas, GameStateNames.Upgrade, data.Input, true) },
            { GameStateNames.WinDialogue, new DialogState(this, data.UpgradeUIGameObject.ResourceInfo, data.GameTurnsData.EndTurnWinDialogCanvas, GameStateNames.Upgrade, data.Input) },
            { GameStateNames.BattleLoseDialogue, new DialogState(this, data.UpgradeUIGameObject.ResourceInfo, data.GameTurnsData.BattleLoseDialogCanvas, GameStateNames.Reload, data.Input) },
            { GameStateNames.SpiritLoseDialogue, new DialogState(this, data.UpgradeUIGameObject.ResourceInfo, data.GameTurnsData.SpiritLoseDialogCanvas, GameStateNames.Reload, data.Input) },
            { GameStateNames.ResourcesLoseDialogue, new DialogState(this, data.UpgradeUIGameObject.ResourceInfo, data.GameTurnsData.ResourceLoseDialogCanvas, GameStateNames.Reload, data.Input) },
            { GameStateNames.FoodLoseDialogue, new DialogState(this, data.UpgradeUIGameObject.ResourceInfo, data.GameTurnsData.FoodLoseDialogCanvas, GameStateNames.Reload, data.Input) },
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
        public readonly GameplayCanvas UpgradeUIGameObject;
        public readonly GameData GameTurnsData;
        public readonly MonoBehaviour CoroutineStarter;
        public readonly CellsGrid CellsGrid;
        public readonly GameObject SpellCanvas;
        public readonly float UpgradeStateCompletionDelay;
        public readonly float UpgradeStateDuration;
        public readonly string EnemyTag;
        public readonly string GnomeTag;
        public readonly IDisableableInput Input;
        public readonly IEnemyDetector Detector;

        public GameStateMachineData(GameplayCanvas upgradeUIGameObject, GameData gameTurnsData, MonoBehaviour coroutineStarter, CellsGrid buildingsGrid,
           GameObject spellCanvas, string enemyTag, string gnomeTag, IDisableableInput input, float upgradeStateDuration, float upgradeStateCompletionDelay, IEnemyDetector detector)
        {
            UpgradeUIGameObject = upgradeUIGameObject;
            GameTurnsData = gameTurnsData;
            CoroutineStarter = coroutineStarter;
            CellsGrid = buildingsGrid;
            SpellCanvas = spellCanvas;
            EnemyTag = enemyTag;
            GnomeTag = gnomeTag;
            Input = input;
            UpgradeStateDuration = upgradeStateDuration;
            UpgradeStateCompletionDelay = upgradeStateCompletionDelay;
            Detector = detector;
        }
    }
}