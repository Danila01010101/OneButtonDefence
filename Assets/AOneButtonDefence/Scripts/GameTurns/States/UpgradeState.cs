using System;
using System.Collections;
using UnityEngine;

public class UpgradeState : IState
{
    private GameplayCanvas upgradeUI;
    private IStringStateChanger stateMachine;
    private bool isUpgradeChosen;
    private float upgradePhaseStartTime;
    private bool isCompletingTurn;
    
    private bool CanCompleteTurn => upgradePhaseStartTime + upgradePhaseDuration < Time.time && isUpgradeChosen;
    
    private readonly float upgradePhaseDuration;
    private readonly float upgradePhaseCompletionDelay;

    public static Action UpgradeStateStarted;
    public static Action UpgradeStateEnding;

    public UpgradeState(IStringStateChanger stateMachine, GameplayCanvas upgradeUI, float upgradePhaseDuration, float upgradePhaseCompletionDelay)
    {
        this.upgradePhaseCompletionDelay = upgradePhaseCompletionDelay;
        this.stateMachine = stateMachine;
        this.upgradeUI = upgradeUI;
        this.upgradePhaseDuration = upgradePhaseDuration;
        upgradeUI.gameObject.SetActive(true);
        upgradeUI.UpgradeWindow.gameObject.SetActive(false);
    }

    public void Enter()
    {
        CheckIfStateCanStart();
        isUpgradeChosen = false;
        upgradePhaseStartTime = Time.time;
        upgradeUI.UpgradeWindow.gameObject.SetActive(true);
        UpgradeStateStarted?.Invoke();
        UpgradeButton.UpgradesChoosen += DetectUpgradeChoosing;
    }

    public void Exit()
    {
        upgradeUI.UpgradeWindow.gameObject.SetActive(false);
        UpgradeButton.UpgradesChoosen -= DetectUpgradeChoosing;
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
        if (CanCompleteTurn == false || isCompletingTurn)
            return;

        CoroutineStarter.Instance.StartCoroutine(CompleteUpgradeState());
    }

    private void DetectUpgradeChoosing() 
    {
        upgradeUI.UpgradeButton.Deactivate();
        isUpgradeChosen = true;
    }

    private void CheckIfStateCanStart()
    {
        if (GameResourcesCounter.GetResourceAmount(ResourceData.ResourceType.Material) <= 0)
        {
            stateMachine.ChangeStateWithString(GameStateNames.ResourcesLoseDialogue);
        }

        if (GameResourcesCounter.GetResourceAmount(ResourceData.ResourceType.Food) <= 0)
        {
            stateMachine.ChangeStateWithString(GameStateNames.FoodLoseDialogue);
        }
        
        if (GameResourcesCounter.GetResourceAmount(ResourceData.ResourceType.Spirit) <= 0)
        {
            stateMachine.ChangeStateWithString(GameStateNames.SpiritLoseDialogue);
        }
    }  

    private IEnumerator CompleteUpgradeState()
    {
        isCompletingTurn = true;

        yield return null;

        UpgradeStateEnding?.Invoke();

        yield return new WaitForSeconds(upgradePhaseCompletionDelay);

        stateMachine.ChangeStateWithString(GameStateNames.BattleState);
        isCompletingTurn = false;
    }
}