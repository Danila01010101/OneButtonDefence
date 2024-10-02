using System.Collections.Generic;

public class GameStateMachine : StateMachine
{
    public DialogueSystem startDialog;
    public DialogueSystem fightEndDialogs;

    public GameStateMachine()
    {
        states = new Dictionary<string, IState>()
        {
            { GameStateNames.StartDialog, new DialogState(startDialog) },
            { GameStateNames.BattleState, new BattleState() },
            { GameStateNames.Upgrade, new UpgradeState() }
        };

        ChangeState(GameStateNames.StartDialog);
    }
}