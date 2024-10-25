using System.Collections;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    [field : SerializeField] public Vector3 BuildingOffset { get; private set; }

    protected int FoodPerTurnAmount;
    protected int Cost;
    protected float AnimationDuration { get; private set; }

    private ResourcesCounter.ResourcesData resources;

    private void Start()
    {
        resources = ResourcesCounter.Instance.Data;
    }

    public void ActivateSpawnActionWithDelay() => StartCoroutine(WaitFrameBeforeStartAction());

    private IEnumerator WaitFrameBeforeStartAction()
    {
        //Delay needed to activate spawn action after building position setuped.
        yield return null;
        ActivateSpawnAction();
    }

    protected virtual void ActivateSpawnAction()
    {
        ResourcesCounter.Instance.Data.Materials -= Cost;
    }

    public abstract void SetupData(BuildingsData buildingsData);

    public void SetAnimationTime(float animationDuration)
    {
        AnimationDuration = animationDuration;
    }

    protected virtual void ActivateEndMoveAction()
    {
        resources.FoodAmount -= FoodPerTurnAmount;
    }

    private void OnEnable()
    {
        UpgradeButton.UpgradeChoosen += ActivateEndMoveAction;
    }

    private void OnDisable()
    {
        UpgradeButton.UpgradeChoosen -= ActivateEndMoveAction;
    }
}