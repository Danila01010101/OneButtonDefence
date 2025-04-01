using System;

public class IncomeDifferenceTextConverter
{
    public static Action<string, bool> FoodIncomeUpdated;
    public static Action<string, bool> MaterialsIncomeUpdated;
    public static Action<string, bool> SpiritIncomeUpdated;
    public static Action<string, bool> WarriorsIncomeUpdated;
    
    private ResourceIncomeCounter resourceIncomeCounter;

    private void UpdateIncomeValues()
    {
        var (newDifferenceInfo, isPositive) = GetIncomeString(resourceIncomeCounter.FoodChange);
        FoodIncomeUpdated?.Invoke(newDifferenceInfo, isPositive);

        (newDifferenceInfo, isPositive) = GetIncomeString(resourceIncomeCounter.MaterialsChange);
        MaterialsIncomeUpdated?.Invoke(newDifferenceInfo, isPositive);

        (newDifferenceInfo, isPositive) = GetIncomeString(resourceIncomeCounter.SpiritChange);
        SpiritIncomeUpdated?.Invoke(newDifferenceInfo, isPositive);

        (newDifferenceInfo, isPositive) = GetIncomeString(resourceIncomeCounter.WarriorChange);
        WarriorsIncomeUpdated?.Invoke(newDifferenceInfo, isPositive);
    }

    private (string, bool) GetIncomeString(int changeValue)
    {
        bool isPositive = changeValue >= 0;
        string newDifferenceInfo = isPositive ? $"+ {changeValue}" : $"- {changeValue}";
        return (newDifferenceInfo, isPositive);
    }

    public IncomeDifferenceTextConverter(ResourceIncomeCounter resourceIncomeCounter)
    {
        this.resourceIncomeCounter = resourceIncomeCounter;
        UpgradeState.UpgradeStateStarted += UpdateIncomeValues;
        UpgradeState.UpgradeStateEnding += UpdateIncomeValues;
    }
}
