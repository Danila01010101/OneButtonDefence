using System;

public interface IResourceView
{
    public void Initialize(ResourceData.ResourceType resourceType, Action<string, bool> incomeUpdated);
    public void UpdateValue();
    public void UpdateTurnIncomeValue(string newValue, bool isPositive);
}