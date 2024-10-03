using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : StateMachine
{
    public GameStateMachine(MonoBehaviour coroutineStarter, GameData gameData)
    {
        states = new Dictionary<string, IState>()
        {
            { GameStateNames.StartDialog, new DialogState(this, gameData.StartDialogCanvas) },
            //{ GameStateNames.DragonDialog, new DialogState(this, gameData.EndTurnDialogCanvas) },
            { GameStateNames.BattleState, new BattleState(this, coroutineStarter, gameData.BattleWavesParameters) },
            { GameStateNames.Upgrade, new UpgradeState(this) }
        };

        ChangeState(GameStateNames.StartDialog);
    }
}