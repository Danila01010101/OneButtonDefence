using System;
using System.Collections.Generic;
using UnityEngine;

public class GameResourcesCounter : MonoBehaviour
{
    private static GameResourcesCounter instance;
    private ResourcesKeeper resourcesKeeper;

    public static Action ResourceAdded;
    
    private int ResourcesBuffMultiplier => 1 + instance.resourcesKeeper.GetResourceAmount(ResourceData.ResourceType.CurrentResourcesBuff) / 100;

    public void Initialize(List<ResourceAmount> resources)
    {
        resourcesKeeper = new ResourcesKeeper();
        resourcesKeeper.Initialize(resources);
    }
    
    public void ChangeResourceAmount(ResourceAmount resourceAmount)
    {
        var resourceWithBuff = new ResourceAmount(resourceAmount.Resource,
            resourceAmount.Amount * ResourcesBuffMultiplier);
        resourcesKeeper.AddResource(resourceWithBuff);
        ResourceAdded?.Invoke();
    }

    public static int GetResourceAmount(ResourceData.ResourceType type) => instance.resourcesKeeper.GetResourceAmount(type);

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(instance);
        }

        instance = this;
    }
}