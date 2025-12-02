using UnityEngine;

public interface IStateChanger
{
    public void ChangeState<T>() where T : IState;

    public void ChangeState(IState state);

    public void HandleInput();

    public void Update();

    public void PhysicsUpdate();

    public void OnTriggerEnter(Collider collider);

    public void OnTriggerExit(Collider collider);

    public void OnAnimationEnterEvent();

    public void OnAnimationExitEvent();

    public void OnAnimationTransitionEvent();

    public void Exit();
    public bool IsActive { get; }
}