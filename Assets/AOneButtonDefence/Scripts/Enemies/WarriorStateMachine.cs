using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarriorStateMachine : StateMachine
{
    public WarriorStateMachine(WarriorStateMachineData data)
    {
        var fightState = new FightState(this, data.CharacterStats.AttackDelay, data.CharacterStats.Damage, data.FightAnimation);
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

    public class WarriorStateMachineData
    {
        public readonly Transform Transform;
        public readonly CharacterStats CharacterStats;
        public readonly NavMeshAgent Agent;
        public readonly WalkingAnimation WalkingAnimation;
        public readonly FightAnimation FightAnimation;
        public readonly IEnemyDetector EnemyDetector;
        
        public WarriorStateMachineData(Transform transform, CharacterStats characterStats, NavMeshAgent agent, 
            WalkingAnimation walkingAnimation, FightAnimation fightAnimation, IEnemyDetector detector)
        {
            Transform = transform;
            CharacterStats = characterStats;
            Agent = agent;
            WalkingAnimation = walkingAnimation;
            FightAnimation = fightAnimation;
            EnemyDetector = detector;
        }
    }
}