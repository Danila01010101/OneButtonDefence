using System.Linq;
using AOneButtonDefence.Scripts.StateMachine;
using UnityEngine;

public abstract class UnitStateBase : IState
{
    protected readonly IStateChanger StateMachine;
    protected readonly Transform SelfTransform;
    protected readonly CharacterStatsCounter StatsCounter;
    protected bool IsControlledByPlayer { get; private set; }

    protected virtual float ScalePercentPerResourceAmount => 0.1f;

    protected UnitStateBase(IStateChanger stateMachine, Transform selfTransform, CharacterStatsCounter statsCounter, bool isControlledByPlayer)
    {
        StateMachine = stateMachine;
        SelfTransform = selfTransform;
        StatsCounter = statsCounter;
        IsControlledByPlayer = isControlledByPlayer;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void HandleInput() { }
    public virtual void OnAnimationEnterEvent() { }
    public virtual void OnAnimationExitEvent() { }
    public virtual void OnAnimationTransitionEvent() { }
    public virtual void PhysicsUpdate() { }
    public virtual void Update() { }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (IsControlledByPlayer == false)
            return;
        
        if (other == null) return;
        if (!(StateMachine is IUnitStateMachineWithEffects machine)) return;

        if (!other.TryGetComponent<IEffectActivator>(out var activator)) return;
        Building.EffectCastInfo info = activator.GetEffectInfo();

        var prefab = info.BuffResource?.Resource?.ResourceEffect;
        ActiveEffect activeEffect = null;
        if (prefab != null)
        {
            var instance = Object.Instantiate(prefab, SelfTransform);
            instance.transform.localPosition = Vector3.zero;
            activeEffect = new ActiveEffect(info, activator, instance, StatsCounter);
        }

        machine.AddEffect(activeEffect);
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (IsControlledByPlayer == false)
            return;
        
        if (other == null) return;
        if (!(StateMachine is IUnitStateMachineWithEffects machine)) return;
        if (machine.CurrentEffects.Count == 0) return;

        if (!other.TryGetComponent<IEffectActivator>(out var activator)) return;
        var existing = machine.CurrentEffects.FirstOrDefault(e => ReferenceEquals(e.OriginActivator, activator));
        if (existing == null) return;

        machine.RemoveEffect(existing);
    }
}