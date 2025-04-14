using System.Collections;
using UnityEngine;

[RequireComponent(typeof(IAnimatable))]
[RequireComponent(typeof(BuildAnimation))]
public class Building : MonoBehaviour
{
    public Vector3 BuildingOffset => data.SpawnOffset;

    protected BasicBuildingData data;

    private Vector3 resourceSpawnPosition;
    private IAnimatable animator;
    private BuildAnimation startAnimation;
    
    protected float AnimationDuration { get; private set; }

    public void Initialize(BasicBuildingData buildingsData, Vector3 position, float animationDuration)
    {
        data = buildingsData;
        transform.position = position + data.SpawnOffset;
        resourceSpawnPosition = transform.position + data.SpawnOffset;
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
        yield return null;
        ActivateSpawnAction();
        RegisterEndMoveAction();
        //startAnimation.StartAnimation();
    }

    protected virtual void ActivateSpawnAction()
    {
        foreach (var resourceChange in data.buildResourceChange)
        {
            ResourceIncomeCounter.Instance.InstantResourceChange(new ResourceAmount(resourceChange.ResourceAmount), resourceSpawnPosition);
        }
    }

    protected virtual void RegisterEndMoveAction()
    {
        foreach (var resourceChange in data.resourcePerTurnChange)
        {
            var resourceAmount = new ResourceAmount(resourceChange.ResourceAmount);
            
            if (resourceChange.ResourceAmount.Resource.IsSpawnable)
            {
                resourceAmount.SetResourceSpawnPosition(resourceSpawnPosition);
            }
            
            ResourceIncomeCounter.Instance.RegisterResourcePerTurnChange(resourceAmount);
        }
    }

    protected virtual void OnDisable()
    {
        UpgradeState.UpgradeStateStarted -= animator.StartAnimation;
        UpgradeState.UpgradeStateEnding -= animator.InteruptAnimation;
    }
}