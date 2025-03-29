[System.Serializable]
public class ResourceAmount
{
    public ResourceData resource;
    public int amount;

    public ResourceAmount(ResourceData resource, int amount)
    {
        this.resource = resource;
        this.amount = amount;
    }
}