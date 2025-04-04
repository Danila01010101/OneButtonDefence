using System.Collections.Generic;
using UnityEngine;

public class ResourceIncomeCounter
{
    public static ResourceIncomeCounter Instance { get; private set; }
    
    private readonly GameResourcesCounter gameResourcesCounter;
    private ResourcesKeeper resourcesKeeper;
    private Dictionary<ResourceData.ResourceType, IResourceEffect> resourceEffects;

    public ResourceIncomeCounter(GameResourcesCounter counter, List<ResourceAmount> startResourcesPerTurn, Dictionary<ResourceData.ResourceType, IResourceEffect> resourceEffects)
    {
        resourcesKeeper = new ResourcesKeeper();
        resourcesKeeper.Initialize(startResourcesPerTurn);
        this.resourceEffects = resourceEffects;
        gameResourcesCounter = counter;
        Instance = this;
        Subscribe();
    }

    public int GetResourceIncome(ResourceData.ResourceType resourceType) => resourcesKeeper.GetResourceAmount(resourceType);

    public void InstantResourceChange(ResourceAmount startResourceAmount, Vector3? spawnPosition = null)
    {
        gameResourcesCounter.ChangeResourceAmount(startResourceAmount);
        
        if (resourceEffects.TryGetValue(startResourceAmount.Resource.Type, out IResourceEffect effect))
        {
            effect.ApplyEffect(startResourceAmount.Amount, spawnPosition);
        }
    }

    public void RegisterResourcePerTurnChange(ResourceAmount startResourceAmount) =>
        resourcesKeeper.AddResource(startResourceAmount);

    private void ActivateIncome()
    {
        foreach (var resource in resourcesKeeper.Resources)
        {
            InstantResourceChange(resource);
        }
    }

    private void Subscribe() => UpgradeState.UpgradeStateEnding += ActivateIncome;

    public void Unsubscribe() => UpgradeState.UpgradeStateEnding -= ActivateIncome;
}
