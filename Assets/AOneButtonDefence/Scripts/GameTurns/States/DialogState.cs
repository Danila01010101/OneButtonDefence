using UnityEngine;

public class DialogState : IState
{
    private IStringStateChanger stateMachine;
    private DialogueSystem startDialogPrefab;
    private DialogueSystem spawnedDialog;
    private Canvas gamePlayCanvas;

    public DialogState(IStringStateChanger stateMachine, DialogueSystem newDialog)
    {
        this.stateMachine = stateMachine;
        startDialogPrefab = newDialog;
    }

    public void Enter()
    {
        SpawnDialogCanvas();
    }

    public void Exit()
    {
        RemoveStartCanvas();
    }

    public void HandleInput() { }

    public void OnAnimationEnterEvent() { } 

    public void OnAnimationExitEvent() { }

    public void OnAnimationTransitionEvent() { }

    public void OnTriggerEnter(Collider collider) { }

    public void OnTriggerExit(Collider collider) { }

    public void PhysicsUpdate() { }

    public void Update() { }

    private void SpawnDialogCanvas()
    { 
        spawnedDialog = MonoBehaviour.Instantiate(startDialogPrefab);
        spawnedDialog.DialogEnded += EndDialog;
    }

    private void RemoveStartCanvas() => MonoBehaviour.Destroy(spawnedDialog.gameObject);

    private void EndDialog()
    {
        spawnedDialog.DialogEnded -= EndDialog;
        stateMachine.ChangeStateWithString(GameStateNames.Upgrade);
    }
}