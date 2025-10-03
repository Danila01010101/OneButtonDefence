using System.Collections.Generic;
using AOneButtonDefence.Scripts;
using AOneButtonDefence.Scripts.StateMachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class WarriorStateMachine : StateMachine, IUnitStateMachineWithEffects
{
    public WarriorStateMachine(WarriorStateMachineData data)
    {
        var fightState = new FightState(
            this,
            data.CharacterStats.AttackDelay,
            data.CharacterStats.Damage,
            data.CharacterStats.DamageUpgradeValue,
            data.FightAnimation,
            data.SelfDamageable,
            true);

        var targetFollowingState = new TargetFollowingState(
            this,
            data.Agent,
            data.CharacterStats,
            fightState,
            data.CharacterStats.EnemyLayerMask,
            data.WalkingAnimation,
            data.EnemyDetector,
            data.SelfDamageable,
            true);

        var targetSearchStateData = new TargetSearchState.TargetSearchStateData(
            this,
            data.SelfTransform,
            targetFollowingState,
            data.Agent,
            data.WalkingAnimation,
            data.EnemyDetector);

        states = new List<IState>()
        {
            new TargetSearchState(targetSearchStateData, true),
            new IdleWarriorState(this, data.SelfTransform.position, data.WalkingAnimation, data.Agent),
            fightState,
            targetFollowingState
        };

        CurrentEffects = new List<ActiveEffect>();
        SelfTransform = data.SelfTransform;
        ChangeState<TargetSearchState>();
    }

    public class WarriorStateMachineData : UnitStateMachineDataBaseClass
    {
        public readonly CharacterStats CharacterStats;
        
        public WarriorStateMachineData(
            Transform selfTransform,
            CharacterStats characterStats,
            NavMeshAgent agent,
            WalkingAnimation walkingAnimation,
            FightAnimation fightAnimation,
            IEnemyDetector detector,
            ISelfDamageable selfDamagable) 
            : base(selfTransform, agent, walkingAnimation, fightAnimation, detector, selfDamagable)
        {
            CharacterStats = characterStats;
        }
    }

    public List<ActiveEffect> CurrentEffects { get; private set; }
    public Vector3 OriginalScale { get; set; }
    public bool OriginalScaleInitialized { get; set; }
    public Transform SelfTransform { get; private set; }

    public void AddEffect(ActiveEffect effect)
    {
        if (effect == null) return;
        if (!CurrentEffects.Contains(effect))
        {
            CurrentEffects.Add(effect);
            EnableEffect(effect);
            RecalculateScale();
        }
    }

    public void RemoveEffect(ActiveEffect effect)
    {
        if (effect == null) return;
        if (CurrentEffects.Contains(effect))
        {
            DisableEffect(effect);
            CurrentEffects.Remove(effect);

            if (effect.EffectInstance != null)
                GameObject.Destroy(effect.EffectInstance.gameObject);

            RecalculateScale();
        }
    }

    public void EnableEffects()
    {
        foreach (var effect in CurrentEffects)
            EnableEffect(effect);

        RecalculateScale();
    }

    public void DisableEffects()
    {
        foreach (var effect in CurrentEffects)
            DisableEffect(effect);

        SelfTransform.localScale = OriginalScale;
    }

    private void EnableEffect(ActiveEffect effect)
    {
        if (effect.EffectInstance != null)
        {
            effect.EffectInstance.Play();
        }
    }

    private void DisableEffect(ActiveEffect effect)
    {
        if (effect.EffectInstance != null)
        {
            effect.EffectInstance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            effect.EffectInstance.transform.localScale = Vector3.zero;
        }
    }

    public void RecalculateScale()
    {
        if (!OriginalScaleInitialized && SelfTransform != null)
        {
            OriginalScale = SelfTransform.localScale;
            OriginalScaleInitialized = true;
        }

        if (!OriginalScaleInitialized) return;

        float product = 1f;
        foreach (var e in CurrentEffects)
            product *= e.ScaleMultiplier;

        Vector3 targetScale = Vector3.Scale(OriginalScale, new Vector3(product, product, product));

        SelfTransform.DOKill();

        SelfTransform.DOScale(targetScale, 0.25f).SetEase(Ease.OutQuad);
    }
}