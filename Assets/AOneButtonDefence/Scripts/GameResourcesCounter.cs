using System.Collections.Generic;
using UnityEngine;

public class GameResourcesCounter : MonoBehaviour
{
    public static IReadOnlyList<ResourceAmount> ResourceAmounts => instance.resourcesKeeper.Resources;
    
    private static GameResourcesCounter instance;
    private ResourcesKeeper resourcesKeeper;

    public void Initialize(List<ResourceAmount> resources)
    {
        resourcesKeeper = new ResourcesKeeper();
        resourcesKeeper.Initialize(resources);
    }
    
    public void ChangeResourceAmount(ResourceAmount resource) => resourcesKeeper.AddResource(resource);

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(instance);
        }

        instance = this;
    }
}