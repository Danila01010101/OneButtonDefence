using System;
using System.Collections.Generic;
using UnityEngine;

public class GameResourcesCounter : MonoBehaviour
{
    private static GameResourcesCounter instance;
    private ResourcesKeeper resourcesKeeper;

    public static Action ResourceAdded;

    public void Initialize(List<ResourceAmount> resources)
    {
        resourcesKeeper = new ResourcesKeeper();
        resourcesKeeper.Initialize(resources);
    }
    
    public void ChangeResourceAmount(ResourceAmount startResource)
    {
        resourcesKeeper.AddResource(startResource);
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