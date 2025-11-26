using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceIncomeCounter
{
    public static ResourceIncomeCounter Instance { get; private set; }
    
    private readonly GameResourcesCounter gameResourcesCounter;
    private ResourcesKeeper resourcesTurnIncomeKeeper;
    private Dictionary<ResourceData.ResourceType, IResourceEffect> resourceEffects;

    public ResourceIncomeCounter(GameResourcesCounter counter, List<ResourceAmount> startResourcesPerTurn, Dictionary<ResourceData.ResourceType, IResourceEffect> resourceEffects)
    {
        resourcesTurnIncomeKeeper = new ResourcesKeeper();
        resourcesTurnIncomeKeeper.Initialize(startResourcesPerTurn);
        this.resourceEffects = resourceEffects;
        gameResourcesCounter = counter;
        Instance = this;
        Subscribe();
    }

    public int GetResourceIncome(ResourceData.ResourceType resourceType) => (int)(resourcesTurnIncomeKeeper.GetResourceAmount(resourceType) * GameResourcesCounter.ResourcesBuffMultiplier);

    public void InstantResourceChange(ResourceAmount startResourceAmount, Vector3? spawnPosition = null)
    {
        ResourceAmount resourceWithBuff = startResourceAmount;
        
        if (startResourceAmount.Amount > 0)
            resourceWithBuff = new ResourceAmount(startResourceAmount.Resource, (int)(startResourceAmount.Amount * GameResourcesCounter.ResourcesBuffMultiplier), startResourceAmount.ResourceSpawnPositon);
        
        gameResourcesCounter.ChangeResourceAmount(resourceWithBuff);
        
        if (resourceEffects.TryGetValue(startResourceAmount.Resource.Type, out IResourceEffect effect))
        {
            if (spawnPosition.HasValue)
            {
                effect.ApplyEffect(resourceWithBuff.Amount, spawnPosition);
            }
            else
            {
                effect.ApplyEffect(startResourceAmount.Amount, startResourceAmount.ResourceSpawnPositon);
            }
        }
    }

    public void RegisterResourcePerTurnChange(ResourceAmount startResourceAmount) =>
        resourcesTurnIncomeKeeper.AddResource(startResourceAmount);

    private void ActivateIncome()
    {
        var groupedResources = resourcesTurnIncomeKeeper.Resources
            .GroupBy(r => new { r.Resource.Type, r.ResourceSpawnPositon })
            .Select(g =>
            {
                var resource = g.First().Resource;
                var totalAmount = g.Sum(r => r.Amount);
                var combined = new ResourceAmount(resource, totalAmount);
                combined.SetResourceSpawnPosition(g.Key.ResourceSpawnPositon);
                return combined;
            });

        foreach (var resource in groupedResources)
        {
            InstantResourceChange(resource, resource.ResourceSpawnPositon);
        }
    }

    private void Subscribe() => UpgradeState.UpgradeStateEnding += ActivateIncome;

    public void Unsubscribe() => UpgradeState.UpgradeStateEnding -= ActivateIncome;
}