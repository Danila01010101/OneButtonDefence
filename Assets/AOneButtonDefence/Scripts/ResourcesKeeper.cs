using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourcesKeeper
{
    public IReadOnlyList<ResourceAmount> Resources => resources;
    private List<ResourceAmount> resources = new List<ResourceAmount>();

    public void Initialize(List<ResourceAmount> startResources)
    {
        foreach (var resource in startResources)
        {
            resources.Add(new ResourceAmount(resource.Resource, resource.Amount));
        }
    }

    public void AddResource(ResourceAmount resourceAmount)
    {
        var resource = resources.FirstOrDefault(r => r.Resource.Type == resourceAmount.Resource.Type && r.ResourceSpawnPositon == resourceAmount.ResourceSpawnPositon);

        if (resource == null)
        {
            var newResource = new ResourceAmount(resourceAmount.Resource, resourceAmount.Amount);
            newResource.SetResourceSpawnPosition(resourceAmount.ResourceSpawnPositon);
            resources.Add(newResource);
        }
        else
        {
            resource.AddResourceAmount(resourceAmount);
        }
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
        var foundResources = resources.FindAll(r => r.Resource.Type == type);

        if (foundResources == null)
            throw new Exception("No such resource registered.");
        
        int resourceAmount = foundResources.Sum(r => r.Amount);

        if (type == ResourceData.ResourceType.CurrentResourcesBuff)
        {
            return resourceAmount;
        }
        
        return resourceAmount;
    }
}