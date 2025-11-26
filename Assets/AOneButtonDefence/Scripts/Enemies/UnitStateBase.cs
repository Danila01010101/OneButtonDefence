using System.Linq;
using AOneButtonDefence.Scripts.StateMachine;
using UnityEngine;

public abstract class UnitStateBase : IState
{
    protected readonly IStateChanger StateMachine;
    protected readonly Transform SelfTransform;
    protected readonly CharacterStatsCounter StatsCounter;
    protected readonly EffectReceiver effectReceiver;

    protected bool IsControlledByPlayer { get; private set; }

    protected virtual float ScalePercentPerResourceAmount => 0.1f;

    protected UnitStateBase(IStateChanger stateMachine, Transform selfTransform, CharacterStatsCounter statsCounter, bool isControlledByPlayer)
    {
        StateMachine = stateMachine;
        SelfTransform = selfTransform;
        StatsCounter = statsCounter;
        IsControlledByPlayer = isControlledByPlayer;
        
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
        if (!IsControlledByPlayer) return;
        effectReceiver?.OnTriggerEnter(other);
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (!IsControlledByPlayer) return;
        effectReceiver?.OnTriggerExit(other);
    }
}