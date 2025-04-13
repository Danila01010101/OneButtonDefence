public class ResourceAmount
{
    public ResourceData Resource => resourceData;
    public int Amount { get; private set; }
    
    private readonly ResourceData resourceData;

    public ResourceAmount(ResourceData resource, int amount)
    {
        resourceData = resource;
        Amount = amount;
    }

    public ResourceAmount(StartResourceAmount resourceAmount)
    {
        resourceData = resourceAmount.Resource;
        Amount = resourceAmount.Amount;
    }

    public void AddResourceAmount(ResourceAmount startResourceAmount)
    {
        if (Resource.Type == startResourceAmount.Resource.Type)
        {
            Amount += startResourceAmount.Amount;
        }
    }

    public void AddResourceAmount(ResourceData.ResourceType resourceType, int amount)
    {
        if (Resource.Type == resourceType)
        {
            Amount += amount;
        }
    }
}