using UnityEngine;

public interface IStateChanger
{
    public void ChangeState(string stateName);

    public void HandleInput();

    public void Update();

    public void PhysicsUpdate();

    public void OnTriggerEnter(Collider collider);

    public void OnTriggerExit(Collider collider);

    public void OnAnimationEnterEvent();

    public void OnAnimationExitEvent();

    public void OnAnimationTransitionEvent();
}