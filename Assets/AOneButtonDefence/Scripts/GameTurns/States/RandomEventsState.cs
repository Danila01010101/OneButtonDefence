using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomEventsState : IState
{
    private readonly IStringStateChanger stateMachine;
    private readonly List<TradeDialogueData> dialogues;
    private readonly TradeDialogueSystem tradeDialoguePrefab;
    private readonly int eventsTurnInterval;
    private readonly string nextStateName;
    
    private List<TradeDialogueData> unusedDialogues = new List<TradeDialogueData>();
    private GameResourcesCounter resourcesCounter;
    private TradeDialogueSystem spawnedDialog;
    private int eventsTurnCounter;

    public RandomEventsState(IStringStateChanger stateMachine, List<TradeDialogueData> allDialogs, GameResourcesCounter resourcesCounter, TradeDialogueSystem tradeDialoguePrefab, int eventsTurnInterval, string nextStateName)
    {
        dialogues = allDialogs;
        this.stateMachine = stateMachine;
        this.resourcesCounter = resourcesCounter;
        this.tradeDialoguePrefab = tradeDialoguePrefab;
        this.nextStateName = nextStateName;
        this.eventsTurnInterval = eventsTurnInterval;
    }
    
    public void Enter()
    {
        if (eventsTurnCounter++ < eventsTurnInterval)
        {
            stateMachine.ChangeStateWithString(GameStateNames.Upgrade);
            return;
        }
        
        spawnedDialog = MonoBehaviour.Instantiate(tradeDialoguePrefab);

        if (unusedDialogues.Count == 0)
            unusedDialogues = dialogues.ToList();
        
        int randomDialogueIndex = Random.Range(0, unusedDialogues.Count);
        
        spawnedDialog.Initialize();
        spawnedDialog.SetupTradeDialogueComponents(resourcesCounter, unusedDialogues[randomDialogueIndex]);
        unusedDialogues.RemoveAt(randomDialogueIndex);
        spawnedDialog.DialogEnded += FinishTrade;
    }

    public void Exit()
    {
        if (spawnedDialog != null)
            spawnedDialog.DialogEnded -= FinishTrade;
    }

    public void HandleInput() { }

    public void Update() { }

    public void PhysicsUpdate() { }

    public void OnTriggerEnter(Collider collider) { }

    public void OnTriggerExit(Collider collider) { }

    public void OnAnimationEnterEvent() { }

    public void OnAnimationExitEvent() { }

    public void OnAnimationTransitionEvent() { }

    private void FinishTrade()
    {
        stateMachine.ChangeStateWithString(nextStateName);
    }
}
