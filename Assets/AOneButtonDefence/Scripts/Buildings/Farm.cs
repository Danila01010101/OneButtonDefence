public class Farm : Building
{
    private FarmData data;

    protected override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        IncomeCounter.Instance.InstantFoodIncome(data.EveryTurnBonus);
    }

    public override void SetupData(BuildingsData buildingsData)
    {
        data = buildingsData.FarmData;
        Cost = data.Cost;
        FoodPerTurnAmount = data.FoodPerTurnAmount;
    }

    protected override void RegisterEndMoveAction()
    {
        base.RegisterEndMoveAction();
        IncomeCounter.Instance.AddFoodIncome(data.EveryTurnBonus);
    }
}