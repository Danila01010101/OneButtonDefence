using System;

public class IncomeDifferenceNotifier
{
    public static Action<string, bool> FoodIncomeUpdated;
    public static Action<string, bool> MaterialsIncomeUpdated;
    public static Action<string, bool> SpiritIncomeUpdated;
    public static Action<string, bool> WarriorsIncomeUpdated;

    private void UpdateIncomeValues()
    {
        var (newDifferenceInfo, isPositive) = GetIncomeString(ResourceChanger.Instance.FoodChange);
        FoodIncomeUpdated?.Invoke(newDifferenceInfo, isPositive);

        (newDifferenceInfo, isPositive) = GetIncomeString(ResourceChanger.Instance.FoodChange);
        MaterialsIncomeUpdated?.Invoke(newDifferenceInfo, isPositive);

        (newDifferenceInfo, isPositive) = GetIncomeString(ResourceChanger.Instance.FoodChange);
        SpiritIncomeUpdated?.Invoke(newDifferenceInfo, isPositive);

        (newDifferenceInfo, isPositive) = GetIncomeString(ResourceChanger.Instance.FoodChange);
        WarriorsIncomeUpdated?.Invoke(newDifferenceInfo, isPositive);
    }

    private (string, bool) GetIncomeString(int changeValue)
    {
        bool isPositive = changeValue > 0;
        string newDifferenceInfo = isPositive ? $"+ {changeValue}" : $"- {changeValue}";
        return (newDifferenceInfo, isPositive);
    }

    public IncomeDifferenceNotifier() 
    {
        UpgradeState.UpgradeStateStarted += UpdateIncomeValues;
    }
}
