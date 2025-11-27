using UnityEngine;

public abstract class UnitStateBase : IState
{
    protected readonly IStateChanger StateMachine;
    protected readonly Transform SelfTransform;
    protected readonly CharacterStatsCounter StatsCounter;
    protected readonly EffectReceiver effectReceiver;

    protected virtual float ScalePercentPerResourceAmount => 0.1f;

    protected UnitStateBase(IStateChanger stateMachine, Transform selfTransform, CharacterStatsCounter statsCounter)
    {
        StateMachine = stateMachine;
        SelfTransform = selfTransform;
        StatsCounter = statsCounter;
        
        if (stateMachine is IEffectsHandler machine)
        {
            effectReceiver = new EffectReceiver(
                machine,
                selfTransform,
                statsCounter
            );
        }
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
        effectReceiver?.OnTriggerEnter(other);
    }

    public virtual void OnTriggerExit(Collider other)
    {
        effectReceiver?.OnTriggerExit(other);
    }
}