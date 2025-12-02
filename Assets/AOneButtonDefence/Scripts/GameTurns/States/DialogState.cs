using System;
using UnityEngine;

public class DialogState : IState
{
    private readonly IStringStateChanger stateMachine;
    private readonly DialogueSystem startDialogPrefab;
    private readonly IDisableableInput input;
    private readonly string nextState;
    private readonly bool isDialogAnimatable;
    private readonly GameObject resourceWindow;
    private DialogueSystem spawnedDialog;
    private Canvas gamePlayCanvas;

    public static Action AnimatableDialogueStarted;
    public static Action AnimatableDialogueEnded;

    public DialogState(IStringStateChanger stateMachine, GameObject resourceWindow, DialogueSystem newDialog, string nextState, IDisableableInput input, bool isDialogAnimatable = false)
    {
        this.stateMachine = stateMachine;
        this.resourceWindow = resourceWindow;
        startDialogPrefab = newDialog;
        this.nextState = nextState;
        this.input = input;
        this.isDialogAnimatable = isDialogAnimatable;
    }

    public void Enter()
    {
        input.Disable();
        SpawnDialogCanvas();
        
        if (isDialogAnimatable)
        {
            resourceWindow?.gameObject.SetActive(false);
            AnimatableDialogueStarted.Invoke();
        }
    }

    public void Exit()
    {
        input.Enable();
        RemoveDialogCanvas();
        
        if (isDialogAnimatable)
        {
            resourceWindow?.gameObject.SetActive(true);
            AnimatableDialogueEnded.Invoke();
        }
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

    private void RemoveDialogCanvas() => MonoBehaviour.Destroy(spawnedDialog.gameObject);

    private void EndDialog()
    {
        spawnedDialog.DialogEnded -= EndDialog;
        stateMachine.ChangeStateWithString(nextState);
    }
}