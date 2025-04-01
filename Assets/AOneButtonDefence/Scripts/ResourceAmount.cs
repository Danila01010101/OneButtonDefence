[System.Serializable]
public class ResourceAmount
{
    public ResourceData Resource { get; private set; }
    public int Amount { get; private set; }

    public ResourceAmount(ResourceData resource, int amount)
    {
        Resource = resource;
        Amount = amount;
    }

    public void AddResourceAmount(ResourceAmount resourceAmount)
    {
        if (Resource.Type == resourceAmount.Resource.Type)
        {
            Amount += resourceAmount.Amount;
        }
    }
}