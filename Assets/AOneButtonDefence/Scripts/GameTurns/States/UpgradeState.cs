using UnityEngine;

public class UpgradeState : IState
{
    private PartManager upgradeUI;
    private IStringStateChanger stateMachine;

    public UpgradeState(IStringStateChanger stateMachine, PartManager upgradeUI)
    {
        this.stateMachine = stateMachine;
        this.upgradeUI = upgradeUI;
        upgradeUI.gameObject.SetActive(false);
    }

    public void Enter()
    {
        upgradeUI.gameObject.SetActive(true);
        UpgradeButton.TurnEnded += EndTurn;
    }

    public void Exit()
    {
        upgradeUI.gameObject.SetActive(false);
        UpgradeButton.TurnEnded -= EndTurn;
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