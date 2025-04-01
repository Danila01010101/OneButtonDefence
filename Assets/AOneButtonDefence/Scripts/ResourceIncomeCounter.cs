using System.Collections.Generic;

public class ResourceIncomeCounter
{
    public static ResourceIncomeCounter Instance { get; private set; }
    
    private readonly GameResourcesCounter gameResourcesCounter;
    private ResourcesKeeper resourcesKeeper;

    public ResourceIncomeCounter(GameResourcesCounter counter, List<ResourceAmount> startResourcesPerTurn)
    {
        resourcesKeeper = new ResourcesKeeper();
        resourcesKeeper.Initialize(startResourcesPerTurn);
        gameResourcesCounter = counter;
        Instance = this;
        Subscribe();
    }

    public void InstantResourceChange(ResourceAmount resourceAmount) => gameResourcesCounter.ChangeResourceAmount(resourceAmount);

    public void RegisterResourcePerTurnChange(ResourceAmount resourceAmount) =>
        resourcesKeeper.AddResource(resourceAmount);

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
