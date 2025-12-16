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
            data.CharacterStatsCounter,
            data.FightAnimation,
            data.SelfDamageable,
            true);

        var targetFollowingState = new TargetFollowingState(
            this,
            data.Agent,
            data.CharacterStatsCounter,
            data.EnemyChaseRange,
            fightState,
            data.EnemyLayerMask,
            data.WalkingAnimation,
            data.EnemyDetector,
            data.SelfDamageable,
            data.SelfTransform
            );

        var targetSearchStateData = new TargetSearchState.TargetSearchStateData(
            this,
            data.SelfTransform,
            targetFollowingState,
            data.Agent,
            data.WalkingAnimation,
            data.EnemyDetector,
            data.DetectionRadius,
            data.SelfTransform.position
            );

        states = new List<IState>()
        {
            new TargetSearchState(targetSearchStateData, data.CharacterStatsCounter),
            new IdleWarriorState(this, data.SelfTransform.position, data.WalkingAnimation, data.Agent, data.CharacterStatsCounter, data.SelfTransform),
            fightState,
            targetFollowingState
        };

        CurrentEffects = new List<ActiveEffect>();
        SelfTransform = data.SelfTransform;
        OriginalScale = SelfTransform.localScale;
        scaleTween = null;
        ChangeState<TargetSearchState>();
    }

    public class WarriorStateMachineData : UnitStateMachineDataBaseClass
    {
        public readonly CharacterStatsCounter CharacterStatsCounter;
        public readonly LayerMask EnemyLayerMask;
        public readonly float EnemyChaseRange;
        public readonly float DetectionRadius;
        
        public WarriorStateMachineData(
            Transform selfTransform,
            CharacterStatsCounter statsCounterCounter,
            float enemyChaseRange,
            LayerMask enemyLayerMask,
            NavMeshAgent agent,
            WalkingAnimation walkingAnimation,
            FightAnimation fightAnimation,
            IEnemyDetector detector,
            ISelfDamageable selfDamagable,
            float detectionRadius) 
            : base(selfTransform, agent, walkingAnimation, fightAnimation, detector, selfDamagable)
        {
            DetectionRadius = detectionRadius;
            EnemyLayerMask = enemyLayerMask;
            CharacterStatsCounter = statsCounterCounter;
            EnemyChaseRange = enemyChaseRange;
        }
    }

    public List<ActiveEffect> CurrentEffects { get; private set; }
    public Vector3 OriginalScale { get; set; }
    public Transform SelfTransform { get; private set; }

    private Tween scaleTween;

    public override void Exit()
    {
        if (scaleTween != null) scaleTween.Kill();
        SelfTransform.DOKill();
        base.Exit();
    }

    public void AddEffect(ActiveEffect effect)
    {
        if (effect == null || effect.EffectInstance == null)
            return;

        bool alreadyHas = CurrentEffects.Exists(e =>
            e == effect || e.EffectInstance == effect.EffectInstance);

        if (alreadyHas)
            return;

        CurrentEffects.Add(effect);
        EnableEffect(effect);
        RecalculateScale();
    }

    public void RemoveEffect(ActiveEffect effect)
    {
        if (effect == null) return;

        var existing = CurrentEffects.Find(e =>
            e == effect || e.EffectInstance == effect.EffectInstance);

        if (existing == null) return;

        DisableEffect(existing);
        CurrentEffects.Remove(existing);

        if (existing.EffectInstance != null)
            GameObject.Destroy(existing.EffectInstance.gameObject);

        RecalculateScale();
    }

    public void RemoveEffectByActivator(IEffectActivator activator)
    {
        var existing = CurrentEffects
            .Find(e => ReferenceEquals(e.OriginActivator, activator));

        if (existing != null)
            RemoveEffect(existing);
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

        CurrentEffects.Clear();

        if (scaleTween != null) scaleTween.Kill();
        SelfTransform.localScale = OriginalScale;
    }

    private void EnableEffect(ActiveEffect effect)
    {
        if (effect.EffectInstance != null)
        {
            effect.Enable();
        }
    }

    private void DisableEffect(ActiveEffect effect)
    {
        if (effect.EffectInstance != null)
        {
            effect.EffectInstance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            effect.Disable();
        }
    }

    private void RecalculateScale()
    {
        float totalMultiplier = 0f;
        foreach (var e in CurrentEffects)
            totalMultiplier += e.ScaleMultiplier;

        foreach (var effect in CurrentEffects)
            effect.ApplyScale();

        Vector3 targetScale = OriginalScale + Vector3.one * totalMultiplier;

        if (scaleTween != null && scaleTween.IsActive()) scaleTween.Kill();

        scaleTween = SelfTransform.DOScale(targetScale, 0.25f).SetEase(Ease.OutQuad).SetLink(SelfTransform.gameObject);
    }
}