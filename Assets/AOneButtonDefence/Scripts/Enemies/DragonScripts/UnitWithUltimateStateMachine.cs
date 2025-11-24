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
            data.BaseStats.Damage,
            data.BaseStats.DamageUpgradeValue,
            data.SelfDamageable
            );
        
        //var fightState = new FightWithUltimateState(fightDragonStateData);
        //var targetFollowingState = new TargetFollowingState(this, data.Agent, data.StatsCounter, data.BaseStats.ChaseRange, fightState, 
        //    data.BaseStats.EnemyLayerMask, data.WalkingAnimation, data.EnemyDetector, data.SelfDamageable);
        //var targetSearchStateData = new TargetSearchState.TargetSearchStateData(
        //    this, data.SelfTransform, targetFollowingState, data.Agent, data.WalkingAnimation, data.EnemyDetector);

        //states = new List<IState>()
        //{
          //  new TargetSearchState(targetSearchStateData, false),
            //new IdleWarriorState(this, data.SelfTransform.position, data.WalkingAnimation, data.Agent),
            //fightState,
            //targetFollowingState
       // };

        //ChangeState<TargetSearchState>();
    }

    public class UnitWithUltimateStateMachineData : UnitStateMachineDataBaseClass
    {
        public readonly UnitWithUltimateStats Characteristics;
        public readonly CharacterStatsCounter StatsCounter;
        public readonly CharacterStats BaseStats;
        public readonly IAttackAnimator UltimateAnimation;

        public UnitWithUltimateStateMachineData(Transform selfTransform, UnitWithUltimateStats characterStats, CharacterStatsCounter statsCounter, DragonAnimation ultimateAnimation, NavMeshAgent agent, 
            WalkingAnimation walkingAnimation, IAttackAnimator fightAnimation, IEnemyDetector detector, ISelfDamageable selfDamageable) : 
            base(selfTransform, agent, walkingAnimation, fightAnimation, detector, selfDamageable)
        {
            StatsCounter = statsCounter;
            UltimateAnimation = ultimateAnimation;
            Characteristics = characterStats;
        }
    }
}