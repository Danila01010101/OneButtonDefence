using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : WarriorStateMachine
{
    public EnemyStateMachine(WarriorStateMachineData data) : base(data)
    {
        var fightState = new FightState(this, data.CharacterStats.AttackDelay, data.CharacterStats.Damage, data.CharacterStats.DamageUpgradeValue, data.FightAnimation, data.SelfDamageable, false);
        var targetFollowingState = new TargetFollowingState(this, data.Agent, data.CharacterStats, fightState, 
            data.CharacterStats.EnemyLayerMask, data.WalkingAnimation, data.EnemyDetector, data.SelfDamageable, false);
        var targetSearchStateData = new TargetSearchState.TargetSearchStateData(
            this, data.SelfTransform, targetFollowingState, data.Agent, data.WalkingAnimation, data.EnemyDetector);

        states = new List<IState>()
        {
            new CharacterOrAnySearchState(targetSearchStateData, false),
            new IdleWarriorState(this, data.SelfTransform.position, data.WalkingAnimation, data.Agent),
            fightState,
            targetFollowingState
        };

        ChangeState<TargetSearchState>();
    }
}