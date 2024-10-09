using UnityEngine;

public class UpgradeState : IState
{
    private GameObject upgradeUI;
    private IStringStateChanger stateMachine;

    public UpgradeState(IStringStateChanger stateMachine, GameObject upgradeUI)
    {
        this.stateMachine = stateMachine;
        this.upgradeUI = upgradeUI;
    }

    public void Enter()
    {
        UpgradeButton.TurnEnded += EndTurn;
        upgradeUI.SetActive(true);
    }

    public void Exit()
    {
        UpgradeButton.TurnEnded -= EndTurn;
        upgradeUI.SetActive(true);
    }

    public void HandleInput() { }

    public void OnAnimationEnterEvent() { }

    public void OnAnimationExitEvent() { }

    public void OnAnimationTransitionEvent() { }

    public void OnTriggerEnter(Collider collider) { }

    public void OnTriggerExit(Collider collider) { }

    public void PhysicsUpdate() { }

    public void Update() { }

    private void EndTurn() => stateMachine.ChangeStateWithString(GameStateNames.BattleState);
}