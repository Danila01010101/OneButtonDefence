using System.Collections.Generic;
using AOneButtonDefence.Scripts.Enemies.DragonScripts;
using UnityEngine;

public class UnitWithUltimateStateMachine : StateMachine
{
    public UnitWithUltimateStateMachine(WarriorStateMachine.WarriorStateMachineData data)
    {
        var fightState = new FightWithUltimateState();
        var targetFollowingState = new TargetFollowingState(this, data.Agent, data.CharacterStats, fightState, 
            data.CharacterStats.EnemyLayerMask, data.WalkingAnimation, data.EnemyDetector);
        var targetSearchStateData = new TargetSearchState.TargetSearchStateData(
            this, data.Transform, targetFollowingState, data.Agent, data.WalkingAnimation, data.EnemyDetector);

        states = new List<IState>()
        {
            new TargetSearchState(targetSearchStateData),
            new IdleWarriorState(this, data.Transform.position, data.WalkingAnimation, data.Agent),
            fightState,
            targetFollowingState
        };

        ChangeState<TargetSearchState>();
    }
}