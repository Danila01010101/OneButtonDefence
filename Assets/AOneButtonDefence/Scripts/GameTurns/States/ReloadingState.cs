using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadingState : IState
{
	private IStringStateChanger stateMachine;

	public void Enter()
	{
		SceneManager.LoadScene(Constants.GameplaySceneIndex);
	}

	public void Exit()
	{
		
	}

	public void HandleInput() { }

	public void OnAnimationEnterEvent() { } 

	public void OnAnimationExitEvent() { }

	public void OnAnimationTransitionEvent() { }

	public void OnTriggerEnter(Collider collider) { }

	public void OnTriggerExit(Collider collider) { }

	public void PhysicsUpdate() { }

	public void Update() { }
}