using System;

public class IncomeDifferenceTextConverter
{
    public static Action<ResourceData.ResourceType, string, bool> ResourceIncomeChanged;
    
    private ResourceIncomeCounter ResourceIncomeCounter => ResourceIncomeCounter.Instance;

    private void UpdateIncomeValues()
    {
        var (newDifferenceInfo, isPositive) = GetIncomeString(ResourceIncomeCounter.GetResourceIncome(ResourceData.ResourceType.Food));
        ResourceIncomeChanged?.Invoke(ResourceData.ResourceType.Food, newDifferenceInfo, isPositive);

        (newDifferenceInfo, isPositive) = GetIncomeString(ResourceIncomeCounter.GetResourceIncome(ResourceData.ResourceType.Material));
        ResourceIncomeChanged?.Invoke(ResourceData.ResourceType.Material, newDifferenceInfo, isPositive);

        (newDifferenceInfo, isPositive) = GetIncomeString(ResourceIncomeCounter.GetResourceIncome(ResourceData.ResourceType.Spirit));
        ResourceIncomeChanged?.Invoke(ResourceData.ResourceType.Spirit, newDifferenceInfo, isPositive);

        (newDifferenceInfo, isPositive) = GetIncomeString(ResourceIncomeCounter.GetResourceIncome(ResourceData.ResourceType.Warrior));
        ResourceIncomeChanged?.Invoke(ResourceData.ResourceType.Warrior, newDifferenceInfo, isPositive);
    }

    private (string, bool) GetIncomeString(int changeValue)
    {
        bool isPositive = changeValue >= 0;
        string newDifferenceInfo = isPositive ? $"+ {changeValue}" : $"- {changeValue}";
        return (newDifferenceInfo, isPositive);
    }

    public void Unsubscribe()
    {
        UpgradeState.UpgradeStateStarted -= UpdateIncomeValues;
        UpgradeState.UpgradeStateEnding -= UpdateIncomeValues;
    }
    
    public IncomeDifferenceTextConverter()
    {
        UpgradeState.UpgradeStateStarted += UpdateIncomeValues;
        UpgradeState.UpgradeStateEnding += UpdateIncomeValues;
    }
}
