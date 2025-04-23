using System.Collections.Generic;
using AOneButtonDefence.Scripts;
using AOneButtonDefence.Scripts.Enemies.DragonScripts;
using AOneButtonDefence.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class UnitWithUltimateStateMachine : StateMachine
{
    public UnitWithUltimateStateMachine(UnitWithUltimateStateMachineData data)
    {
        var fightDragonStateData = new FightWithUltimateState.FightWithUltimateStateData(
            data.Characteristics.UltimateDelay,
            data.Characteristics.SpellsData,
            data.Characteristics.BaseStats.EnemyLayerMask,
            data.UltimateAnimation,
            this,
            data.FightAnimation,
            data.Characteristics.BaseStats.AttackDelay,
            data.Characteristics.BaseStats.Damage
            );
        
        var fightState = new FightWithUltimateState(fightDragonStateData);
        var targetFollowingState = new TargetFollowingState(this, data.Agent, data.Characteristics.BaseStats, fightState, 
            data.Characteristics.BaseStats.EnemyLayerMask, data.WalkingAnimation, data.EnemyDetector);
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

    public class UnitWithUltimateStateMachineData : UnitStateMachineDataBaseClass
    {
        public readonly UnitWithUltimateStats Characteristics;
        public readonly IAttackAnimator UltimateAnimation;

        public UnitWithUltimateStateMachineData(Transform transform, UnitWithUltimateStats characterStats, DragonAnimation ultimateAnimation, NavMeshAgent agent, 
            WalkingAnimation walkingAnimation, IAttackAnimator fightAnimation, IEnemyDetector detector) : 
            base(transform, agent, walkingAnimation, fightAnimation, detector)
        {
            UltimateAnimation = ultimateAnimation;
            Characteristics = characterStats;
        }
    }
}