using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : IStateChanger
{
    public string CurrentStateName => currentState.GetType().Name.ToString();

    protected IState currentState;
    protected List<IState> states;

    public virtual void ChangeState(IState enwState)
    {
        currentState?.Exit();

        currentState = enwState;

        currentState.Enter();
    }

    public void ChangeState<T>() where T : IState
    {
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
        currentState?.HandleInput();
    }

    public void Update()
    {
        currentState?.Update();
    }

    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }

    public void OnTriggerEnter(Collider collider)
    {
        currentState?.OnTriggerEnter(collider);
    }

    public void OnTriggerExit(Collider collider)
    {
        currentState?.OnTriggerExit(collider);
    }

    public void OnAnimationEnterEvent()
    {
        currentState?.OnAnimationEnterEvent();
    }

    public void OnAnimationExitEvent()
    {
        currentState?.OnAnimationExitEvent();
    }

    public void OnAnimationTransitionEvent()
    {
        currentState?.OnAnimationTransitionEvent();
    }
}