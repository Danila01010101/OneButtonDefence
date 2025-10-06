using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : IStateChanger
{
    protected IState currentState;
    protected List<IState> states;
    
    public bool IsActive { get; private set; } = true;

    public virtual void ChangeState(IState newState)
    {
        if (!IsActive)
            return;

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void ChangeState<T>() where T : IState
    {
        if (!IsActive)
            return;

        foreach (var state in states)
        {
            if (state is T)
            {
                ChangeState(state);
                return;
            }
        }

        throw new System.ArgumentException("No such state as " + typeof(T));
    }

    public void HandleInput()
    {
        if (!IsActive) return;
        currentState?.HandleInput();
    }

    public void Update()
    {
        if (!IsActive) return;
        currentState?.Update();
    }

    public void PhysicsUpdate()
    {
        if (!IsActive) return;
        currentState?.PhysicsUpdate();
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (!IsActive) return;
        currentState?.OnTriggerEnter(collider);
    }

    public void OnTriggerExit(Collider collider)
    {
        if (!IsActive) return;
        currentState?.OnTriggerExit(collider);
    }

    public void OnAnimationEnterEvent()
    {
        if (!IsActive) return;
        currentState?.OnAnimationEnterEvent();
    }

    public void OnAnimationExitEvent()
    {
        if (!IsActive) return;
        currentState?.OnAnimationExitEvent();
    }

    public void OnAnimationTransitionEvent()
    {
        if (!IsActive) return;
        currentState?.OnAnimationTransitionEvent();
    }

    public virtual void Exit()
    {
        if (!IsActive)
            return;

        IsActive = false;
        currentState?.Exit();
        currentState = null;
    }
}