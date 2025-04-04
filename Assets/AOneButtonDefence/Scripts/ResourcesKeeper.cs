using System;
using System.Collections.Generic;
using System.Linq;

public class ResourcesKeeper
{
    public IReadOnlyList<ResourceAmount> Resources => resources;
    private List<ResourceAmount> resources = new List<ResourceAmount>();

    public void Initialize(List<ResourceAmount> startResources)
    {
        resources = startResources;
    }

    public void AddResource(ResourceAmount resourceAmount)
    {
        var resource = resources.FirstOrDefault(r => r.Resource.Type == resourceAmount.Resource.Type);

        if (resource == null)
            throw new Exception("No such resource registered.");
        
        resource.AddResourceAmount(resource);
    }

    public void AddResourceByType(ResourceData.ResourceType resourceType, int amount)
    {
        var resource = resources.FirstOrDefault(r => r.Resource.Type == resourceType);

        if (resource == null)
            throw new Exception("No such resource registered.");
        
        resource.AddResourceAmount(resourceType, amount);
    }

    public int GetResourceAmount(ResourceData.ResourceType type)
    {
        var resource = resources.FirstOrDefault(r => r.Resource.Type == type);

        if (resource == null)
            throw new Exception("No such resource registered.");
        
        return resource.Amount;
    }
}