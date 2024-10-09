using System.Collections.Generic;
using UnityEngine;

public class EnemieStateMachine : StateMachine
{
    public EnemieStateMachine(Transform transform, CharacterStats characterStats)
    {
        states = new List<IState>()
        {
            new TargetSearchState(this, transform, characterStats.DetectionRadius, characterStats.EnemyLayerMask),
            new TargetFollowingState(),
            new FightState()
        };
    }
}