public class Farm : Building
{
    protected override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        ResourceIncomeCounter.Instance.InstantFoodChange(data.SpawnBonus);
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
        ResourceIncomeCounter.Instance.AddFoodPerTurn(data.EveryTurnBonus);
    }
}