using System.Collections.Generic;
using AOneButtonDefence.Scripts;
using UnityEngine;
using UnityEngine.AI;

public class WarriorStateMachine : StateMachine
{
    public WarriorStateMachine(WarriorStateMachineData data)
    {
        var fightState = new FightState(this, data.CharacterStats.AttackDelay, data.CharacterStats.Damage, data.CharacterStats.DamageUpgradeValue, data.FightAnimation, data.SelfDamageable, true);
        var targetFollowingState = new TargetFollowingState(this, data.Agent, data.CharacterStats, fightState, 
            data.CharacterStats.EnemyLayerMask, data.WalkingAnimation, data.EnemyDetector, data.SelfDamageable, true);
        var targetSearchStateData = new TargetSearchState.TargetSearchStateData(
            this, data.SelfTransform, targetFollowingState, data.Agent, data.WalkingAnimation, data.EnemyDetector);

        states = new List<IState>()
        {
            new TargetSearchState(targetSearchStateData),
            new IdleWarriorState(this, data.SelfTransform.position, data.WalkingAnimation, data.Agent),
            fightState,
            targetFollowingState
        };

        ChangeState<TargetSearchState>();
    }

    public class WarriorStateMachineData : UnitStateMachineDataBaseClass
    {
        public readonly CharacterStats CharacterStats;
        
        public WarriorStateMachineData(Transform selfTransform, CharacterStats characterStats, NavMeshAgent agent, 
            WalkingAnimation walkingAnimation, FightAnimation fightAnimation, IEnemyDetector detector, ISelfDamageable selfDamagable) : 
            base(selfTransform, agent, walkingAnimation, fightAnimation, detector, selfDamagable)
        {
            CharacterStats = characterStats;
        }
    }
}