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
        if (other == null) return;
        var machine = StateMachine as IUnitStateMachineWithEffects;
        if (machine == null) return;

        if (!other.TryGetComponent<IEffectActivator>(out var activator)) return;

        Building.EffectCastInfo info = activator.GetEffectInfo();

        if (!machine.OriginalScaleInitialized)
        {
            machine.OriginalScale = SelfTransform.localScale;
            machine.OriginalScaleInitialized = true;
        }

        float multiplier = CalculateScaleMultiplier(info);

        var activeEffect = new ActiveEffect(info, activator, multiplier);

        var prefab = info.BuffResource?.Resource?.ResourceEffect;
        if (prefab != null)
        {
            var instance = Object.Instantiate(prefab, SelfTransform);
            instance.transform.localPosition = Vector3.zero;
            activeEffect.EffectInstance = instance;
        }

        machine.AddEffect(activeEffect);
        RecalculateScale(machine);
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other == null) return;
        var machine = StateMachine as IUnitStateMachineWithEffects;
        if (machine == null || machine.CurrentEffects.Count == 0) return;

        if (!other.TryGetComponent<IEffectActivator>(out var activator)) return;

        var existing = machine.CurrentEffects.FirstOrDefault(e => ReferenceEquals(e.OriginActivator, activator));
        if (existing == null) return;

        if (existing.EffectInstance != null)
            Object.Destroy(existing.EffectInstance.gameObject);

        machine.RemoveEffect(existing);
        RecalculateScale(machine);
    }

    protected virtual float CalculateScaleMultiplier(Building.EffectCastInfo info)
    {
        if (info == null || info.BuffResource == null) return 1f;
        return 1f + info.BuffResource.Amount * ScalePercentPerResourceAmount;
    }

    private void RecalculateScale(IUnitStateMachineWithEffects machine)
    {
        if (!machine.OriginalScaleInitialized) return;

        float product = 1f;
        foreach (var e in machine.CurrentEffects)
            product *= e.ScaleMultiplier;

        SelfTransform.localScale = Vector3.Scale(machine.OriginalScale, new Vector3(product, product, product));
    }
}