using UnityEngine;

public abstract class Building : MonoBehaviour
{
    [field : SerializeField] public Vector3 Offset { get; private set; }

    protected int foodPerTurnAmount;
    protected int cost;

    private ResourcesCounter.ResourcesData resources;

    private void Start()
    {
        resources = ResourcesCounter.Instance.Data;
    }

    public virtual void ActivateSpawnAction()
    {
        ResourcesCounter.Instance.Data.Materials -= cost;
    }

    public abstract void SetupData(BuildingsData buildingsData);

    protected virtual void ActivateEndMoveAction()
    {
        resources.FoodAmount -= foodPerTurnAmount;
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