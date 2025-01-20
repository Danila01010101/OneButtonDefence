using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarriorStateMachine : StateMachine
{
    public WarriorStateMachine(WarriorStateMachineData data)
    {
        var fightState = new FightState(this, data.CharacterStats.AttackDelay, data.CharacterStats.Damage, data.FightAnimation);
        var targetFollowingState = new TargetFollowingState(this, data.Agent, data.CharacterStats, fightState, data.CharacterStats.EnemyLayerMask, data.WalkingAnimation);
        var targetSearchStateData = new TargetSearchState.TargetSearchStateData(
            this, data.Transform, data.CharacterStats.DetectionRadius, data.CharacterStats.EnemyLayerMask,
            targetFollowingState, data.Agent, data.Transform.position, data.WalkingAnimation);

        states = new List<IState>()
        {
            new TargetSearchState(targetSearchStateData),
            fightState,
            targetFollowingState
        };

        ChangeState<TargetSearchState>();
    }

    public class WarriorStateMachineData
    {
        public Transform Transform { get; private set; }
        public CharacterStats CharacterStats { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public MonoBehaviour CoroutineStarter { get; private set; }
        public WalkingAnimation WalkingAnimation { get; private set; }
        public FightAnimation FightAnimation { get; private set; }
        
        public WarriorStateMachineData(Transform transform, CharacterStats characterStats, NavMeshAgent agent, 
            MonoBehaviour coroutineStarter, WalkingAnimation walkingAnimation, FightAnimation fightAnimation)
        {
            Transform = transform;
            CharacterStats = characterStats;
            Agent = agent;
            CoroutineStarter = coroutineStarter;
            WalkingAnimation = walkingAnimation;
            FightAnimation = fightAnimation;
        }
    }
}