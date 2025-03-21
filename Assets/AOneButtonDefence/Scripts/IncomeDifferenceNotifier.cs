using System;

public class IncomeDifferenceNotifier
{
    public static Action<string, bool> FoodIncomeUpdated;
    public static Action<string, bool> MaterialsIncomeUpdated;
    public static Action<string, bool> SpiritIncomeUpdated;
    public static Action<string, bool> WarriorsIncomeUpdated;
    
    private ResourceChanger resourceChanger;

    private void UpdateIncomeValues()
    {
        var (newDifferenceInfo, isPositive) = GetIncomeString(resourceChanger.FoodChange);
        FoodIncomeUpdated?.Invoke(newDifferenceInfo, isPositive);

        (newDifferenceInfo, isPositive) = GetIncomeString(resourceChanger.MaterialsChange);
        MaterialsIncomeUpdated?.Invoke(newDifferenceInfo, isPositive);

        (newDifferenceInfo, isPositive) = GetIncomeString(resourceChanger.SpiritChange);
        SpiritIncomeUpdated?.Invoke(newDifferenceInfo, isPositive);

        (newDifferenceInfo, isPositive) = GetIncomeString(resourceChanger.WarriorChange);
        WarriorsIncomeUpdated?.Invoke(newDifferenceInfo, isPositive);
    }

    private (string, bool) GetIncomeString(int changeValue)
    {
        bool isPositive = changeValue > 0;
        string newDifferenceInfo = isPositive ? $"+ {changeValue}" : $"- {changeValue}";
        return (newDifferenceInfo, isPositive);
    }

    public IncomeDifferenceNotifier(ResourceChanger resourceChanger)
    {
        this.resourceChanger = resourceChanger;
        UpgradeState.UpgradeStateStarted += UpdateIncomeValues;
        UpgradeState.UpgradeStateEnding += UpdateIncomeValues;
    }
}
