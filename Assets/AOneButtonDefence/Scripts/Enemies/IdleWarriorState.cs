using UnityEngine;
using UnityEngine.AI;

public class IdleWarriorState : UnitStateBase, IState
{
	private WalkingAnimation walkingAnimation;
	private NavMeshAgent agent;
	private IStateChanger stateMachine;
	private Vector3 startPosition;

	public IdleWarriorState(IStateChanger stateChanger, Vector3 startPosition, WalkingAnimation walkingAnimation, NavMeshAgent agent, CharacterStatsCounter statsCounter, Transform selfTransform) : 
		base(stateChanger, selfTransform, statsCounter)
	{
        stateMachine = stateChanger;
		this.agent = agent;
        this.startPosition = startPosition;
        this.walkingAnimation = walkingAnimation;
	}
	
	public void Enter()
	{
		if (stateMachine is WarriorStateMachine sm)
			sm.DisableEffects();

		GoToStartPosition();
	}

	public void Exit()
	{
		if (stateMachine is WarriorStateMachine sm)
			sm.EnableEffects();
	}


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