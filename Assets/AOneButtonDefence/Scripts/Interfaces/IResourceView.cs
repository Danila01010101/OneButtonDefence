using System;

public interface IResourceView
{
    public void Initialize(ResourceData.ResourceType resourceType);
    public void UpdateValue();
    public void UpdateTurnIncomeValue(ResourceData.ResourceType type, string newValue, bool isPositive);
}