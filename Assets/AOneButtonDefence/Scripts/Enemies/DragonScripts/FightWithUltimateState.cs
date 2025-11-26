using System.Collections.Generic;
using AOneButtonDefence.Scripts.Enemies.DragonScripts;
using AOneButtonDefence.Scripts.Interfaces;
using UnityEngine;

public class FightWithUltimateState : FightState
{
    private readonly FightWithUltimateStateData unitWithUltimateData;
    
    private static CharacterStatsCounter characterStatsCounter;
    private Transform spellsPositionTransform;
    private float LastTimeUltimateUsed;
    
    public FightWithUltimateState(FightWithUltimateStateData unitWithUltimateData) 
        : base(unitWithUltimateData.StateMachine, characterStatsCounter, unitWithUltimateData.Animation, unitWithUltimateData.SelfDamageable, false)
    {
        this.unitWithUltimateData = unitWithUltimateData;
        spellsPositionTransform = unitWithUltimateData.Animation.Transform;
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
            SpellCaster.Cast(randomSpell.BaseMagicCircle, randomSpell, spellsPositionTransform.position, unitWithUltimateData.TargetLayer, 0);
    }

    public class FightWithUltimateStateData
    {
        public readonly float UltimateDelay;
        public readonly List<SpellData> SpellsData;
        public readonly LayerMask TargetLayer;
        public readonly IAttackAnimator UltimateAnimator;
        public readonly IStateChanger StateMachine;
        public readonly IAttackAnimator Animation;
        public readonly ISelfDamageable SelfDamageable;
        public readonly float AttackDelay;
        public readonly int Damage;
        public readonly int DamageUpgradeValue;

        public FightWithUltimateStateData(float ultimateDelay, List<SpellData> spellsData, LayerMask targetLayer, IAttackAnimator ultimateAnimator, 
            IStateChanger stateMachine, IAttackAnimator animation, float attackDelay, int damage, int damageUpgradeValue, ISelfDamageable selfDamageable)
        {
            UltimateDelay = ultimateDelay;
            SpellsData = spellsData;
            TargetLayer = targetLayer;
            UltimateAnimator = ultimateAnimator;
            StateMachine = stateMachine;
            Animation = animation;
            AttackDelay = attackDelay;
            Damage = damage;
            DamageUpgradeValue = damageUpgradeValue;
            SelfDamageable = selfDamageable;
        }
    }
}