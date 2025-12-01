using System.Collections.Generic;
using AOneButtonDefence.Scripts;
using AOneButtonDefence.Scripts.Enemies.DragonScripts;
using AOneButtonDefence.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;

public class UnitWithUltimateStateMachine : StateMachine
{
    public UnitWithUltimateStateMachine(UnitWithUltimateStateMachineData data)
    {
        var fightDragonStateData = new FightWithUltimateState.FightWithUltimateStateData(
            data.Characteristics.UltimateDelay,
            data.Characteristics.SpellsData,
            data.TargetLayer,
            data.UltimateAnimation,
            this,
            data.FightAnimation,
            data.SelfDamageable,
            data.StatsCounter
            );
        
        var fightState = new FightWithUltimateState(fightDragonStateData);
        var targetFollowingState = new TargetFollowingState(this, data.Agent, data.StatsCounter, Mathf.Infinity, fightState, 
            data.TargetLayer, data.WalkingAnimation, data.EnemyDetector, data.SelfDamageable);
        var targetSearchStateData = new TargetSearchState.TargetSearchStateData(
            this, data.SelfTransform, targetFollowingState, data.Agent, data.WalkingAnimation, data.EnemyDetector);

        states = new List<IState>()
        {
            new TargetSearchState(targetSearchStateData, data.StatsCounter),
            new IdleWarriorState(this, data.SelfTransform.position, data.WalkingAnimation, data.Agent, data.StatsCounter),
            fightState,
            targetFollowingState
        };

        ChangeState<TargetSearchState>();
    }

    public class UnitWithUltimateStateMachineData : UnitStateMachineDataBaseClass
    {
        public readonly UnitWithUltimateStats Characteristics;
        public readonly CharacterStatsCounter StatsCounter;
        public readonly IAttackAnimator UltimateAnimation;
        public readonly LayerMask TargetLayer;

        public UnitWithUltimateStateMachineData(Transform selfTransform, UnitWithUltimateStats characterStats, CharacterStatsCounter statsCounter, DragonAnimation ultimateAnimation, NavMeshAgent agent, 
            WalkingAnimation walkingAnimation, IAttackAnimator fightAnimation, IEnemyDetector detector, ISelfDamageable selfDamageable, LayerMask targetLayer) : 
            base(selfTransform, agent, walkingAnimation, fightAnimation, detector, selfDamageable)
        {
            StatsCounter = statsCounter;
            UltimateAnimation = ultimateAnimation;
            Characteristics = characterStats;
            TargetLayer = targetLayer;
        }
    }
}