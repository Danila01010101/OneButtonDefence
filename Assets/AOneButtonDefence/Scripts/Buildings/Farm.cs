public class Farm : Building
{
    private FarmData data;

    protected override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        ResourceChanger.Instance.InstantFoodChange(data.SpawnBonus);
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
        ResourceChanger.Instance.AddFoodPerTurn(data.EveryTurnBonus);
    }
}