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
        // Delay needed to activate spawn action after building position changed and animation is over.
        yield return new WaitForSeconds(AnimationDuration);
        Debug.Log("Position after initialization for warriors is " + transform.position + data.SpawnOffset);
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