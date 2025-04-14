using System.Collections;
using UnityEngine;

[RequireComponent(typeof(IAnimatable))]
[RequireComponent(typeof(BuildAnimation))]
public class Building : MonoBehaviour
{
    public Vector3 BuildingOffset => data.SpawnOffset;

    protected BasicBuildingData data;
    protected float AnimationDuration { get; private set; }
    private IAnimatable animator;
    private BuildAnimation startAnimation;

    public void Initialize(BasicBuildingData buildingsData, Vector3 position, float animationDuration)
    {
        data = buildingsData;
        transform.position = position + data.SpawnOffset;
        animator = GetComponent<IAnimatable>();
        startAnimation = GetComponent<BuildAnimation>();
        startAnimation.BuildingAnimation();
        AnimationDuration = animationDuration;
        ActivateSpawnActionWithDelay();
        UpgradeState.UpgradeStateStarted += animator.StartAnimation;
        UpgradeState.UpgradeStateEnding += animator.InteruptAnimation;
    }

    private void ActivateSpawnActionWithDelay() => StartCoroutine(WaitFrameBeforeStartAction());

    private IEnumerator WaitFrameBeforeStartAction()
    {
        // Delay needed to activate spawn action after building position changed.
        yield return null;
        ActivateSpawnAction();
        RegisterEndMoveAction();
        //startAnimation.StartAnimation();
    }

    protected virtual void ActivateSpawnAction()
    {
        foreach (var resourceChange in data.buildResourceChange)
        {
            var position = resourceChange.ResourceAmount.Resource.IsSpawnable ? (Vector3?) (transform.position + data.SpawnOffset) : null;
            ResourceIncomeCounter.Instance.InstantResourceChange(new ResourceAmount(resourceChange.ResourceAmount), position);
            Debug.Log($"Added {resourceChange.ResourceAmount} {resourceChange.ResourceAmount.Resource.Type} resource from {gameObject.name}");
        }
    }

    protected virtual void RegisterEndMoveAction()
    {
        foreach (var resourceChange in data.resourcePerTurnChange)
        {
            ResourceIncomeCounter.Instance.RegisterResourcePerTurnChange(new ResourceAmount(resourceChange.ResourceAmount));
        }
    }

    protected virtual void OnDisable()
    {
        UpgradeState.UpgradeStateStarted -= animator.StartAnimation;
        UpgradeState.UpgradeStateEnding -= animator.InteruptAnimation;
    }
}