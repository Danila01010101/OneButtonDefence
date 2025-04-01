public interface IResourceView
{
    public void Initialize(ResourceData resourceData);
    public void UpdateValue();
    public void UpdateTurnIncomeValue(string newValue, bool isPositive);
}