using UnityEngine;
using UnityEngine.AI;

public class IdleWarriorState : IState
{
	private WalkingAnimation walkingAnimation;
	private NavMeshAgent agent;
	private IStateChanger stateMachine;
	private Vector3 startPosition;
	private bool isBattleGoing = true;

	public IdleWarriorState(IStateChanger stateChanger, Vector3 startPosition, WalkingAnimation walkingAnimation, NavMeshAgent agent)
	{
        stateMachine = stateChanger;
		this.agent = agent;
        this.startPosition = startPosition;
        this.walkingAnimation = walkingAnimation;
	}

	public void Enter()
    {
        GoToStartPosition();
	}

	public void Exit() { }

	public void HandleInput() { }

	public void Update()
	{
		if (BattleNotifier.Instance.IsBattleGoing())
		{
			stateMachine.ChangeState<TargetSearchState>();
		}
	}

	public void PhysicsUpdate() { }

	public void OnTriggerEnter(Collider collider) { }

	public void OnTriggerExit(Collider collider) { }

	public void OnAnimationEnterEvent() { }

	public void OnAnimationExitEvent() { }

	public void OnAnimationTransitionEvent() { }

	private void GoToStartPosition()
	{
		walkingAnimation.StartAnimation();
		agent.SetDestination(startPosition);
	}
}