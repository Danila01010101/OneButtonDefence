using UnityEngine;

public class Building : MonoBehaviour
{
    [field : SerializeField] public Vector3 Offset { get; private set; }

    protected int humanAmount;

    private ResourcesCounter.ResourcesData resources;

    private void Start()
    {
        resources = ResourcesCounter.Instance.Data;
    }

    public virtual void ActivateSpawnAction() { }

    public virtual void SetupData(BuildingsData buildingsData) { }

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