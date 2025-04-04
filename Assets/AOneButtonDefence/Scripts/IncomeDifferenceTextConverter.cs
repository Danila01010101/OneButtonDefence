using System;

public class IncomeDifferenceTextConverter
{
    public static Action<string, bool> FoodIncomeUpdated;
    public static Action<string, bool> MaterialsIncomeUpdated;
    public static Action<string, bool> SpiritIncomeUpdated;
    public static Action<string, bool> WarriorsIncomeUpdated;
    
    private ResourceIncomeCounter ResourceIncomeCounter => ResourceIncomeCounter.Instance;

    private void UpdateIncomeValues()
    {
        var (newDifferenceInfo, isPositive) = GetIncomeString(ResourceIncomeCounter.GetResourceIncome(ResourceData.ResourceType.Food));
        FoodIncomeUpdated?.Invoke(newDifferenceInfo, isPositive);

        (newDifferenceInfo, isPositive) = GetIncomeString(ResourceIncomeCounter.GetResourceIncome(ResourceData.ResourceType.Material));
        MaterialsIncomeUpdated?.Invoke(newDifferenceInfo, isPositive);

        (newDifferenceInfo, isPositive) = GetIncomeString(ResourceIncomeCounter.GetResourceIncome(ResourceData.ResourceType.Spirit));
        SpiritIncomeUpdated?.Invoke(newDifferenceInfo, isPositive);

        (newDifferenceInfo, isPositive) = GetIncomeString(ResourceIncomeCounter.GetResourceIncome(ResourceData.ResourceType.Warrior));
        WarriorsIncomeUpdated?.Invoke(newDifferenceInfo, isPositive);
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
