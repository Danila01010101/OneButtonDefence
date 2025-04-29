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
            data.BaseStats.EnemyLayerMask,
            data.UltimateAnimation,
            this,
            data.FightAnimation,
            data.BaseStats.AttackDelay,
            data.BaseStats.Damage
            );
        
        var fightState = new FightWithUltimateState(fightDragonStateData);
        var targetFollowingState = new TargetFollowingState(this, data.Agent, data.BaseStats, fightState, 
            data.BaseStats.EnemyLayerMask, data.WalkingAnimation, data.EnemyDetector);
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
        public readonly CharacterStats BaseStats;
        public readonly IAttackAnimator UltimateAnimation;

        public UnitWithUltimateStateMachineData(Transform transform, UnitWithUltimateStats characterStats, CharacterStats baseStats, DragonAnimation ultimateAnimation, NavMeshAgent agent, 
            WalkingAnimation walkingAnimation, IAttackAnimator fightAnimation, IEnemyDetector detector) : 
            base(transform, agent, walkingAnimation, fightAnimation, detector)
        {
            BaseStats = baseStats;
            UltimateAnimation = ultimateAnimation;
            Characteristics = characterStats;
        }
    }
}