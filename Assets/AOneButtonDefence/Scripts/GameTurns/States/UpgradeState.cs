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
    public static Action UpgradeStateEnded;

    public UpgradeState(IStringStateChanger stateMachine, GameplayCanvas upgradeUI, float upgradePhaseDuration, float upgradePhaseCompletionDelay)
    {
        this.stateMachine = stateMachine;
        this.upgradeUI = upgradeUI;
        this.upgradePhaseDuration = upgradePhaseDuration;
        upgradeUI.gameObject.SetActive(false);
    }

    public void Enter()
    {
        isUpgradeChosen = false;
        upgradePhaseStartTime = Time.time;
        upgradeUI.gameObject.SetActive(true);
        UpgradeStateStarted?.Invoke();
        upgradeUI.UpgradeButton.Activate();
        UpgradeButton.UpgradesChoosen += DetectUpgradeChoosing;
    }

    public void Exit()
    {
        upgradeUI.gameObject.SetActive(false);
        UpgradeStateEnded?.Invoke();
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

    private IEnumerator CompleteUpgradeState()
    {
        isCompletingTurn = true;
        
        yield return new WaitForSeconds(upgradePhaseCompletionDelay);
        
        if (ResourcesCounter.Instance.Data.Materials <= 0)
        {
            stateMachine.ChangeStateWithString(GameStateNames.ResourcesLoseDialogue);
            yield break;
        }

        if (ResourcesCounter.Instance.Data.FoodAmount <= 0)
        {
            stateMachine.ChangeStateWithString(GameStateNames.FoodLoseDialogue);
            yield break;
        }
        
        if (ResourcesCounter.Instance.Data.SurvivorSpirit <= 0)
        {
            stateMachine.ChangeStateWithString(GameStateNames.SpiritLoseDialogue);
            yield break;
        }
        
        stateMachine.ChangeStateWithString(GameStateNames.BattleState);
        isCompletingTurn = false;
    }
}