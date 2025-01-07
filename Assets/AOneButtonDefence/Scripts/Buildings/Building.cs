using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(IAnimatable))]
public abstract class Building : MonoBehaviour
{
    [field : SerializeField] public Vector3 BuildingOffset { get; private set; }

    protected float AnimationDuration { get; private set; }

    protected int FoodPerTurnAmount;
    protected int Cost;

    private ResourcesCounter.ResourcesData resources;
    private IAnimatable animator;

    private void Awake()
    {
        animator = GetComponent<IAnimatable>();
    }

    public void ActivateSpawnActionWithDelay() => StartCoroutine(WaitFrameBeforeStartAction());

    private IEnumerator WaitFrameBeforeStartAction()
    {
        //Delay needed to activate spawn action after building position changed.
        yield return null;
        resources = ResourcesCounter.Instance.Data;
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