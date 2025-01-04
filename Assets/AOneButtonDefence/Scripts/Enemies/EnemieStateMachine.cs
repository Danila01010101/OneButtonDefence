using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemieStateMachine : StateMachine
{
    public EnemieStateMachine(Transform transform, CharacterStats characterStats, NavMeshAgent agent, MonoBehaviour coroutineStarter)
    {
        var fightState = new FightState(this, coroutineStarter, characterStats.AttackDelay, characterStats.Damage);
        var targetFollowingState = new TargetFollowingState(this, agent, characterStats, fightState);

        states = new List<IState>()
        {
            new TargetSearchState(this, transform, characterStats.DetectionRadius, characterStats.EnemyLayerMask, targetFollowingState),
            fightState,
            targetFollowingState
        };

        ChangeState<TargetSearchState>();
    }
}