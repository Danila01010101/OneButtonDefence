using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : WarriorStateMachine
{
    public EnemyStateMachine(WarriorStateMachineData data) : base(data)
    {
        var fightState = new FightState(this, data.CharacterStatsCounter, data.FightAnimation, data.SelfDamageable, false);
        var targetFollowingState = new TargetFollowingState(this, data.Agent, data.CharacterStatsCounter, data.EnemyChaseRange, fightState, data.EnemyLayerMask,
            data.WalkingAnimation, data.EnemyDetector, data.SelfDamageable, false);
        var targetSearchStateData = new TargetSearchState.TargetSearchStateData(
            this, data.SelfTransform, targetFollowingState, data.Agent, data.WalkingAnimation, data.EnemyDetector);

        states = new List<IState>()
        {
            new CharacterOrAnySearchState(targetSearchStateData, false, data.CharacterStatsCounter),
            new IdleWarriorState(this, data.SelfTransform.position, data.WalkingAnimation, data.Agent, data.CharacterStatsCounter, false),
            fightState,
            targetFollowingState
        };

        ChangeState<TargetSearchState>();
    }
}