using System;
using UnityEngine;

public class UpgradeState : IState
{
    private PartManager upgradeUI;
    private IStringStateChanger stateMachine;
    private bool isUpgradeChoosen = false;
    private float upgradeFaseDuration;
    private float upgradeFaseStartTime;
    private bool canEndTurn => upgradeFaseStartTime + upgradeFaseDuration > Time.time && isUpgradeChoosen;

    public static Action UpgradeStateStarted;
    public static Action UpgradeStateEnded;

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
        UpgradeStateStarted?.Invoke();
        upgradeUI.UpgradeButton.Activate();
        UpgradeButton.UpgradeChoosen += DetectUpgradeChoosing;
    }

    public void Exit()
    {
        upgradeUI.gameObject.SetActive(false);
        UpgradeStateEnded?.Invoke();
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

    private void DetectUpgradeChoosing() 
    {
        upgradeUI.UpgradeButton.Deactivate();
        isUpgradeChoosen = true;
    }
}