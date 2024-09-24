using UnityEngine;

public class Building : MonoBehaviour
{
    [field : SerializeField] public Vector3 Offset { get; private set; }

    [SerializeField] protected BuildingsData BuildingsData { get; private set; }
    [SerializeField] protected int humanAmount = 1;

    private ResourcesCounter.ResourcesData resources;

    private void Start()
    {
        resources = ResourcesCounter.Instance.Data;
    }

    public virtual void ActivateSpawnAction() { }

    protected virtual void SetupData() { }

    protected virtual void ActivateEndMoveAction()
    {
        resources.FoodAmount -= humanAmount;
    }

    private void OnEnable()
    {
        UpgradeButton.TurnEnded += ActivateEndMoveAction;
    }

    private void OnDisable()
    {
        UpgradeButton.TurnEnded -= ActivateEndMoveAction;
    }
}