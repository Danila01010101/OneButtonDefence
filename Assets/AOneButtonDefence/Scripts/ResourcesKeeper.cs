using System;
using System.Collections.Generic;
using System.Linq;

public class ResourcesKeeper
{
    public IReadOnlyList<ResourceAmount> Resources => resources;
    private List<ResourceAmount> resources;

    public void Initialize(List<ResourceAmount> resources)
    {
        this.resources = resources;
    }

    public void AddResource(ResourceAmount resourceAmount)
    {
        var resource = resources.FirstOrDefault(r => r.Resource.Type == resourceAmount.Resource.Type);

        if (resource == null)
            throw new Exception("No such resource registered.");
        
        resource.AddResourceAmount(resourceAmount);
    }

    public int GetResourceAmount(ResourceData.ResourceType type)
    {
        var resource = resources.FirstOrDefault(r => r.Resource.Type == type);

        if (resource == null)
            throw new Exception("No such resource registered.");
        
        return resource.Amount;
    }
}