using System.Collections.Generic;
using UnityEngine;

public class GameResourcesCounter : MonoBehaviour
{
    private static GameResourcesCounter instance;
    private ResourcesKeeper resourcesKeeper;

    public void Initialize(List<ResourceAmount> resources)
    {
        resourcesKeeper = new ResourcesKeeper();
        resourcesKeeper.Initialize(resources);
    }
    
    public void ChangeResourceAmount(ResourceAmount resource) => resourcesKeeper.AddResource(resource);

    public int GetResourceAmount(ResourceData.ResourceType type) => resourcesKeeper.GetResourceAmount(type);

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(instance);
        }

        instance = this;
    }
}