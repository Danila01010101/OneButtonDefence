using UnityEngine;

public class DialogState : IState
{
    private DialogueSystem startDialogPrefab;
    private DialogueSystem spawnedDialogCanvas;
    private Canvas gamePlayCanvas;

    public DialogState(DialogueSystem newDialog)
    {
        startDialogPrefab = newDialog;
    }

    public void Enter()
    {
        SpawnDialogCanvas();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void HandleInput()
    {
        throw new System.NotImplementedException();
    }

    public void OnAnimationEnterEvent()
    {
        throw new System.NotImplementedException();
    }

    public void OnAnimationExitEvent()
    {
        throw new System.NotImplementedException();
    }

    public void OnAnimationTransitionEvent()
    {
        throw new System.NotImplementedException();
    }

    public void OnTriggerEnter(Collider collider)
    {
        throw new System.NotImplementedException();
    }

    public void OnTriggerExit(Collider collider)
    {
        throw new System.NotImplementedException();
    }

    public void PhysicsUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }

    private void SpawnDialogCanvas() => spawnedDialogCanvas = MonoBehaviour.Instantiate(startDialogPrefab);

    public void RemoveStartCanvas() => MonoBehaviour.Destroy(spawnedDialogCanvas.gameObject);
}