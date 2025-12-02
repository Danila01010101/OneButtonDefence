using System;
using UnityEngine;

public class BattleEventsForBossFight : IBattleEvent
{
    public void Subscribe(Action startHandler, Action endHandler)
    {
        BossFightBattleState.BattleStarted += startHandler;
        BossFightBattleState.EnemiesDefeated += endHandler;
    }

    public void Unsubscribe(Action startHandler, Action endHandler)
    {
        BossFightBattleState.BattleStarted -= startHandler;
        BossFightBattleState.EnemiesDefeated -= endHandler;
    }
}
