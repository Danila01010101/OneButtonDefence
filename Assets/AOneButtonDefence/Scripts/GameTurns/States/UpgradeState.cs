using UnityEngine;

public class UpgradeState : IState
{
    private PartManager upgradeUI;
    private IStringStateChanger stateMachine;
    private bool isUpgradeChoosen = false;
    private float upgradeFaseDuration;
    private float upgradeFaseStartTime;
    private bool canEndTurn => upgradeFaseStartTime + upgradeFaseDuration > Time.time && isUpgradeChoosen;

    public UpgradeState(IStringStateChanger stateMachine, PartManager upgradeUI, float upgradeFaseDuration)
    {
        this.stateMachine = stateMachine;
        this.upgradeUI = upgradeUI;
        this.upgradeFaseDuration = upgradeFaseDuration;
        upgradeUI.gameObject.SetActive(false);
    }

    public void Enter()
    {
        isUpgradeChoosen = false;
        upgradeFaseStartTime = Time.time;
        upgradeUI.gameObject.SetActive(true);
        UpgradeButton.UpgradeChoosen += DetectUpgradeChoosing;
    }

    public void Exit()
    {
        upgradeUI.gameObject.SetActive(false);
        UpgradeButton.UpgradeChoosen -= DetectUpgradeChoosing;
    }

    public void HandleInput() { }

    public void OnAnimationEnterEvent() { }

    public void OnAnimationExitEvent() { }

    public void OnAnimationTransitionEvent() { }

    public void OnTriggerEnter(Collider collider) { }

    public void OnTriggerExit(Collider collider) { }

    public void PhysicsUpdate() { }

    public void Update()
    {
        if (canEndTurn)
        {
            stateMachine.ChangeStateWithString(GameStateNames.BattleState);
        }
    }

    private void DetectUpgradeChoosing() => isUpgradeChoosen = true;
}