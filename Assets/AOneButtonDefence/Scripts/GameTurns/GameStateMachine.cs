using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : StateMachine, IStringStateChanger
{
    private Dictionary<string, IState> stringStates;

    public GameStateMachine(GameStateMachineData data)
    {
        stringStates = new Dictionary<string, IState>()
        {
            { GameStateNames.StartDialog, new DialogState(this, data.GameTurnsData.StartDialogCanvas) },
            //{ GameStateNames.DragonDialog, new DialogState(this, gameData.EndTurnDialogCanvas) },
            { GameStateNames.BattleState, new BattleState(this, data.CoroutineStarter, data.GameTurnsData.BattleWavesParameters) },
            { GameStateNames.Upgrade, new UpgradeState(this, data.UpgradeUIGameobject) }
        };

        ChangeStateWithString(GameStateNames.StartDialog);
    }
    
    public void ChangeStateWithString(string stateName) => ChangeState(stringStates[stateName]);

    public class GameStateMachineData
    {
        public GameObject UpgradeUIGameobject { get; private set; }
        public GameData GameTurnsData { get; private set; }
        public MonoBehaviour CoroutineStarter { get; private set; }

        public GameStateMachineData(GameObject upgradeUIGameobject, GameData gameTurnsData, MonoBehaviour coroutineStarter)
        {
            this.UpgradeUIGameobject = upgradeUIGameobject;
            this.GameTurnsData = gameTurnsData;
            this.CoroutineStarter = coroutineStarter;
        }
    }
}