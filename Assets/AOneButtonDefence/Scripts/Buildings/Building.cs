using System.Collections;
using UnityEngine;

[RequireComponent(typeof(IAnimatable))]
public abstract class Building : MonoBehaviour
{
    [field : SerializeField] public Vector3 BuildingOffset { get; private set; }

    public enum BuildingType { Empty = 0, MainCastle = 1, Farm = 2, SpiritBuilding = 3, MilitaryCamp = 4, Factory = 5 }

    protected float AnimationDuration { get; private set; }

    protected int FoodPerTurnAmount;
    protected int Cost;

    private ResourcesCounter.ResourcesData resources;
    private IAnimatable animator;

    private void Awake()
    {
        animator = GetComponent<IAnimatable>();
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
        UpgradeState.UpgradeStateStarted += animator.StartAnimation;
        UpgradeState.UpgradeStateEnded += animator.InteruptAnimation;
        UpgradeState.UpgradeStateEnded += ActivateEndMoveAction;
    }

    private void OnDisable()
    {
        UpgradeState.UpgradeStateStarted -= animator.StartAnimation;
        UpgradeState.UpgradeStateEnded -= animator.InteruptAnimation;
        UpgradeState.UpgradeStateEnded -= ActivateEndMoveAction;
    }
}