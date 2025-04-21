using System.Collections.Generic;
using UnityEngine;

namespace AOneButtonDefence.Scripts.Enemies.DragonScripts
{
    public class FightWithUltimateState : FightState
    {
        private readonly FightWithUltimateStateData unitWithUltimateData;
        
        private Transform spellsPositionTransform;
        private float LastTimeUltimateUsed;
        
        public FightWithUltimateState(FightWithUltimateStateData unitWithUltimateData) 
            : base(unitWithUltimateData.StateMachine, unitWithUltimateData.AttackDelay, unitWithUltimateData.Damage, unitWithUltimateData.Animation)
        {
            this.unitWithUltimateData = unitWithUltimateData;
            spellsPositionTransform = unitWithUltimateData.UltimateAnimator.transform;
        }

        public override void Enter()
        {
            base.Enter();
            unitWithUltimateData.UltimateAnimator.CharacterAttacked += UseUltimate;
        }

        public override void Exit()
        {
            base.Exit();
            unitWithUltimateData.UltimateAnimator.CharacterAttacked -= UseUltimate;
        }

        public override void Update()
        {
            base.Update();
            
            if (LastTimeUltimateUsed + unitWithUltimateData.UltimateDelay >= Time.time)
                return;
            
            LastTimeUltimateUsed = Time.time;
            unitWithUltimateData.UltimateAnimator.StartAnimation();
        }

        protected virtual void UseUltimate()
        {
            int randomSpellIndex = Random.Range(0, unitWithUltimateData.SpellsData.Count);
            SpellData randomSpell = unitWithUltimateData.SpellsData[randomSpellIndex];
            
            if (IsTargetSetted)
                SpellCaster.Cast(randomSpell.BaseMagicCircle, randomSpell, spellsPositionTransform.position, unitWithUltimateData.TargetLayer);
        }

        public class FightWithUltimateStateData
        {
            public readonly float UltimateDelay;
            public readonly List<SpellData> SpellsData;
            public readonly LayerMask TargetLayer;
            public readonly FightAnimation UltimateAnimator;
            public readonly IStateChanger StateMachine;
            public readonly MonoBehaviour CoroutineStarter;
            public readonly FightAnimation Animation;
            public readonly float AttackDelay;
            public readonly int Damage;

            public FightWithUltimateStateData(float ultimateDelay, List<SpellData> spellsData, LayerMask targetLayer, FightAnimation ultimateAnimator, 
                IStateChanger stateMachine, MonoBehaviour coroutineStarter, FightAnimation animation, float attackDelay, int damage)
            {
                UltimateDelay = ultimateDelay;
                SpellsData = spellsData;
                TargetLayer = targetLayer;
                UltimateAnimator = ultimateAnimator;
                StateMachine = stateMachine;
                CoroutineStarter = coroutineStarter;
                Animation = animation;
                AttackDelay = attackDelay;
                Damage = damage;
            }
        }
    }
}