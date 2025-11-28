using System.Collections.Generic;

public class BossFightStateMachine : StateMachine, IStringStateChanger
{
    private Dictionary<string, IState> stringStates;

    public BossFightStateMachine(BossFightStateMachineData data)
    {
        stringStates = new Dictionary<string, IState>()
        {
            { GameStateNames.StartDialog, new DialogState(this, null, data.GameTurnsData.StartDragonDialogCanvas, GameStateNames.BattleState, data.Input, true) },
            { GameStateNames.Reload, new ReloadingState() },
            { GameStateNames.BattleState, new BossFightBattleState() },
        };
        
        ChangeStateWithString(GameStateNames.StartDialog);
    }
    
    public void ChangeStateWithString(string stateName) => ChangeState(stringStates[stateName]);

    public class BossFightStateMachineData
    {
        public readonly GameData GameTurnsData;
        public readonly IDisableableInput Input;

        public BossFightStateMachineData(GameData gameTurnsData, IDisableableInput input)
        {
            GameTurnsData = gameTurnsData;
            Input = input;
        }
    }
}
