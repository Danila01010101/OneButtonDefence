public interface IResourceView
{
    public void Initialize(ResourceData.ResourceType resourceType);
    public void UpdateValue();
    public void UpdateTurnIncomeValue(string newValue, bool isPositive);
}