using UnityEngine;

[System.Serializable]
public class ResourceAmount
{
    [field: SerializeField] public ResourceData Resource { get; private set; }
    [field: SerializeField] public int Amount { get; private set; }

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

    public void AddResourceAmount(ResourceData.ResourceType resourceType, int amount)
    {
        if (Resource.Type == resourceType)
        {
            Amount += amount;
        }
    }
}