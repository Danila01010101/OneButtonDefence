using System;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialStartState : IState
{
    private readonly IStringStateChanger stateMachine;
    private readonly string nextStateName;

    public static Action TutorialStarted;

    public TutorialStartState(IStringStateChanger stateMachine, string nextStateName)
    {
        this.stateMachine = stateMachine;
        this.nextStateName = nextStateName;
    }

    public void Enter()
    {
        TutorialStarted?.Invoke();
        stateMachine.ChangeStateWithString(nextStateName);
    }

    public void Exit() { }

    public void HandleInput() { }

    public void OnAnimationEnterEvent() { }

    public void OnAnimationExitEvent() { }

    public void OnAnimationTransitionEvent() { }

    public void OnTriggerEnter(Collider collider) { }

    public void OnTriggerExit(Collider collider) { }

    public void PhysicsUpdate() { }

    public void Update() { }
}
