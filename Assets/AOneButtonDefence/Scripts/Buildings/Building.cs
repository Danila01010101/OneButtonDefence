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
        ActivateSpawnAction();
        RegisterEndMoveAction();
    }

    protected virtual void ActivateSpawnAction()
    {
        ResourceChanger.Instance.InstantMaterialsChange(-Cost);
    }

    public abstract void SetupData(BuildingsData buildingsData);

    public void SetAnimationTime(float animationDuration)
    {
        AnimationDuration = animationDuration;
    }

    protected virtual void RegisterEndMoveAction()
    {
        ResourceChanger.Instance.AddFoodPerTurn(FoodPerTurnAmount);
    }

    protected virtual void OnEnable()
    {
        UpgradeState.UpgradeStateStarted += animator.StartAnimation;
        UpgradeState.UpgradeStateEnding += animator.InteruptAnimation;
    }

    protected virtual void OnDisable()
    {
        UpgradeState.UpgradeStateStarted -= animator.StartAnimation;
        UpgradeState.UpgradeStateEnding -= animator.InteruptAnimation;
    }
}