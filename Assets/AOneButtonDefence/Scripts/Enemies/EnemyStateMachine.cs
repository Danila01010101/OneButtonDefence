using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    public EnemyStateMachine(EnemyStateMachineData data)
    {
        var fightState = new FightState(this, data.CharacterStats.AttackDelay, data.CharacterStats.Damage, data.FightAnimation);
        var targetFollowingState = new TargetFollowingState(this, data.Agent, data.CharacterStats, fightState, data.CharacterStats.EnemyLayerMask, data.GoingAnimation);

        states = new List<IState>()
        {
            new TargetSearchState(this, data.Transform, data.CharacterStats.DetectionRadius, data.CharacterStats.EnemyLayerMask, targetFollowingState),
            fightState,
            targetFollowingState
        };

        ChangeState<TargetSearchState>();
    }

    public class EnemyStateMachineData
    {
        public Transform Transform { get; private set; }
        public CharacterStats CharacterStats { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public MonoBehaviour CoroutineStarter { get; private set; }
        public GoingAnimation GoingAnimation { get; private set; }
        public FightAnimation FightAnimation { get; private set; }
        
        public EnemyStateMachineData(Transform transform, CharacterStats characterStats, NavMeshAgent agent, 
            MonoBehaviour coroutineStarter, GoingAnimation goingAnimation, FightAnimation fightAnimation)
        {
            Transform = transform;
            CharacterStats = characterStats;
            Agent = agent;
            CoroutineStarter = coroutineStarter;
            GoingAnimation = goingAnimation;
            FightAnimation = fightAnimation;
        }
    }
}