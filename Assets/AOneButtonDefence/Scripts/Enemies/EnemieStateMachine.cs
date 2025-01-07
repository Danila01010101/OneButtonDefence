using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemieStateMachine : StateMachine
{
    public EnemieStateMachine(Transform transform, CharacterStats characterStats, NavMeshAgent agent, MonoBehaviour coroutineStarter)
    {
        var fightState = new FightState(this, characterStats.AttackDelay, characterStats.Damage);
        var targetFollowingState = new TargetFollowingState(this, agent, characterStats, fightState, characterStats.EnemyLayerMask);

        states = new List<IState>()
        {
            new TargetSearchState(this, transform, characterStats.DetectionRadius, characterStats.EnemyLayerMask, targetFollowingState),
            fightState,
            targetFollowingState
        };

        ChangeState<TargetSearchState>();
    }
}