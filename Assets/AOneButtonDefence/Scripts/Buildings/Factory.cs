public class Factory : Building
{
    private FactoryData data;

    protected override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        IncomeCounter.Instance.InstantMaterialsIncome(data.SpawnBonus);
    }

    public override void SetupData(BuildingsData buildingsData)
    {
        data = buildingsData.FactoryData;
        Cost = data.Cost;
        FoodPerTurnAmount = data.FoodPerTurnAmount;
    }

    protected override void RegisterEndMoveAction()
    {
        base.RegisterEndMoveAction();
        IncomeCounter.Instance.AddMaterialsIncome(data.EveryTurnBonus);
    }
}