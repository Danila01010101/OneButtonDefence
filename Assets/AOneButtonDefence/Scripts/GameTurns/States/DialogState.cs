using UnityEngine;

public class DialogState : IState
{
    private IStringStateChanger stateMachine;
    private DialogueSystem startDialogPrefab;
    private DialogueSystem spawnedDialog;
    private Canvas gamePlayCanvas;
    private Camera mainCamera;

    public GameObject DialogueGnomePrefab;  // ������ ������ �������
    public Vector2 viewportPosition;  // ������� � Viewport (�������� �� 0 �� 1)
    public float distanceFromCamera = 5f; // ���������� �� ������

    public DialogState(IStringStateChanger stateMachine, DialogueSystem newDialog)
    {
        this.stateMachine = stateMachine;
        startDialogPrefab = newDialog;
    }

    public void Enter()
    {
        SpawnDialogCanvas();
        SpawnGnome();
    }

    public void Exit()
    {
        RemoveStartCanvas();
    }

    public void SpawnGnome() 
    {
        mainCamera = Camera.main;

        // ������� � ������������ ������
        Vector3 spawnPosition = new Vector3(viewportPosition.x, viewportPosition.y, distanceFromCamera);

        // �������������� Viewport � ������� ����������
        Vector3 worldPosition = mainCamera.ViewportToWorldPoint(spawnPosition);

        // ����� ������� � ���� �������
        MonoBehaviour.Instantiate(DialogueGnomePrefab, worldPosition, Quaternion.identity);
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