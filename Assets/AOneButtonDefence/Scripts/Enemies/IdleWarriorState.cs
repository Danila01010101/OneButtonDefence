using UnityEngine;
using UnityEngine.AI;

public class IdleWarriorState : IState
{
	private WalkingAnimation walkingAnimation;
	private NavMeshAgent agent;
	private Vector3 startPosition;
	private bool isBattleGoing = true;

	public IdleWarriorState()
	{
        startPosition = data.StartPosition;
        walkingAnimation = data.WalkingAnimation;
		GameBattleState.BattleWon += DetectBattleEnd;
		GameBattleState.BattleLost += DetectBattleEnd;
		GameBattleState.BattleStarted += DetectBattleStart;
	}

	public void Enter() { }

	public void Exit() { }

	public void HandleInput() { }

	public void Update() { }

	public void PhysicsUpdate() { }

	public void OnTriggerEnter(Collider collider) { }

	public void OnTriggerExit(Collider collider) { }

	public void OnAnimationEnterEvent() { }

	public void OnAnimationExitEvent() { }

	public void OnAnimationTransitionEvent() { }
    
	private void DetectBattleEnd() => isBattleGoing = false;
    
	private void DetectBattleStart() => isBattleGoing = true;

	private void GoToStartPosition()
	{
		walkingAnimation.StartAnimation();
		agent.SetDestination(startPosition);
	}
}