using System;

public class BattleEvents : IBattleEvent
{
    public void Subscribe(Action startHandler, Action endHandler)
    {
        GameBattleState.BattleStarted += startHandler;
        GameBattleState.BattleWon += endHandler;
        GameBattleState.BattleLost += endHandler;
    }

    public void Unsubscribe(Action startHandler, Action endHandler)
    {
        GameBattleState.BattleStarted -= startHandler;
        GameBattleState.BattleWon -= endHandler;
        GameBattleState.BattleLost -= endHandler;
    }
}