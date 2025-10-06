using System.Linq;
using AOneButtonDefence.Scripts.StateMachine;
using UnityEngine;

public abstract class UnitStateBase : IState
{
    protected readonly IStateChanger StateMachine;
    protected readonly Transform SelfTransform;
    protected readonly bool IsPlayerControlled;

    protected virtual float ScalePercentPerResourceAmount => 0.1f;

    protected UnitStateBase(IStateChanger stateMachine, Transform selfTransform, bool isPlayerControlled)
    {
        StateMachine = stateMachine;
        SelfTransform = selfTransform;
        IsPlayerControlled = isPlayerControlled;
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
        if (other == null || !IsPlayerControlled) return;
        if (!(StateMachine is IUnitStateMachineWithEffects machine)) return;

        if (!other.TryGetComponent<IEffectActivator>(out var activator)) return;
        Building.EffectCastInfo info = activator.GetEffectInfo();

        var prefab = info.BuffResource?.Resource?.ResourceEffect;
        ActiveEffect activeEffect = null;
        if (prefab != null)
        {
            var instance = Object.Instantiate(prefab, SelfTransform);
            instance.transform.localPosition = Vector3.zero;
            activeEffect = new ActiveEffect(info, activator, instance);
        }

        machine.AddEffect(activeEffect);
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other == null) return;
        if (!(StateMachine is IUnitStateMachineWithEffects machine)) return;
        if (machine.CurrentEffects.Count == 0) return;

        if (!other.TryGetComponent<IEffectActivator>(out var activator)) return;
        var existing = machine.CurrentEffects.FirstOrDefault(e => ReferenceEquals(e.OriginActivator, activator));
        if (existing == null) return;

        machine.RemoveEffect(existing);
    }
}